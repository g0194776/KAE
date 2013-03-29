using KJFramework.Messages.Contracts;
using KJFramework.Messages.Engine;
using KJFramework.Messages.Exceptions;
using KJFramework.Messages.Objects;
using KJFramework.Messages.Proxies;
using KJFramework.Messages.TypeProcessors;
using KJFramework.Messages.TypeProcessors.Maps;
using System;
using System.Collections.Concurrent;

namespace KJFramework.Messages.Helpers
{
    /// <summary>
    ///     数据帮助器，提供了相关的基本操作。
    /// </summary>
    public static class DataHelper
    {
        #region Members

        private static readonly ConcurrentDictionary<string, Action<IMemorySegmentProxy, object>> _serializers = new ConcurrentDictionary<string, Action<IMemorySegmentProxy, object>>();
        private static readonly ConcurrentDictionary<string, Func<byte[], object>> _deserializers = new ConcurrentDictionary<string, Func<byte[], object>>(); 

        #endregion

        #region Methods

        /// <summary>
        ///     将指定类型的实例序列化为二进制数据
        /// </summary>
        /// <param name="instance">实例对象</param>
        /// <returns>返回序列化后的二进制数据, 如果instance为null, 则返回null</returns>
        public static byte[] ToBytes(object instance)
        {
            return ToBytes(instance.GetType(), instance);
        }
        
        /// <summary>
        ///     将指定类型的实例序列化为二进制数据
        /// </summary>
        /// <param name="instance">实例对象</param>
        /// <returns>返回序列化后的二进制数据, 如果instance为null, 则返回null</returns>
        public static byte[] ToBytes<T>(object instance)
        {
            return ToBytes(typeof (T), instance);
        }
        
        /// <summary>
        ///     将指定类型的实例序列化为二进制数据
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="instance">实例对象</param>
        /// <returns>返回序列化后的二进制数据, 如果instance为null, 则返回null</returns>
        /// <exception cref="ArgumentNullException">type 参数不能为空</exception>
        /// <exception cref="NotSupportedException">不被支持的类型</exception>
        public static byte[] ToBytes(Type type, object instance)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (instance == null) return null;
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = null;
            try
            {
                proxy = MemorySegmentProxyFactory.Create();
                ToBytes(type, instance, proxy);
                return proxy.GetBytes();
            }
            catch
            {
                if (proxy != null) proxy.Dispose();
                throw;
            }
        }

        /// <summary>
        ///     将指定类型的实例序列化为二进制数据
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="instance">实例对象</param>
        /// <param name="proxy">内存段代理器</param>
        /// <returns>返回序列化后的二进制数据, 如果instance为null, 则返回null</returns>
        /// <exception cref="NotSupportedException">不被支持的类型</exception>
        private static void ToBytes(Type type, object instance, IMemorySegmentProxy proxy)
        {
            Type innerType;
            Action<IMemorySegmentProxy, object> cache;
            if (_serializers.TryGetValue(type.FullName, out cache))
            {
                cache(proxy, instance);
                return;
            }
            IIntellectTypeProcessor intellectTypeProcessor;

            #region 普通类型判断

            intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(type);
            if (intellectTypeProcessor != null)
            {
                //添加热缓存
                IIntellectTypeProcessor processor = intellectTypeProcessor;
                _serializers.TryAdd(type.FullName, (cache = (innerProxy, innerObj) => processor.Process(innerProxy, innerObj)));
            }

            #endregion

            #region 枚举类型判断

            //枚举类型
            else if (type.IsEnum)
            {
                //获取枚举类型
                Type enumType = Enum.GetUnderlyingType(type);
                intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(enumType);
                if (intellectTypeProcessor == null) throw new NotSupportedException(string.Format(ExceptionMessage.EX_NOT_SUPPORTED_VALUE_TEMPORARY, type));
                IIntellectTypeProcessor processor = intellectTypeProcessor;
                _serializers.TryAdd(type.FullName, (cache = (innerProxy, innerObj) => processor.Process(innerProxy, innerObj)));
            }

            #endregion

            #region 可空类型判断

            else if ((innerType = Nullable.GetUnderlyingType(type)) != null)
            {
                intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(innerType);
                if (intellectTypeProcessor == null) throw new NotSupportedException(string.Format(ExceptionMessage.EX_NOT_SUPPORTED_VALUE_TEMPORARY, innerType));
                IIntellectTypeProcessor processor = intellectTypeProcessor;
                _serializers.TryAdd(type.FullName, (cache = (innerProxy, innerObj) => processor.Process(innerProxy, innerObj)));
            }

            #endregion

            #region 智能对象类型判断

            else if (type.IsSubclassOf(typeof(IntellectObject)))
                _serializers.TryAdd(type.FullName, (cache = (innerProxy, innerObj) => { IntellectObjectEngine.ToBytes((IIntellectObject)innerObj, innerProxy); }));

            #endregion

            #region 数组的判断

            else if (type.IsArray)
            {
                if (!type.HasElementType)
                    throw new NotSupportedException(string.Format(ExceptionMessage.EX_NOT_SUPPORTED_VALUE_TEMPORARY, type));
                Type elementType = type.GetElementType();
                VT vt = FixedTypeManager.IsVT(elementType);
                IIntellectTypeProcessor arrayProcessor = ArrayTypeProcessorMapping.Instance.GetProcessor(type);
                if (arrayProcessor != null)
                    _serializers.TryAdd(type.FullName, (cache = (innerProxy, innerObj) => arrayProcessor.Process(innerProxy, innerObj)));
                else if (vt != null)
                    throw new NotSupportedException(string.Format(ExceptionMessage.EX_NOT_SUPPORTED_VALUE_TEMPORARY, type));
                else if (elementType.IsSubclassOf(typeof(IntellectObject)))
                {
                    //Add hot cache.
                    _serializers.TryAdd(type.FullName, (cache = (innerProxy, innerObj) =>
                    {
                        IIntellectObject[] array = (IIntellectObject[]) innerObj;
                        if (array == null || array.Length == 0) return;
                        //write array length.
                        innerProxy.WriteUInt32((uint)array.Length);
                        for (int i = 0; i < array.Length; i++)
                        {
                            if (array[i] == null) innerProxy.WriteUInt16(0);
                            else
                            {
                                MemoryPosition innerStartObjPosition = innerProxy.GetPosition();
                                innerProxy.Skip(Size.UInt16);
                                IntellectObjectEngine.ToBytes(array[i], innerProxy);
                                MemoryPosition innerEndObjPosition = innerProxy.GetPosition();
                                innerProxy.WriteBackUInt16(innerStartObjPosition, (ushort)(MemoryPosition.CalcLength(innerProxy.SegmentCount, innerStartObjPosition, innerEndObjPosition) - 2));
                            }
                        }
                    }));
                }
                else throw new NotSupportedException(string.Format(ExceptionMessage.EX_NOT_SUPPORTED_VALUE_TEMPORARY, type));
            }

            #endregion

            cache(proxy, instance);
        }

