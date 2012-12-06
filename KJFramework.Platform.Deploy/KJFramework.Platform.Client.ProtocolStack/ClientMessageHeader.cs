using System;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Client.ProtocolStack
{
    /// <summary>
    ///     部署节点消息头，提供了相关的基本操作。
    /// </summary>
    public class ClientMessageHeader : IntellectObject
    {
        #region Constructor

        /// <summary>
        ///     部署节点消息头，提供了相关的基本操作。
        /// </summary>
        public ClientMessageHeader()
        {
            TaskId = DateTime.Now.Ticks.ToString();
        }

        #endregion

        #region Members

        [IntellectProperty(0, IsRequire = true)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1, IsRequire = true)]
        public int ServiceId { get; set; }
        [IntellectProperty(2, IsRequire = false)]
        public int DetailsId { get; set; }
        [IntellectProperty(3, IsRequire = true)]
        public int SessionId { get; set; }
        [IntellectProperty(4, IsRequire = true)]
        public string ClientTag { get; set; }
        [IntellectProperty(5, IsRequire = true)]
        public string TaskId { get; set; }

        #endregion
    }
}