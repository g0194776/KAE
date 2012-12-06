using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.SMC.Common.Performances;
using KJFramework.Platform.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     ����������Ϣ
    /// </summary>
    public class DSCHeartBeatRequestMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     ��̬��������������Ϣ
        /// </summary>
        public DSCHeartBeatRequestMessage()
        {
            Header.ProtocolId = 4;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����û�����
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string MachineName { get; set; }
        /// <summary>
        ///     ��ȡ�����÷����е�����
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public ServicePerformanceItem[] PerformanceItems { get; set; }

        #endregion
    }
}