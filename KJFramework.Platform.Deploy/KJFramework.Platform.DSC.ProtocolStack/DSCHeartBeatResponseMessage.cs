using KJFramework.Messages.Attributes;
using KJFramework.Platform.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     ����������Ϣ
    /// </summary>
    public class DSCHeartBeatResponseMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     ����������Ϣ
        /// </summary>
        public DSCHeartBeatResponseMessage()
        {
            Header.ProtocolId = 5;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�������������
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public bool Result { get; set; }

        #endregion
    }
}