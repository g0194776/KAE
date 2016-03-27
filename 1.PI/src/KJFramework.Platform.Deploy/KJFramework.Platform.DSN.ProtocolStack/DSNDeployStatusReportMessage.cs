using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     ����״̬�㱨��Ϣ
    /// </summary>
    public class DSNDeployStatusReportMessage : DSNMessage
    {
        #region Constructor

        /// <summary>
        ///     ����״̬�㱨��Ϣ
        /// </summary>
        public DSNDeployStatusReportMessage()
        {
            Header.ProtocolId = 8;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ��������������
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string RequestToken { get; set; }
        /// <summary>
        ///     ��ȡ�����õ�ǰ״̬��Ϣ
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public string CurrentStatus { get; set; }

        #endregion
    }
}