using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     ��ȡ���ķ�����Ϣ������Ϣ
    /// </summary>
    public class DSCGetCoreServiceResponseMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ���ķ�����Ϣ������Ϣ
        /// </summary>
        public DSCGetCoreServiceResponseMessage()
        {
            Header.ProtocolId = 22;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����ú��ķ�����Ϣ����
        /// </summary>
        [IntellectProperty(11, IsRequire = false)]
        public CoreServiceItem[] Items { get; set; }

        #endregion
    }
}