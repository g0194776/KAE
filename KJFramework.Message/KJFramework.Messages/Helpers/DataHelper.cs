using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Engine;
using KJFramework.Messages.Objects;
using KJFramework.Messages.TypeProcessors;
using KJFramework.Messages.TypeProcessors.Maps;

namespace KJFramework.Messages.Helpers
{
    /// <summary>
    ///     ���ݰ��������ṩ����صĻ���������
    /// </summary>
    public static class DataHelper
    {
        #region Members

        private static readonly ConcurrentDictionary<string, Func<object, byte[]>> _serializers = new ConcurrentDictionary<string, Func<object, byte[]>>();
        private static readonly ConcurrentDictionary<string, Func<byte[], object>> _deserializers = new ConcurrentDictionary<string, Func<byte[], object>>(); 

        #endregion

        #region Methods

        /// <summary>
        ///     ��ָ�����͵�ʵ�����л�Ϊ����������
        /// </summary>
        /// <param name="type">����</param>
        /// <param name="instance">ʵ������</param>
        /// <returns>�������л���Ķ���������</returns>
        /// <exception cref="ArgumentNullException">type ��������Ϊ��</exception>
        public static byte[] ToBytes(Type type, object instance)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (instance == null) return null;
            Type innerType;
            Func<object, byte[]> cache;
            if (_serializers.TryGetValue(type.FullName, out cache)) return cache(instance);
            IIntellectTypeProcessor intellectTypeProcessor;

            #region ��ͨ�����ж�

            intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(type);
            if (intellectTypeProcessor != null)
            {
                //����Ȼ���
                IIntellectTypeProcessor processor = intellectTypeProcessor;
                _serializers.TryAdd(type.FullName, (cache = parameter => { return processor.Process(IntellectTypeProcessorMapping.DefaultAttribute, parameter); }));
            }

            #endregion

            #region ö�������ж�

            //ö������
            else if (type.IsEnum)
            {
                //��ȡö������
                Type enumType = Enum.GetUnderlyingType(type);
                intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(enumType);
                if (intellectTypeProcessor == null) throw new System.Exception("Cannot support this enum type! #type: " + type);
                IIntellectTypeProcessor processor = intellectTypeProcessor;
                _serializers.TryAdd(type.FullName, (cache = parameter => { return processor.Process(IntellectTypeProcessorMapping.DefaultAttribute, parameter); }));
            }

            #endregion

            #region �ɿ������ж�

            else if ((innerType = Nullable.GetUnderlyingType(type)) != null)
            {
                intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(innerType);
                if (intellectTypeProcessor == null) throw new System.Exception("Cannot find compatible processor, #type: " + type);
                IIntellectTypeProcessor processor = intellectTypeProcessor;
                _serializers.TryAdd(type.FullName, (cache = parameter => { return processor.Process(IntellectTypeProcessorMapping.DefaultAttribute, parameter); }));
            }

            #endregion

            #region ���ܶ��������ж�

            else if (type.IsSubclassOf(typeof(IntellectObject)))
            {
                _serializers.TryAdd(type.FullName, (cache = parameter => { return IntellectObjectEngine.ToBytes((IIntellectObject)parameter); }));
            }

            #endregion

            #region ������ж�

