using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Services
{
    /// <summary>
    ///     动态服务消息头，提供了相关的基本操作。
    /// </summary>
    public class DynamicServiceMessageHeader : IntellectObject
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
        [IntellectProperty(5, IsRequire = false)]
        public string TaskId { get; set; }

        #endregion
    }
}