using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     ��������������Ϣ
    /// </summary>
    public class DSNStartServiceRequestMessage : DSNMessage
    {
        #region Constructor

        /// <summary>
        ///     ��������������Ϣ
        /// </summary>
        public DSNStartServiceRequestMessage()
        {
            Header.ProtocolId = 11;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string ServiceName { get; set; }

        #endregion
    }
}