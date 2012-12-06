using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Services
{
    /// <summary>
    ///     ��̬������������ʱ�䷴����Ϣ
    /// </summary>
    public class DynamicServiceResetHeartBeatTimeResponseMessage : DynamicServiceMessage
    {
        #region Constructor

        /// <summary>
        ///     ��̬������������ʱ�䷴����Ϣ
        /// </summary>
        public DynamicServiceResetHeartBeatTimeResponseMessage()
        {
            Header.ProtocolId = 5;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����ý��
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public bool Result { get; set; }
        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public string ServiceName { get; set; }

        #endregion
    }
}