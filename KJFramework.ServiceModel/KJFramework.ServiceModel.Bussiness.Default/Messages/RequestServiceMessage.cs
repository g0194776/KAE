using KJFramework.Messages.Attributes;
using KJFramework.Net.Channels.Identities;
using KJFramework.ServiceModel.Bussiness.Default.Objects;

namespace KJFramework.ServiceModel.Bussiness.Default.Messages
{
	/// <summary>
	///     请求服务消息，提供了相关的基本操作。
	///     <para>* 对于普通的服务方法请求消息 : ProtocolKey = 0, ServiceKey = 0</para>
	/// </summary>
	internal class RequestServiceMessage : Message
	{
		#region Constructor

		/// <summary>
		///     请求服务消息，提供了相关的基本操作。
		///     <para>* 对于普通的服务方法请求消息 : ProtocolKey = 0, ServiceKey = 0</para>
		/// </summary>
		public RequestServiceMessage()
		{
		    MessageIdentity = new MessageIdentity {ProtocolId = 0, ServiceId = 0, DetailsId = 0};
		}

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置请求方法对象
        /// </summary>
        [IntellectProperty(10, IsRequire = true)]
        public RequestMethodObject RequestObject { get; set; }

        #endregion

	    #region Implementation of IClearable

	    /// <summary>
	    /// 清除对象自身
	    /// </summary>
	    public override void Clear()
        {
            Body = null;
            TransactionIdentity = null;
            RequestObject = null;
	    }

	    #endregion
	}
}