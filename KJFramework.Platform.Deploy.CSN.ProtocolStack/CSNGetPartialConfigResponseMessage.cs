using KJFramework.Messages.Attributes;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Transaction.Messages;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     获取部分配置信息回馈消息
    /// </summary>
    public class CSNGetPartialConfigResponseMessage : BaseMessage
    {
        #region Constructor

        /// <summary>
        ///     获取部分配置信息回馈消息
        /// </summary>
        public CSNGetPartialConfigResponseMessage()
        {
            MessageIdentity = new MessageIdentity { ProtocolId = 0, ServiceId = 4, DetailsId = 1 };
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置查询后的数据集合
        /// </summary>
        [IntellectProperty(11, AllowDefaultNull = true)]
        public byte ErrorId { get; set; }
        /// <summary>
        ///     获取或设置最后的错误信息
        /// </summary>
        [IntellectProperty(12)]
        public string LastError { get; set; }
        /// <summary>
        ///     获取或设置配置信息
        /// </summary>
        [IntellectProperty(13)]
        public string Config { get; set; }

        #endregion
    }
}