using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Client.ProtocolStack
{
    /// <summary>
    ///     �ͻ��˻�ȡ���з��������Ϣ������Ϣ
    /// </summary>
    public class ClientGetServicesRequestMessage : ClientMessage
    {
        #region Constructor

        /// <summary>
        ///     �ͻ��˻�ȡ���з��������Ϣ������Ϣ
        /// </summary>
        public ClientGetServicesRequestMessage()
        {
            Header.ProtocolId = 2;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����÷�������
        ///     <para>* ����Э��涨�������ֵΪ��*ALL*, ���ʾ�������з����״̬</para>
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     ��ȡ�����û�����
        ///     <para>* ��ͨ����������������Ҫ��ȡ������Ϣ��SMC�� �����ֵ����Ϊ: *ALL*, �����ȫ�����������б��ط���Ҫ��ȡ��ϸ��Ϣ</para>
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public string MachineName { get; set; }

        #endregion
    }
}