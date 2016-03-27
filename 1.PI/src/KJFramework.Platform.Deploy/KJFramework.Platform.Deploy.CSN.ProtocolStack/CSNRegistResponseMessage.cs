using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     CSNע�������Ϣ
    /// </summary>
    public class CSNRegistResponseMessage : CSNMessage
    {
        #region Constructor

        /// <summary>
        ///     CSNע�������Ϣ
        /// </summary>
        public CSNRegistResponseMessage()
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