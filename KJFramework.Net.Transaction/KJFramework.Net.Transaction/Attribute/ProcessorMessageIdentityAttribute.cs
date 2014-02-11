using System;

namespace KJFramework.Net.Transaction.Attribute
{
    /// <summary>
    ///     处理器的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ProcessorMessageIdentityAttribute : System.Attribute
    {
        #region Members

        /// <summary>
        ///     获取协议ID
        /// </summary>
        public byte ProtocolId { get; set; }
        /// <summary>
        ///     获取服务角色ID
        /// </summary>
        public byte ServiceId { get; set; }
        /// <summary>
        ///     获取详细信息ID
        /// </summary>
        public byte DetailsId { get; set; }

        #endregion
    }
}
