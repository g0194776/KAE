using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     ��ȡ��������������Ϣ
    /// </summary>
    public class DSCGetServicesRequestMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ��������������Ϣ
        /// </summary>
        public DSCGetServicesRequestMessage()
        {
            Header.ProtocolId = 6;
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