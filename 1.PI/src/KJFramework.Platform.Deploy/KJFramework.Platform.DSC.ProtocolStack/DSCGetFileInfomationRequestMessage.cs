using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     ��ȡ�ļ���ϸ��Ϣ������Ϣ
    /// </summary>
    public class DSCGetFileInfomationRequestMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ�ļ���ϸ��Ϣ������Ϣ
        /// </summary>
        public DSCGetFileInfomationRequestMessage()
        {
            Header.ProtocolId = 17;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ������Ҫ��ȡ��ϸ��Ϣ���ļ��б�
        ///     <para>* Э��涨�������ֵΪ��*ALL*�򷵻������ļ�����ϸ��Ϣ</para>
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string Files { get; set; }
        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     ��ȡ�����û�����
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public string MachineName { get; set; }

        #endregion
    }
}