        /// <summary>
        ///     将二进制数据反序列化成指定类型对象
        /// </summary>
        /// <param name="data">二进制数据</param>
        /// <returns>返回反序列化后的对象, 如果data为null, 则返回null</returns>
        public static T GetObject<T>(byte[] data)
        {
            return (T)GetObject(typeof (T), data);
        }

        /// <summary>
        ///     将二进制数据反序列化成指定类型对象
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="data">二进制数据</param>
        /// <returns>返回反序列化后的对象, 如果data为null, 则返回null</returns>
        /// <exception cref="ArgumentNullException">type 参数不能为空</exception>
        public static object GetObject(Type type, byte[] data)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (data == null) return null;
            Type innerType;
            Func<byte[], object> cache;
            if (_deserializers.TryGetValue(type.FullName, out cache)) return cache(data);
            IIntellectTypeProcessor intellectTypeProcessor;

            #region 普通类型判断

            intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(type);
            if (intellectTypeProcessor != null)
            {
                //添加热缓存
                IIntellectTypeProcessor processor = intellectTypeProcessor;
                _deserializers.TryAdd(type.FullName, cache = parameter => { return processor.Process(IntellectTypeProcessorMapping.DefaultAttribute, parameter); });
            }

            #endregion

            #region 枚举类型判断

            //枚举类型
            else if (type.IsEnum)
            {
                Type enumType = Enum.GetUnderlyingType(type);
                intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(enumType);
                if (intellectTypeProcessor == null) throw new NotSupportedException(string.Format(ExceptionMessage.EX_NOT_SUPPORTED_VALUE_TEMPORARY, type));
                IIntellectTypeProcessor processor = intellectTypeProcessor;
                _deserializers.TryAdd(type.FullName, cache = parameter => { return processor.Process(IntellectTypeProcessorMapping.DefaultAttribute, parameter); });
            }

            #endregion

            #region 可空类型判断

            else if ((innerType = Nullable.GetUnderlyingType(type)) != null)
            {
                intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(innerType);
                if (intellectTypeProcessor == null) throw new NotSupportedException(string.Format(ExceptionMessage.EX_NOT_SUPPORTED_VALUE_TEMPORARY, type));
                IIntellectTypeProcessor processor = intellectTypeProcessor;
                _deserializers.TryAdd(type.FullName, cache = parameter => { return processor.Process(IntellectTypeProcessorMapping.DefaultAttribute, parameter); });
            }

            #endregion

            #region 智能类型的判断

            //智能对象的判断
            else if (type.IsSubclassOf(typeof(IntellectObject)))
                _deserializers.TryAdd(type.FullName, cache = parameter => { return IntellectObjectEngine.GetObject<object>(type, parameter); });

            #endregion

            #region 数组的判断

            else if (type.IsArray)
            {
                Type elementType = type.GetElementType();
                IIntellectTypeProcessor arrayProcessor = ArrayTypeProcessorMapping.Instance.GetProcessor(type);
                //VT type.
                if (arrayProcessor != null)
                    _deserializers.TryAdd(type.FullName, cache = parameter => { return arrayProcessor.Process(IntellectTypeProcessorMapping.DefaultAttribute, parameter); });
                else if (elementType.IsSubclassOf(typeof(IntellectObject)))
                {
                    #region IntellectObject type array processor.

                    _deserializers.TryAdd(type.FullName, cache = parameter =>
                    {
                        object[] array = (object[])Activator.CreateInstance(type, BitConverter.ToInt32(parameter, 0));
                        int innerOffset = 4;
                        ushort size;
                        for (int i = 0; i < array.Length; i++)
                        {
                            size = BitConverter.ToUInt16(parameter, innerOffset);
                            innerOffset += 2;
                            if (size == 0) array[i] = null;
                            else array[i] = IntellectObjectEngine.GetObject<IntellectObject>(elementType, parameter, innerOffset, size);
                            innerOffset += size;
                        }
                        return array;
                    });

                    #endregion
                }
                else throw new NotSupportedException(string.Format(ExceptionMessage.EX_NOT_SUPPORTED_VALUE_TEMPORARY, type));
            }

            #endregion
            
            return cache(data);
        }

        #endregion
    }
}