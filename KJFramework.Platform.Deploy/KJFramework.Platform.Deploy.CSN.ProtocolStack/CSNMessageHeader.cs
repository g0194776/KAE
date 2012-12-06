using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     部署节点消息头，提供了相关的基本操作。
    /// </summary>
    public class CSNMessageHeader : IntellectObject
    {
        #region Members

        [IntellectProperty(0, IsRequire = true)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1, IsRequire = true)]
        public int ServiceId { get; set; }
        [IntellectProperty(2, IsRequire = false)]
        public int DetailsId { get; set; }
        [IntellectProperty(3, IsRequire = true)]
        public int SessionId { get; set; }
        [IntellectProperty(4, IsRequire = false)]
        public string ClientTag { get; set; }
        /// <summary>
        ///     获取或设置服务的唯一序列值
        ///     <para>目前该值规定为：MachineKey : ServiceName : ServiceVersion</para>
        /// </summary>
        [IntellectProperty(5, IsRequire = true)]
        public string ServiceKey { get; set; }

        #endregion
    }
}