            else if (type.IsArray)
            {
                if (!type.HasElementType)
                    throw new System.Exception("Cannot support this array type! #type: " + type);
                Type elementType = type.GetElementType();
                VT vt = FixedTypeManager.IsVT(elementType);
                if (vt != null)
                {
                    #region Deal VT type array specal.

                    intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(elementType);
                    if (intellectTypeProcessor == null) throw new System.Exception("Cannot support this array element type! #type: " + elementType);
                    //Add hot cache.
                    IIntellectTypeProcessor processor = intellectTypeProcessor;
                    _serializers.TryAdd(type.FullName, cache = parameter =>
                    {
                        Array array = (Array)parameter;
                        //id(1) + total length(4) + rank(4)
                        byte[] memory = new byte[9 + array.Length * vt.Size];
                        //be carefully, flag 0(array length) cannot equals really array length(Equal array.length - current property id - 4 bytes).
                        BitConvertHelper.GetBytes(memory.Length - 5, memory, 1);
                        BitConvertHelper.GetBytes(array.Length, memory, 5);
                        int innerVtOffset = 9;
                        for (int i = 0; i < array.Length; i++)
                        {
                            object element = array.GetValue(i);
                            processor.Process(memory, innerVtOffset, IntellectTypeProcessorMapping.DefaultAttribute, element);
                            innerVtOffset += vt.Size;
                        }
                        return memory;
                    });

                    #endregion
                }
                else if (elementType.IsSubclassOf(typeof(IntellectObject)))
                {
                    _serializers.TryAdd(type.FullName, cache = parameter =>
                    {
                        Array array = (Array)parameter;
                        IList<byte[]> elementDatas = new List<byte[]>(array.Length);
                        int getLength = 0;
                        for (int i = 0; i < array.Length; i++)
                        {
                            IntellectObject element = (IntellectObject)array.GetValue(i);
                            byte[] elementData = IntellectObjectHelper.SetLength(IntellectObjectEngine.ToBytes(element));
                            elementDatas.Add(elementData);
                            getLength += elementData.Length;
                        }
                        #region Concat data.

                        byte[] memory = new byte[getLength + 9];
                        BitConvertHelper.GetBytes(getLength + 4, memory, 1);
                        BitConvertHelper.GetBytes(array.Length, memory, 5);
                        int innerOffset = 9;
                        foreach (byte[] bytes in elementDatas)
                        {
                            Buffer.BlockCopy(bytes, 0, memory, innerOffset, bytes.Length);
                            innerOffset += bytes.Length;
                        }
                        return memory;

                        #endregion
                    });
                }
                else
                {
                    intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(elementType);
                    if (intellectTypeProcessor == null) throw new System.Exception("Cannot support this array element type processor! #type: " + elementType);
                    //Add hot cache.
                    IIntellectTypeProcessor processor = intellectTypeProcessor;
                    _serializers.TryAdd(type.FullName, cache = parameter =>
                    {
                        Array array = (Array)parameter;
                        IList<byte[]> elementDatas = new List<byte[]>(array.Length);
                        int getLength = 0;
                        for (int i = 0; i < array.Length; i++)
                        {
                            object element = array.GetValue(i);
                            byte[] elementData = IntellectObjectHelper.SetLength(processor.Process(IntellectTypeProcessorMapping.DefaultAttribute, element));
                            elementDatas.Add(elementData);
                            getLength += elementData.Length;
                        }
                        #region Concat data.

                        byte[] memory = new byte[getLength + 9];
                        BitConvertHelper.GetBytes(getLength + 4, memory, 1);
                        BitConvertHelper.GetBytes(array.Length, memory, 5);
                        int innerOffset = 9;
                        foreach (byte[] bytes in elementDatas)
                        {
                            Buffer.BlockCopy(bytes, 0, memory, innerOffset, bytes.Length);
                            innerOffset += bytes.Length;
                        }
                        return memory;

                        #endregion
                    });
                }
            }

            #endregion

            #region Error

            else throw new System.Exception("Cannot process this data type! #type: " + type);

            #endregion

            return cache(instance);
        }

