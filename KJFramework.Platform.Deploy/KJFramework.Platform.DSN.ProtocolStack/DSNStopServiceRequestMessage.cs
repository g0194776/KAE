using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     ֹͣ����������Ϣ
    /// </summary>
    public class DSNStopServiceRequestMessage : DSNMessage
    {
        #region Constructor

        /// <summary>
        ///     ֹͣ����������Ϣ
        /// </summary>
        public DSNStopServiceRequestMessage()
        {
            Header.ProtocolId = 13;
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