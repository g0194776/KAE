using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     ��ȡ�ļ���ϸ��Ϣ������Ϣ
    /// </summary>
    public class DSCGetFileInfomationResponseMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ�ļ���ϸ��Ϣ������Ϣ
        /// </summary>
        public DSCGetFileInfomationResponseMessage()
        {
            Header.ProtocolId = 18;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     ��ȡ������Ҫ��ȡ��ϸ��Ϣ���б�
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public FileInfo[] Files { get; set; }
        /// <summary>
        ///     ��ȡ�����û�����
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public string MachineName { get; set; }

        #endregion
    }
}