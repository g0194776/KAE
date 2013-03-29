using System.Net;
using KJFramework.Messages.Analysers;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Proxies;
using KJFramework.Messages.TypeProcessors;
using KJFramework.Net.Transaction.Identities;

namespace KJFramework.Net.Transaction.Processors
{
    /// <summary>
    ///     事物唯一标示处理器
    /// </summary>
    public class TransactionIdentityProcessor : IntellectTypeProcessor
    {
        #region Constructor

        /// <summary>
        ///     事物唯一标示处理器
        /// </summary>
        public TransactionIdentityProcessor()
        {
            _supportedType = typeof (TransactionIdentity);
            _supportUnmanagement = true;
        }

        #endregion

        #region Overrides of IntellectTypeProcessor
        
        /// <summary>
        /// 从第三方客户数据转换为元数据
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
            if (identity == null) return;
            proxy.WriteByte((byte)attribute.Id);
            proxy.WriteByte((byte)(identity.IsRequest ? 1 : 0));
            proxy.WriteByte((byte)(identity.IsOneway ? 1 : 0));
            proxy.WriteIPEndPoint(identity.EndPoint);
            proxy.WriteInt32(identity.MessageId);
        }

        /// <summary>
        ///     从第三方客户数据转换为元数据
        ///     <para>* 此方法将会被轻量级的DataHelper所使用，并且写入的数据将不会拥有编号(Id)</para>
        /// </summary>
        /// <param name="proxy">内存片段代理器</param>
        /// <param name="target">目标对象实例</param>
        /// <param name="isArrayElement">当前写入的值是否为数组元素标示</param>
        /// <param name="isNullable">是否为可空字段标示</param>
        public override void Process(IMemorySegmentProxy proxy, object target, bool isArrayElement = false, bool isNullable = false)
        {
            TransactionIdentity identity = (TransactionIdentity) target;
            if (identity == null) return;
            proxy.WriteByte((byte)(identity.IsRequest ? 1 : 0));
            proxy.WriteByte((byte)(identity.IsOneway ? 1 : 0));
            proxy.WriteIPEndPoint(identity.EndPoint);
            proxy.WriteInt32(identity.MessageId);
        }

        /// <summary>
        /// 从元数据转换为第三方客户数据
        /// </summary>
        /// <param name="attribute">当前字段标注的属性</param>
        /// <param name="data">元数据</param>
        /// <returns>
        /// 返回转换后的第三方客户数据
        /// </returns>
        /// <exception cref="N:KJFramework.Exception">转换失败</exception>
        public override object Process(IntellectPropertyAttribute attribute, byte[] data)
        {
            TransactionIdentity identity = new TransactionIdentity();
            unsafe
            {
                fixed (byte* pData = data)
                {
                    identity.IsRequest = *pData == 1;
                    identity.IsOneway = *(pData + 1) == 1;
                    identity.EndPoint = new IPEndPoint(new IPAddress(*(long*)(pData + 2)), *(int*)(pData + 10));
                    identity.MessageId = *(int*)(pData + 14);
                }
            }
            return identity;
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
                fixed (byte* pData = &data[offset])
                {
                    identity.IsRequest = *pData == 1;
                    identity.IsOneway = *(pData + 1) == 1;
                    identity.EndPoint = new IPEndPoint(new IPAddress(*(long*)(pData + 2)), *(int*)(pData + 10));
                    identity.MessageId = *(int*)(pData + 14);
                }
            }
            result.SetValue(instance, identity);
        }

        #endregion
    }
}