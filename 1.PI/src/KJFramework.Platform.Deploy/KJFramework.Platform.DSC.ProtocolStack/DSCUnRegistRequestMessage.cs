using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     ע��������Ϣ
    /// </summary>
    public class DSCUnRegistRequestMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     ע��������Ϣ
        /// </summary>
        public DSCUnRegistRequestMessage()
        {
            Header.ProtocolId = 2;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����û�����
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string MachineName { get; set; }
        /// <summary>
        ///     ��ȡ������ע��ԭ��
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public string Reason { get; set; }

        #endregion
    }
}