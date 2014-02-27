using KJFramework.Messages.Attributes;
using KJFramework.Net.Channels.Identities;
using KJFramework.ServiceModel.Bussiness.Default.Objects;

namespace KJFramework.ServiceModel.Bussiness.Default.Messages
{
    /// <summary>
    ///     响应服务消息，提供了相关的基本操作。
    ///     <para>* 对于普通的服务方法响应消息 : ProtocolKey = 1, ServiceKey = 0</para>
    /// </summary>
    public class ResponseServiceMessage : Message
    {
        #region Constructor
        
        /// <summary>
        ///     响应服务消息，提供了相关的基本操作。
        ///     <para>* 对于普通的服务方法响应消息 : ProtocolKey = 1, ServiceKey = 0</para>
        /// </summary>
        public ResponseServiceMessage() : this(null)
        {
        }

        /// <summary>
        ///     响应服务消息，提供了相关的基本操作。
        ///     <para>* 对于普通的服务方法响应消息 : ProtocolKey = 0, ServiceKey = 0</para>
        /// </summary>
        public ResponseServiceMessage(ServiceReturnValue serviceReturnValue)
        {
            ServiceReturnValue = serviceReturnValue;
            MessageIdentity = new MessageIdentity { ProtocolId = 0, ServiceId = 0, DetailsId = 1 };
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置请求方法对象
        /// </summary>
        [IntellectProperty(10, IsRequire = true)]
        public ServiceReturnValue ServiceReturnValue { get; set; }

        #endregion

        #region Implementation of IClearable

        /// <summary>
        /// 清除对象自身
        /// </summary>
        public override void Clear()
        {
            Body = null;
            ServiceReturnValue = null;
        }

        #endregion
    }
}