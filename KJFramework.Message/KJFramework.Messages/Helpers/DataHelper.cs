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
    ///     ���ݰ��������ṩ����صĻ���������
    /// </summary>
    public static class DataHelper
    {
        #region Members

        private static readonly ConcurrentDictionary<string, Action<IMemorySegmentProxy, object>> _serializers = new ConcurrentDictionary<string, Action<IMemorySegmentProxy, object>>();
        private static readonly ConcurrentDictionary<string, Func<byte[], object>> _deserializers = new ConcurrentDictionary<string, Func<byte[], object>>(); 

        #endregion

        #region Methods

        /// <summary>
        ///     ��ָ�����͵�ʵ�����л�Ϊ����������
        /// </summary>
        /// <param name="instance">ʵ������</param>
        /// <returns>�������л���Ķ���������, ���instanceΪnull, �򷵻�null</returns>
        public static byte[] ToBytes(object instance)
        {
            return ToBytes(instance.GetType(), instance);
        }
        
        /// <summary>
        ///     ��ָ�����͵�ʵ�����л�Ϊ����������
        /// </summary>
        /// <param name="instance">ʵ������</param>
        /// <returns>�������л���Ķ���������, ���instanceΪnull, �򷵻�null</returns>
        public static byte[] ToBytes<T>(object instance)
        {
            return ToBytes(typeof (T), instance);
        }
        
        /// <summary>
        ///     ��ָ�����͵�ʵ�����л�Ϊ����������
        /// </summary>
        /// <param name="type">����</param>
        /// <param name="instance">ʵ������</param>
        /// <returns>�������л���Ķ���������, ���instanceΪnull, �򷵻�null</returns>
        /// <exception cref="ArgumentNullException">type ��������Ϊ��</exception>
        /// <exception cref="NotSupportedException">����֧�ֵ�����</exception>
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
        ///     ��ָ�����͵�ʵ�����л�Ϊ����������
        /// </summary>
        /// <param name="type">����</param>
        /// <param name="instance">ʵ������</param>
        /// <param name="proxy">�ڴ�δ�����</param>
        /// <returns>�������л���Ķ���������, ���instanceΪnull, �򷵻�null</returns>
        /// <exception cref="NotSupportedException">����֧�ֵ�����</exception>
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

            #region ��ͨ�����ж�

            intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(type);
            if (intellectTypeProcessor != null)
            {
                //����Ȼ���
                IIntellectTypeProcessor processor = intellectTypeProcessor;
                _serializers.TryAdd(type.FullName, (cache = (innerProxy, innerObj) => processor.Process(innerProxy, innerObj)));
            }

            #endregion

            #region ö�������ж�

            //ö������
            else if (type.IsEnum)
            {
                //��ȡö������
                Type enumType = Enum.GetUnderlyingType(type);
                intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(enumType);
                if (intellectTypeProcessor == null) throw new NotSupportedException(string.Format(ExceptionMessage.EX_NOT_SUPPORTED_VALUE_TEMPORARY, type));
                IIntellectTypeProcessor processor = intellectTypeProcessor;
                _serializers.TryAdd(type.FullName, (cache = (innerProxy, innerObj) => processor.Process(innerProxy, innerObj)));
            }

            #endregion

            #region �ɿ������ж�

            else if ((innerType = Nullable.GetUnderlyingType(type)) != null)
            {
                intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(innerType);
                if (intellectTypeProcessor == null) throw new NotSupportedException(string.Format(ExceptionMessage.EX_NOT_SUPPORTED_VALUE_TEMPORARY, innerType));
                IIntellectTypeProcessor processor = intellectTypeProcessor;
                _serializers.TryAdd(type.FullName, (cache = (innerProxy, innerObj) => processor.Process(innerProxy, innerObj)));
            }

            #endregion

            #region ���ܶ��������ж�

            else if (type.IsSubclassOf(typeof(IntellectObject)))
                _serializers.TryAdd(type.FullName, (cache = (innerProxy, innerObj) => { IntellectObjectEngine.ToBytes((IIntellectObject)innerObj, innerProxy); }));

            #endregion

            #region ������ж�

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
        ///     �����������ݷ����л���ָ�����Ͷ���
        /// </summary>
        /// <param name="data">����������</param>
        /// <returns>���ط����л���Ķ���, ���dataΪnull, �򷵻�null</returns>
        public static T GetObject<T>(byte[] data)
        {
            return (T)GetObject(typeof (T), data);
        }

        /// <summary>
        ///     �����������ݷ����л���ָ�����Ͷ���
        /// </summary>
        /// <param name="type">����</param>
        /// <param name="data">����������</param>
        /// <returns>���ط����л���Ķ���, ���dataΪnull, �򷵻�null</returns>
        /// <exception cref="ArgumentNullException">type ��������Ϊ��</exception>
        public static object GetObject(Type type, byte[] data)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (data == null) return null;
            Type innerType;
            Func<byte[], object> cache;
            if (_deserializers.TryGetValue(type.FullName, out cache)) return cache(data);
            IIntellectTypeProcessor intellectTypeProcessor;

            #region ��ͨ�����ж�

            intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(type);
            if (intellectTypeProcessor != null)
            {
                //����Ȼ���
                IIntellectTypeProcessor processor = intellectTypeProcessor;
                _deserializers.TryAdd(type.FullName, cache = parameter => { return processor.Process(IntellectTypeProcessorMapping.DefaultAttribute, parameter); });
            }

            #endregion

            #region ö�������ж�

            //ö������
            else if (type.IsEnum)
            {
                Type enumType = Enum.GetUnderlyingType(type);
                intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(enumType);
                if (intellectTypeProcessor == null) throw new NotSupportedException(string.Format(ExceptionMessage.EX_NOT_SUPPORTED_VALUE_TEMPORARY, type));
                IIntellectTypeProcessor processor = intellectTypeProcessor;
                _deserializers.TryAdd(type.FullName, cache = parameter => { return processor.Process(IntellectTypeProcessorMapping.DefaultAttribute, parameter); });
            }

            #endregion

            #region �ɿ������ж�

            else if ((innerType = Nullable.GetUnderlyingType(type)) != null)
            {
                intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(innerType);
                if (intellectTypeProcessor == null) throw new NotSupportedException(string.Format(ExceptionMessage.EX_NOT_SUPPORTED_VALUE_TEMPORARY, type));
                IIntellectTypeProcessor processor = intellectTypeProcessor;
                _deserializers.TryAdd(type.FullName, cache = parameter => { return processor.Process(IntellectTypeProcessorMapping.DefaultAttribute, parameter); });
            }

            #endregion

            #region �������͵��ж�

            //���ܶ�����ж�
            else if (type.IsSubclassOf(typeof(IntellectObject)))
                _deserializers.TryAdd(type.FullName, cache = parameter => { return IntellectObjectEngine.GetObject<object>(type, parameter); });

            #endregion

            #region ������ж�

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