using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using KJFramework.Messages.Enums;
using KJFramework.Messages.Exceptions;
using KJFramework.Messages.ValueStored;

namespace KJFramework.Messages.Types
{
    /// <summary>
    ///    资源块对象
    /// </summary>
    public class ResourceBlock
    {
        #region Constructors.

        /// <summary>
        ///    资源块对象
        /// </summary>
        public ResourceBlock()
        {
            
        }

        /// <summary>
        ///     内部构造函数，用于初始化一个拥有了内部数据的包装对象
        /// </summary>
        /// <param name="dic">内部结构数据</param>
        internal ResourceBlock(Dictionary<byte, BaseValueStored> dic)
        {
            if (dic == null) throw new ArgumentNullException("dic");
            _valueStoreds = dic;
        }

        #endregion

        #region Members

        protected Dictionary<byte, BaseValueStored> _valueStoreds = new Dictionary<byte, BaseValueStored>();

        #endregion

        #region Methods.

        /// <summary>
        ///     获取一个指定key的属性值
        /// </summary>
        /// <param name="id">属性对应的Id值</param>
        /// <returns>返回一个属性value</returns>
        public virtual BaseValueStored GetAttribute(byte id)
        {
            BaseValueStored value;
            if (!_valueStoreds.TryGetValue(id, out value)) return null;
            return value;
        }

        /// <summary>
        ///     获取一个指定key的属性值
        ///     <para>* 此方法不支持: T = 智能对象类型</para>
        /// </summary>
        /// <param name="id">属性对应的Id值</param>
        /// <returns>返回一个属性value</returns>
        /// <remarks>
        ///     * 强烈建议使用此方法之前，先使用IsAttibuteExsits()方法进行判断。
        /// </remarks>
        /// <exception cref="SpecificKeyNotExistsException">指定的key不存在</exception>
        public virtual T GetAttributeAsType<T>(byte id)
        {
            BaseValueStored value;
            if (!_valueStoreds.TryGetValue(id, out value)) throw new SpecificKeyNotExistsException(string.Format("#Sadly, current attribute id: {0} didn't existed.", id));
            return value.GetValue<T>();
        }

        /// <summary>
        ///     尝试获取一个指定key的属性值
        ///     <para>* 此方法不支持: T = 智能对象类型</para>
        /// </summary>
        /// <param name="id">属性对应的Id值</param>
        /// <param name="value">输出的参数值</param>
        /// <returns>如果当前对象内部不存在指定的key, 则返回false, 否则返回true.</returns>
        public virtual bool TryGetAttributeAsType<T>(byte id, out T value)
        {
            BaseValueStored valueStored;
            if (!_valueStoreds.TryGetValue(id, out valueStored))
            {
                value = default(T);
                return false;
            }
            value = valueStored.GetValue<T>();
            return true;
        }

        /// <summary>
        ///     设置一个指定key的属性值
        /// </summary>
        /// <param name="id">设置属性对应的id</param>
        /// <param name="baseValueStored">设置属性的value</param>
        /// <exception cref="ArgumentNullException">baseValueStored不能为空</exception>
        public virtual ResourceBlock SetAttribute(byte id, BaseValueStored baseValueStored)
        {
            if (baseValueStored == null) throw new ArgumentNullException("baseValueStored");
            _valueStoreds[id] = baseValueStored;
            return this;
        }

        internal virtual Dictionary<byte, BaseValueStored> GetMetaDataDictionary()
        {
            return _valueStoreds;
        }

        /// <summary>
        ///     是否存在指定key的属性
        /// </summary>
        /// <param name="id">指定key</param>
        /// <returns>是否移除</returns>
        public virtual bool IsAttibuteExsits(byte id)
        {
            return _valueStoreds.ContainsKey(id);
        }

        /// <summary>
        ///     移除指定key的属性
        /// </summary>
        /// <param name="id">指定key</param>
        public virtual void RemoveAttribute(byte id)
        {
            _valueStoreds.Remove(id);
        }

        /// <summary>
        ///    为当前资源块提供可阅读的输出信息
        /// </summary>
        /// <returns>返回内部所有字段可阅读状态的字符串信息</returns>
        public override string ToString()
        {
            return ToString(string.Empty, false, _valueStoreds);
        }

