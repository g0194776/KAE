using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Services
{
    /// <summary>
    ///     ��̬������������ʱ��������Ϣ
    /// </summary>
    public class DynamicServiceResetHeartBeatTimeRequestMessage : DynamicServiceMessage
    {
        #region Constructor

        /// <summary>
        ///     ��̬������������ʱ��������Ϣ
        /// </summary>
        public DynamicServiceResetHeartBeatTimeRequestMessage()
        {
            Header.ProtocolId = 4;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�������������
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public int Interval { get; set; }

        #endregion
    }
}