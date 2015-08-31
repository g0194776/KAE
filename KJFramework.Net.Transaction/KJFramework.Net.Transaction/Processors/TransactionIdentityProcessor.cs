﻿using System;
﻿using KJFramework.Messages.Analysers;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Proxies;
using KJFramework.Messages.TypeProcessors;
﻿using KJFramework.Net.Enums;
﻿using KJFramework.Net.Identities;

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
            if (identity.IdentityType == TransactionIdentityTypes.TCP) TCPTransactionIdentity.Serialize((byte) attribute.Id, IdentitySerializationTypes.IntellectObject, (TCPTransactionIdentity) identity, proxy);
            else if (identity.IdentityType == TransactionIdentityTypes.NamedPipe) NamedPipeTransactionIdentity.Serialize((byte)attribute.Id, IdentitySerializationTypes.IntellectObject, (NamedPipeTransactionIdentity)identity, proxy);
            else throw new NotSupportedException(string.Format("#We were not support current type of Transaction-Identity yet! {0}", identity.IdentityType));
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
            TransactionIdentity identity = (TransactionIdentity)target;
            if (identity == null) return;
            if (identity.IdentityType == TransactionIdentityTypes.TCP) TCPTransactionIdentity.Serialize(0, IdentitySerializationTypes.IntellectObject, (TCPTransactionIdentity)identity, proxy);
            else if (identity.IdentityType == TransactionIdentityTypes.NamedPipe) NamedPipeTransactionIdentity.Serialize(0, IdentitySerializationTypes.IntellectObject, (NamedPipeTransactionIdentity)identity, proxy);
            else throw new NotSupportedException(string.Format("#We were not support current type of Transaction-Identity yet! {0}", identity.IdentityType));
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
            TransactionIdentityTypes identityType = (TransactionIdentityTypes)data[0];
            if (identityType == TransactionIdentityTypes.TCP) return TCPTransactionIdentity.Deserialize(data, 0, data.Length);
            if (identityType == TransactionIdentityTypes.NamedPipe) return NamedPipeTransactionIdentity.Deserialize(data, 0, data.Length);
            throw new NotSupportedException(string.Format("#We were not support current type of Transaction-Identity yet! {0}", identityType));
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
            TransactionIdentityTypes identityType = (TransactionIdentityTypes)data[offset];
            if (identityType == TransactionIdentityTypes.TCP) result.SetValue(instance, TCPTransactionIdentity.Deserialize(data, offset, length));
            else if (identityType == TransactionIdentityTypes.NamedPipe) result.SetValue(instance, NamedPipeTransactionIdentity.Deserialize(data, offset, length));
            else throw new NotSupportedException(string.Format("#We were not support current type of Transaction-Identity yet! {0}", identityType));
        }

        #endregion
    }
}