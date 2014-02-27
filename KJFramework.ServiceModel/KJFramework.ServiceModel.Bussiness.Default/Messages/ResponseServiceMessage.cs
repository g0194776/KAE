using KJFramework.Messages.Attributes;
using KJFramework.Net.Channels.Identities;
using KJFramework.ServiceModel.Bussiness.Default.Objects;

namespace KJFramework.ServiceModel.Bussiness.Default.Messages
{
    /// <summary>
    ///     ��Ӧ������Ϣ���ṩ����صĻ���������
    ///     <para>* ������ͨ�ķ��񷽷���Ӧ��Ϣ : ProtocolKey = 1, ServiceKey = 0</para>
    /// </summary>
    public class ResponseServiceMessage : Message
    {
        #region Constructor
        
        /// <summary>
        ///     ��Ӧ������Ϣ���ṩ����صĻ���������
        ///     <para>* ������ͨ�ķ��񷽷���Ӧ��Ϣ : ProtocolKey = 1, ServiceKey = 0</para>
        /// </summary>
        public ResponseServiceMessage() : this(null)
        {
        }

        /// <summary>
        ///     ��Ӧ������Ϣ���ṩ����صĻ���������
        ///     <para>* ������ͨ�ķ��񷽷���Ӧ��Ϣ : ProtocolKey = 0, ServiceKey = 0</para>
        /// </summary>
        public ResponseServiceMessage(ServiceReturnValue serviceReturnValue)
        {
            ServiceReturnValue = serviceReturnValue;
            MessageIdentity = new MessageIdentity { ProtocolId = 0, ServiceId = 0, DetailsId = 1 };
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ���������󷽷�����
        /// </summary>
        [IntellectProperty(10, IsRequire = true)]
        public ServiceReturnValue ServiceReturnValue { get; set; }

        #endregion

        #region Implementation of IClearable

        /// <summary>
        /// �����������
        /// </summary>
        public override void Clear()
        {
            Body = null;
            ServiceReturnValue = null;
        }

        #endregion
    }
}