        /// <summary>
        ///     �����������ݷ����л���ָ�����Ͷ���
        /// </summary>
        /// <param name="type">����</param>
        /// <param name="data">����������</param>
        /// <returns>���ط����л���Ķ���</returns>
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
                if (intellectTypeProcessor == null) throw new System.Exception("Cannot support this enum type! #type: " + type);
                IIntellectTypeProcessor processor = intellectTypeProcessor;
                _deserializers.TryAdd(type.FullName, cache = parameter => { return processor.Process(IntellectTypeProcessorMapping.DefaultAttribute, parameter); });
            }

            #endregion

            #region �ɿ������ж�

            else if ((innerType = Nullable.GetUnderlyingType(type)) != null)
            {
                intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(innerType);
                if (intellectTypeProcessor == null) throw new System.Exception("Cannot find compatible processor, #type: " + type);
                IIntellectTypeProcessor processor = intellectTypeProcessor;
                _deserializers.TryAdd(type.FullName, cache = parameter => { return processor.Process(IntellectTypeProcessorMapping.DefaultAttribute, parameter); });
            }

            #endregion

            #region �������͵��ж�

            //���ܶ�����ж�
            else if (type.IsSubclassOf(typeof(IntellectObject)))
            {
                _deserializers.TryAdd(type.FullName, cache = parameter => { return IntellectObjectEngine.GetObject<Object>(type, parameter); });
            }

            #endregion

            #region ������ж�

            else if (type.IsArray)
            {
                Type elementType = type.GetElementType();
                VT vt = FixedTypeManager.IsVT(elementType);
                intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(elementType);
                //VT type.
                if (vt != null)
                {
                    #region VT type array processor.

                    if (intellectTypeProcessor == null) throw new System.Exception("Cannot found any processor to process current VT type. #type: " + elementType);
                    IIntellectTypeProcessor safeProcessor = intellectTypeProcessor;
                    _deserializers.TryAdd(type.FullName, cache = parameter =>
                    {
                        Array array;
                        int innerOffset = 5;
                        int chunkSize = parameter.Length;
                        int arrLen = BitConverter.ToInt32(parameter, innerOffset);
                        if (arrLen == 0) return Array.CreateInstance(elementType, 0);
                        innerOffset += 4;
                        array = Array.CreateInstance(elementType, arrLen);
                        int arrIndex = 0;
                        do
                        {
                            if ((parameter.Length - innerOffset) < vt.Size)
                                throw new System.Exception("Illegal remaining binary data length!");
                            //use unmanagement method by default.
                            array.SetValue(safeProcessor.Process(IntellectTypeProcessorMapping.DefaultAttribute, parameter, innerOffset, vt.Size), arrIndex);
                            innerOffset += vt.Size;
                            arrIndex++;
                        } while (innerOffset < parameter.Length && innerOffset < chunkSize);
                        return array;
                    });

                    #endregion
                }
                else if (elementType.IsSubclassOf(typeof(IntellectObject)))
                {
                    #region IntellectObject type array processor.

                    _deserializers.TryAdd(type.FullName, cache = parameter =>
                    {
                        Array array;
                        int innerOffset = 5;
                        int chunkSize = parameter.Length;
                        int arrLen = BitConverter.ToInt32(parameter, innerOffset);
                        if (arrLen == 0) return Array.CreateInstance(elementType, 0);
                        innerOffset += 4;
                        array = Array.CreateInstance(elementType, arrLen);
                        int arrIndex = 0;
                        short size;
                        do
                        {
                            size = BitConverter.ToInt16(parameter, innerOffset);
                            innerOffset += 2;
                            if ((parameter.Length - innerOffset) < size)
                                throw new System.Exception("Illegal remaining binary data length!");
                            //use unmanagement method by default.
                            if (size == 0) array.SetValue(null, arrIndex);
                            else array.SetValue(IntellectObjectEngine.GetObject<IntellectObject>(elementType, parameter, innerOffset, size), arrIndex);
                            innerOffset += size;
                            arrIndex++;
                        } while (innerOffset < parameter.Length && innerOffset < chunkSize);
                        return array;
                    });

                    #endregion
                }
                else
                {
                    #region Any types if it can get the processor.

                    intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(elementType);
                    if (intellectTypeProcessor == null) throw new System.Exception("Cannot support this array element type processor! #type: " + elementType);
                    //Add hot cache.
                    IIntellectTypeProcessor safeProcessor = intellectTypeProcessor;

                    _deserializers.TryAdd(type.FullName, cache = parameter =>
                    {
                        Array array;
                        int innerOffset = 5;
                        int chunkSize = parameter.Length;
                        int arrLen = BitConverter.ToInt32(parameter, innerOffset);
                        if (arrLen == 0) return Array.CreateInstance(elementType, 0);
                        innerOffset += 4;
                        array = Array.CreateInstance(elementType, arrLen);
                        int arrIndex = 0;
                        short size;
                        do
                        {
                            size = BitConverter.ToInt16(parameter, innerOffset);
                            innerOffset += 2;
                            if ((parameter.Length - innerOffset) < size)
                                throw new System.Exception("Illegal remaining binary data length!");
                            //use unmanagement method by default.
                            if (size == 0) array.SetValue(null, arrIndex);
                            else array.SetValue(safeProcessor.Process(IntellectTypeProcessorMapping.DefaultAttribute, parameter, innerOffset, size), arrIndex);
                            innerOffset += size;
                            arrIndex++;
                        } while (innerOffset < parameter.Length && innerOffset < chunkSize);
                        return array;
                    });

                    #endregion
                }
            }

            #endregion

            #region Error

            else throw new System.Exception("Cannot support this data type: " + type);

            #endregion

            return cache(data);
        }

        #endregion
    }
}