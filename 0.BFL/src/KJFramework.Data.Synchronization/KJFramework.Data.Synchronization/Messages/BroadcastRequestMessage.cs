using KJFramework.Messages.Attributes;
using KJFramework.Net.Identities;
using KJFramework.Net.Transaction.Messages;

namespace KJFramework.Data.Synchronization.Messages
{
    public class BroadcastRequestMessage : BaseMessage
    {
        #region Constructor

        /// <summary>
        ///     广播请求消息
        /// </summary>
        public BroadcastRequestMessage()
        {
            MessageIdentity = new MessageIdentity
                                  {
                                      ProtocolId = 3,
                                      ServiceId = 0,
                                      DetailsId = 0
                                  };
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置广播所属的分组
        /// </summary>
        [IntellectProperty(10, IsRequire = true)]
        public string Catalog { get; set; }
        /// <summary>
        ///     获取或设置广播的KEY
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public byte[] Key { get; set; }
        /// <summary>
        ///     获取或设置广播的数据
        /// </summary>
        [IntellectProperty(12)]
        public byte[] Value { get; set; }

        #endregion 
    }
}