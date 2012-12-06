using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     ״̬����Ļ�����Ϣ
    /// </summary>
    public class DSCStatusChangeResponseMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     ״̬����Ļ�����Ϣ
        /// </summary>
        public DSCStatusChangeResponseMessage()
        {
            Header.ProtocolId = 9;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����ñ�����
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public bool Result { get; set; }

        #endregion
    }
}