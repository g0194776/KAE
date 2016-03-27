using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     ע��������Ϣ
    /// </summary>
    public class DSCUnRegistResponseMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     ע��������Ϣ
        /// </summary>
        public DSCUnRegistResponseMessage()
        {
            Header.ProtocolId = 3;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ������ע�����
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public bool Result { get; set; }

        #endregion
    }
}