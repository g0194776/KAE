using KJFramework.Messages.Attributes;
using KJFramework.Net.Channels.Identities;
using KJFramework.ServiceModel.Bussiness.Default.Objects;

namespace KJFramework.ServiceModel.Bussiness.Default.Messages
{
	/// <summary>
	///     ���������Ϣ���ṩ����صĻ���������
	///     <para>* ������ͨ�ķ��񷽷�������Ϣ : ProtocolKey = 0, ServiceKey = 0</para>
	/// </summary>
	internal class RequestServiceMessage : Message
	{
		#region Constructor

		/// <summary>
		///     ���������Ϣ���ṩ����صĻ���������
		///     <para>* ������ͨ�ķ��񷽷�������Ϣ : ProtocolKey = 0, ServiceKey = 0</para>
		/// </summary>
		public RequestServiceMessage()
		{
		    MessageIdentity = new MessageIdentity {ProtocolId = 0, ServiceId = 0, DetailsId = 0};
		}

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ���������󷽷�����
        /// </summary>
        [IntellectProperty(10, IsRequire = true)]
        public RequestMethodObject RequestObject { get; set; }

        #endregion

	    #region Implementation of IClearable

	    /// <summary>
	    /// �����������
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