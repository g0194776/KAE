using KJFramework.Messages.Attributes;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Transaction.Messages;

namespace KJFramework.Data.Synchronization.Messages
{
    /// <summary>
    ///     同步数据请求消息
    /// </summary>
    public class SyncDataRequestMessage : BaseMessage
    {
        #region Constructor

        /// <summary>
        ///     同步数据请求消息
        /// </summary>
        public SyncDataRequestMessage()
        {
            MessageIdentity = new MessageIdentity
                                  {
                                      ProtocolId = 2,
                                      ServiceId = 0,
                                      DetailsId = 0
                                  };
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置同步数据所属的分组
        /// </summary>
        [IntellectProperty(10, IsRequire = true)]
        public string Catalog { get; set; }
        /// <summary>
        ///     获取或设置同步数据的KEY
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public byte[] Key { get; set; }
        /// <summary>
        ///     获取或设置同步数据
        /// </summary>
        [IntellectProperty(12)]
        public byte[] Value { get; set; }

        #endregion
    }
}
