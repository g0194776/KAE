using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     ��������ʱ�������Ϣ
    /// </summary>
    public class DSCResetHeartBeatTimeResponseMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     ��������ʱ�������Ϣ
        /// </summary>
        public DSCResetHeartBeatTimeResponseMessage()
        {
            Header.ProtocolId = 11;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ��������������ʱ��Ľ��
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public bool Result { get; set; }
        /// <summary>
        ///     ��ȡ�����û�������
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public string MachineName { get; set; }

        #endregion
    }
}