using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Services
{
    /// <summary>
    ///     ��ȡ�ļ���ϸ��Ϣ������Ϣ
    /// </summary>
    public class DynamicServiceGetFileInfomationResponseMessage : DynamicServiceMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ�ļ���ϸ��Ϣ������Ϣ
        /// </summary>
        public DynamicServiceGetFileInfomationResponseMessage()
        {
            Header.ProtocolId = 12;
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
        
        #endregion
    }
}