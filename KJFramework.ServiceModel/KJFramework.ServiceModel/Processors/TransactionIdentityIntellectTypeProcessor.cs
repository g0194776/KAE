using System;
using System.Net;
using KJFramework.Messages.Analysers;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Proxies;
using KJFramework.Messages.TypeProcessors;
using KJFramework.ServiceModel.Identity;

namespace KJFramework.ServiceModel.Processors
{
    /// <summary>
    ///     事务唯一标示处理器
    /// </summary>
    public class TransactionIdentityIntellectTypeProcessor : IntellectTypeProcessor
    {
        #region Constructor

        public TransactionIdentityIntellectTypeProcessor()
        {
            _supportedType = typeof (TransactionIdentity);
            _supportUnmanagement = true;
        }

        #endregion

        #region Overrides of IntellectTypeProcessor

        /// <summary>
        ///     从第三方客户数据转换为元数据
        /// </summary>
        /// <param name="memory">需要填充的字节数组</param>
        /// <param name="offset">需要填充数组的起始偏移量</param>
        /// <param name="attribute">当前字段标注的属性</param>
        /// <param name="value">第三方客户数据</param>
        public override void Process(byte[] memory, int offset, IntellectPropertyAttribute attribute, object value)
        {
            TransactionIdentity identity = (TransactionIdentity) value;
            unsafe
            {
                fixed (byte* pByte = &memory[offset])
                {
                    int innerOffset = 0;
                    *(long*)(pByte + innerOffset) = identity.Iep.Address.Address;
                    innerOffset += 8;
                    *(int*)(pByte + innerOffset) = identity.Iep.Port;
                    innerOffset += 4;
                    *(int*)(pByte + innerOffset) = identity.MessageId;
                    innerOffset += 4;
                    *(pByte + innerOffset) = (byte) (identity.IsOneway ? 1 : 0);
                    innerOffset++;
                    *(pByte + innerOffset) = (byte)(identity.IsRequest ? 1 : 0);
                }
            }
        }

        /// <summary>
        ///     从第三方客户数据转换为元数据
        /// </summary>
        /// <param name="proxy">内存片段代理器</param>
        /// <param name="attribute">字段属性</param>
        /// <param name="analyseResult">分析结果</param>
        /// <param name="target">目标对象实例</param>
        /// <param name="isArrayElement">当前写入的值是否为数组元素标示</param>
        /// <param name="isNullable">是否为可空字段标示</param>
        public override void Process(IMemorySegmentProxy proxy, IntellectPropertyAttribute attribute, ToBytesAnalyseResult analyseResult, object target, bool isArrayElement = false, bool isNullable = false)
        {
            TransactionIdentity identity = analyseResult.GetValue<TransactionIdentity>(target);
            proxy.WriteByte((byte) attribute.Id);
            proxy.WriteIPEndPoint(identity.Iep);
            proxy.WriteInt32(identity.MessageId);
            proxy.WriteByte((byte) (identity.IsOneway ? 1 : 0));
            proxy.WriteByte((byte) (identity.IsRequest ? 1 : 0));
        }

        /// <summary>
        ///     从第三方客户数据转换为元数据
        /// </summary>
        /// <param name="attribute">当前字段标注的属性</param>
        /// <param name="value">第三方客户数据</param>
        /// <returns>返回转换后的元数据</returns>
        /// <exception cref="Exception">转换失败</exception>
        public override byte[] Process(IntellectPropertyAttribute attribute, object value)
        {
            TransactionIdentity identity = (TransactionIdentity)value;
            byte[] data = new byte[18];
            unsafe
            {
                fixed (byte* pByte = data)
                {
                    int innerOffset = 0;
                    *(long*)(pByte + innerOffset) = identity.Iep.Address.Address;
                    innerOffset += 8;
                    *(int*)(pByte + innerOffset) = identity.Iep.Port;
                    innerOffset += 4;
                    *(int*)(pByte + innerOffset) = identity.MessageId;
                    innerOffset += 4;
                    *(pByte + innerOffset) = (byte)(identity.IsOneway ? 1 : 0);
                    innerOffset++;
                    *(pByte + innerOffset) = (byte)(identity.IsRequest ? 1 : 0);
                    return data;
                }
            }
        }

        /// <summary>
        ///     从元数据转换为第三方客户数据
        /// </summary>
        /// <param name="attribute">当前字段标注的属性</param>
        /// <param name="data">元数据</param>
        /// <returns>返回转换后的第三方客户数据</returns>
        /// <exception cref="Exception">转换失败</exception>
        public override object Process(IntellectPropertyAttribute attribute, byte[] data)
        {
            return Process(attribute, data, 0, 18);
        }

        /// <summary>
        ///     从元数据转换为第三方客户数据
        /// </summary>
        /// <param name="attribute">当前字段标注的属性</param>
        /// <param name="data">元数据</param>
        /// <param name="offset">元数据所在的偏移量</param>
        /// <param name="length">元数据长度</param>
        /// <returns>返回转换后的第三方客户数据</returns>
        /// <exception cref="Exception">转换失败</exception>
        public override object Process(IntellectPropertyAttribute attribute, byte[] data, int offset, int length = 0)
        {
            TransactionIdentity identity = new TransactionIdentity();
            unsafe
            {
                fixed (byte* pByte = &data[offset])
                {
                    int innerOffset = 0;
                    identity.Iep = new IPEndPoint(*(long*) pByte, *(int*) (pByte + 8));
                    innerOffset += 12;
                    identity.MessageId = *(int*)(pByte + innerOffset);
                    innerOffset += 4;
                    identity.IsOneway = *(pByte + innerOffset) == 1;
                    innerOffset++;
                    identity.IsRequest = *(pByte + innerOffset) == 1;
                    return identity;
                }
            }
        }

        /// <summary>
        ///     从元数据转换为第三方客户数据
        /// </summary>
        /// <param name="instance">目标对象</param>
        /// <param name="result">分析结果</param>
        /// <param name="data">元数据</param>
        /// <param name="offset">元数据所在的偏移量</param>
        /// <param name="length">元数据长度</param>
        public override void Process(object instance, GetObjectAnalyseResult result, byte[] data, int offset, int length = 0)
        {
            TransactionIdentity identity = new TransactionIdentity();
            unsafe
            {
                fixed (byte* pByte = &data[offset])
                {
                    int innerOffset = 0;
                    identity.Iep = new IPEndPoint(*(long*)pByte, *(int*)(pByte + 8));
                    innerOffset += 12;
                    identity.MessageId = *(int*)(pByte + innerOffset);
                    innerOffset += 4;
                    identity.IsOneway = *(pByte + innerOffset) == 1;
                    innerOffset++;
                    identity.IsRequest = *(pByte + innerOffset) == 1;
                }
            }
            result.SetValue(instance, identity);
        }

        #endregion
    }
}