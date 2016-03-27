using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Client.ProtocolStack
{
    /// <summary>
    ///     �����������������Ϣ
    /// </summary>
    public class ClientResetHeartBeatTimeRequestMessage : ClientMessage
    {
        #region Constructor

        /// <summary>
        ///     �����������������Ϣ
        /// </summary>
        public ClientResetHeartBeatTimeRequestMessage()
        {
            Header.ProtocolId = 5;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�������������
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public int Interval { get; set; }
        /// <summary>
        ///     ��ȡ��������������ʱ�����Ķ���
        ///     <para>* ����Э��涨�������ֵΪ *SMC*����֤��������SMC��ʱ����</para>
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public string Target { get; set; }
        /// <summary>
        ///     ��ȡ�����û�����
        ///     <para>* ������Target = *SMC*ʱ��������MachineName</para>
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public string MachineName { get; set; }

        #endregion
    }
}