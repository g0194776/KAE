using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     ��ȡ�ؼ����������û�����Ϣ
    /// </summary>
    public class CSNGetKeyDataResponseMessage : CSNMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ�ؼ����������û�����Ϣ
        /// </summary>
        public CSNGetKeyDataResponseMessage()
        {
            Header.ProtocolId = 5;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����ò�ѯ������ݼ���
        /// </summary>
        [IntellectProperty(11, IsRequire = false)]
        public KeyData Datas { get; set; }
        /// <summary>
        ///     ��ȡ���������Ĵ�����Ϣ
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public string LastError { get; set; }

        #endregion
    }
}