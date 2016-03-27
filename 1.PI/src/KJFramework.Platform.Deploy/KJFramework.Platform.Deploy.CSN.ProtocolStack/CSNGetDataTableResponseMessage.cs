using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     ��ȡ���ñ������Ϣ
    /// </summary>
    public class CSNGetDataTableResponseMessage : CSNMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ���ñ������Ϣ
        /// </summary>
        public CSNGetDataTableResponseMessage()
        {
            Header.ProtocolId = 3;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ���������ݿ����
        /// </summary>
        [IntellectProperty(11, IsRequire = false)]
        public DataTable[] Tables { get; set; }
        /// <summary>
        ///     ��ȡ��������������Ϣ
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public string LastError { get; set; }

        #endregion
    }
}