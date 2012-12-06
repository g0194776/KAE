using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     ��ȡ�������������Ϣ
    /// </summary>
    public class DSCGetServicesResponseMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ�������������Ϣ
        /// </summary>
        public DSCGetServicesResponseMessage()
        {
            Header.ProtocolId = 7;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����÷�����ϸ��Ϣ
        /// </summary>
        [IntellectProperty(11, IsRequire = false)]
        public OwnServiceItem[] Items { get; set; }
        /// <summary>
        ///     ��ȡ�����û�����
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public string MachineName { get; set; }

        #endregion
    }
}