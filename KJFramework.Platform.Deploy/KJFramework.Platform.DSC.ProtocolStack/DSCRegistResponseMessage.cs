using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     DSCע�������Ϣ
    /// </summary>
    public class DSCRegistResponseMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     DSCע�������Ϣ
        /// </summary>
        public DSCRegistResponseMessage()
        {
            Header.ProtocolId = 1;
        }

        #endregion

        #region Members
        
        /// <summary>
        ///     ��ȡ������ע��Ľ��
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public bool Result { get; set; }

        #endregion
    }
}