        /// <summary>
        ///     内部方法，用于将一个对象转换为字符串的形式表现出来
        /// </summary>
        /// <param name="space">缩进空间</param>
        /// <param name="isArrayLoop">是否陷入数组循环的标示</param>
        /// <param name="valueStoreds">集合信息</param>
        /// <returns>返回标示当前对象的字符串</returns>
        internal string ToString(string space, bool isArrayLoop, Dictionary<byte, BaseValueStored> valueStoreds)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (KeyValuePair<byte, BaseValueStored> pair in valueStoreds)
            {
                if (pair.Value.IsExtension)
                {
                    stringBuilder.AppendLine(string.Format("{0}{1}: {2}", space, pair.Key.ToString("X2"), pair.Value));
                }
                else
                {
                  switch (pair.Value.TypeId)
                  {
                    case (byte)PropertyTypes.UInt32:
                        stringBuilder.AppendLine(string.Format("{0}{1}: {2}", space, pair.Key.ToString("X2"), pair.Value.GetValue<uint>()));
                        break;
                    case (byte)PropertyTypes.String:
                        stringBuilder.AppendLine(string.Format("{0}{1}: {2}", space, pair.Key.ToString("X2"), pair.Value.GetValue<string>()));
                        break;
                    case (byte)PropertyTypes.Float:
                        stringBuilder.AppendLine(string.Format("{0}{1}: {2}", space, pair.Key.ToString("X2"), pair.Value.GetValue<float>()));
                        break;
                    case (byte)PropertyTypes.Int16:
                        stringBuilder.AppendLine(string.Format("{0}{1}: {2}", space, pair.Key.ToString("X2"), pair.Value.GetValue<short>()));
                        break;
                    case (byte)PropertyTypes.UInt16:
                        stringBuilder.AppendLine(string.Format("{0}{1}: {2}", space, pair.Key.ToString("X2"), pair.Value.GetValue<ushort>()));
                        break;
                    case (byte)PropertyTypes.Int32:
                        stringBuilder.AppendLine(string.Format("{0}{1}: {2}", space, pair.Key.ToString("X2"), pair.Value.GetValue<int>()));
                        break;
                    case (byte)PropertyTypes.UInt64:
                        stringBuilder.AppendLine(string.Format("{0}{1}: {2}", space, pair.Key.ToString("X2"), pair.Value.GetValue<ulong>()));
                        break;
                    case (byte)PropertyTypes.Int64:
                        stringBuilder.AppendLine(string.Format("{0}{1}: {2}", space, pair.Key.ToString("X2"), pair.Value.GetValue<long>()));
                        break;
                    case (byte)PropertyTypes.Double:
                        stringBuilder.AppendLine(string.Format("{0}{1}: {2}", space, pair.Key.ToString("X2"), pair.Value.GetValue<double>()));
                        break;
                    case (byte)PropertyTypes.Boolean:
                        stringBuilder.AppendLine(string.Format("{0}{1}: {2}", space, pair.Key.ToString("X2"), pair.Value.GetValue<bool>()));
                        break;
                    case (byte)PropertyTypes.Byte:
                        stringBuilder.AppendLine(string.Format("{0}{1}: {2}", space, pair.Key.ToString("X2"), pair.Value.GetValue<byte>()));
                        break;
                    case (byte)PropertyTypes.Char:
                        stringBuilder.AppendLine(string.Format("{0}{1}: {2}", space, pair.Key.ToString("X2"), pair.Value.GetValue<char>()));
                        break;
                    case (byte)PropertyTypes.Decimal:
                        stringBuilder.AppendLine(string.Format("{0}{1}: {2}", space, pair.Key.ToString("X2"), pair.Value.GetValue<decimal>()));
                        break;
                    case (byte)PropertyTypes.SByte:
                        stringBuilder.AppendLine(string.Format("{0}{1}: {2}", space, pair.Key.ToString("X2"), pair.Value.GetValue<sbyte>()));
                        break;
                    case (byte)PropertyTypes.DateTime:
                        stringBuilder.AppendLine(string.Format("{0}{1}: {2}", space, pair.Key.ToString("X2"), pair.Value.GetValue<DateTime>()));
                        break;
                    case (byte)PropertyTypes.Guid:
                        stringBuilder.AppendLine(string.Format("{0}{1}: {2}", space, pair.Key.ToString("X2"), pair.Value.GetValue<Guid>()));
                        break;
                    case (byte)PropertyTypes.IPEndPoint:
                        stringBuilder.AppendLine(string.Format("{0}{1}: {2}", space, pair.Key.ToString("X2"), pair.Value.GetValue<IPEndPoint>()));
                        break;
                    case (byte)PropertyTypes.IntPtr:
                        stringBuilder.AppendLine(string.Format("{0}{1}: {2}", space, pair.Key.ToString("X2"), pair.Value.GetValue<IntPtr>()));
                        break;
                    case (byte)PropertyTypes.TimeSpan:
                        stringBuilder.AppendLine(string.Format("{0}{1}: {2}", space, pair.Key.ToString("X2"), pair.Value.GetValue<TimeSpan>()));
                        break;
                    case (byte)PropertyTypes.BitFlag:
                        stringBuilder.AppendLine(string.Format("{0}{1}: {2}", space, pair.Key.ToString("X2"), pair.Value.GetValue<BitFlag>()));
                        break;
                    case (byte)PropertyTypes.Blob:
                        stringBuilder.AppendLine(string.Format("{0}{1}: {2}", space, pair.Key.ToString("X2"), pair.Value.GetValue<Blob>()));
                        break;
                    case (byte)PropertyTypes.Null:
                        break;
                    case (byte)PropertyTypes.ResourceBlock:
                        stringBuilder.AppendLine(string.Format("{0}{1}: ", space, pair.Key.ToString("X2"))).Append(space).AppendLine("{");
                        stringBuilder.Append(ToString(space + "  ", isArrayLoop, pair.Value.GetValue<ResourceBlock>().GetMetaDataDictionary()));
                        stringBuilder.Append(space).AppendLine("}");
                        break;
                    default:
                        #region Process array.

                        if (pair.Value.TypeId == (byte)PropertyTypes.GuidArray)
                        {
                            #region Set handler of Guid array

                            Guid[] array = pair.Value.GetValue<Guid[]>();
                            if (array == null)
                            {
                                stringBuilder.AppendLine(string.Format("{0}{1}: NULL", space, pair.Key.ToString("X2")));
                                continue;
                            }
                            string nxtSpace = space + "  ";
                            stringBuilder.AppendLine(string.Format("{0}{1}: ", space, pair.Key.ToString("X2"))).Append(space).AppendLine("{");
                            StringBuilder innerBuilder = new StringBuilder();
                            innerBuilder.Append("[Management Guid] ");
                            for (int j = 0; j < array.Length; j++)
                            {
                                Guid guid = (Guid)array.GetValue(j);
                                foreach (byte element in guid.ToByteArray())
                                    innerBuilder.Append(element.ToString("x02")).Append(", ");
                                stringBuilder.AppendLine(string.Format("{0}{1}", nxtSpace, innerBuilder));
                            }
                            stringBuilder.Append(space).AppendLine("}");

                            #endregion
                        }
                        else if (pair.Value.TypeId == (byte)PropertyTypes.ByteArray || pair.Value.TypeId == (byte)PropertyTypes.IntellectObjectArray || pair.Value.TypeId == (byte)PropertyTypes.IntellectObject)
                        {
                            #region Set handler of Byte array

                            byte[] array = pair.Value.GetValue<byte[]>();
                            if (array == null)
                            {
                                stringBuilder.AppendLine(string.Format("{0}{1}: NULL", space, pair.Key.ToString("X2")));
                                continue;
                            }
                            string nextSpace = space + "  ";
                            stringBuilder.AppendLine(string.Format("{0}{1}: ", space, pair.Key.ToString("X2"))).Append(space).AppendLine("{");
                            int round = array.Length / 8 + (array.Length % 8 > 0 ? 1 : 0);
                            int currentOffset, remainningLen;
                            for (int j = 0; j < round; j++)
                            {
                                currentOffset = j * 8;
                                remainningLen = ((array.Length - currentOffset) >= 8 ? 8 : (array.Length - currentOffset));
                                StringBuilder rawByteBuilder = new StringBuilder();
                                for (int k = 0; k < remainningLen; k++)
                                {
                                    rawByteBuilder.AppendFormat("0x{0}", array[currentOffset + k].ToString("X2"));
                                    if (k != remainningLen - 1) rawByteBuilder.Append(", ");
                                }
                                rawByteBuilder.Append(new string(' ', (remainningLen == 8 ? 5 : (8 - remainningLen) * 4 + (((8 - remainningLen) - 1) * 2) + 7)));
                                for (int k = 0; k < remainningLen; k++)
                                {
                                    if ((char)array[currentOffset + k] > 126 || (char)array[currentOffset + k] < 32) rawByteBuilder.Append('.');
                                    else rawByteBuilder.Append((char)array[currentOffset + k]);
                                }
                                stringBuilder.AppendLine(string.Format("{0}{1}", nextSpace, rawByteBuilder));
                            }
                            stringBuilder.Append(space).AppendLine("}");

                            #endregion
                        }
                        else if (pair.Value.TypeId == (byte)PropertyTypes.IntPtrArray)
                        {
                            #region Set handler of IntPtr array
                            
                            IntPtr[] array = pair.Value.GetValue<IntPtr[]>();
                            if (array == null)
                            {
                                stringBuilder.AppendLine(string.Format("{0}{1}: NULL", space, pair.Key.ToString("X2")));
                                continue;
                            }
                            string nxtSpace = space + "  ";
                            stringBuilder.AppendLine(string.Format("{0}{1}: ", space, pair.Key.ToString("X2"))).Append(space).AppendLine("{");
                            for (int j = 0; j < array.Length; j++)
                            {
                                IntPtr intPtr = array[j];
                                stringBuilder.AppendLine(string.Format("{0}[Management IntPtr] {1}", nxtSpace, intPtr.ToInt32()));
                            }
                            stringBuilder.Append(space).AppendLine("}");

                            #endregion
                        }
                        else if (pair.Value.TypeId == (byte)PropertyTypes.ResourceBlockArray)
                        {
                            #region Set handler of ResourceBlock array

                            ResourceBlock[] array = pair.Value.GetValue<ResourceBlock[]>();
                            if (array == null)
                            {
                                stringBuilder.AppendLine(string.Format("{0}{1}: NULL", space, pair.Key.ToString("X2")));
                                continue;
                            }
                            string nxtSpace = space + "  ";
                            stringBuilder.AppendLine(string.Format("{0}{1}: ", space, pair.Key.ToString("X2"))).Append(space).AppendLine("{");
                            for (int j = 0; j < array.Length; j++)
                            {
                                stringBuilder.AppendLine(string.Format("{0}#---------RESOURCE BLOCK ELEMENT: {1}---------#", nxtSpace, j));
                                ResourceBlock obj = array[j];
                                if (obj == null) stringBuilder.AppendLine(string.Format("{0}NULL", nxtSpace));
                                else stringBuilder.AppendLine(string.Format("{0}", obj.ToString(nxtSpace, true, obj.GetMetaDataDictionary())));
                            }
                            stringBuilder.Append(space).AppendLine("}");

                            #endregion
                        }
                        else
                        {
                            #region Set handler of other normally array

                            Array array = pair.Value.GetValue<Array>();
                            if (array == null)
                            {
                                stringBuilder.AppendLine(string.Format("{0}{1}: NULL", space, pair.Key.ToString("X2")));
                                continue;
                            }
                            string nxtSpace = space + "  ";
                            stringBuilder.AppendLine(string.Format("{0}{1}: ", space, pair.Key.ToString("X2"))).Append(space).AppendLine("{");
                            for (int j = 0; j < array.Length; j++)
                            {
                                Object obj = array.GetValue(j);
                                if (obj == null) stringBuilder.Append(nxtSpace).AppendLine("NULL");
                                else stringBuilder.AppendLine(string.Format("{0}{1}", nxtSpace, obj));
                            }
                            stringBuilder.Append(space).AppendLine("}");

                            #endregion
                        }

                        #endregion
                        break;
                }  
                }
                
            }
            return stringBuilder.ToString();
        }

        #endregion
    }
}