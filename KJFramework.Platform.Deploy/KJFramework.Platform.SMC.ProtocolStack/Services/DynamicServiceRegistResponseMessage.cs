using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Uri;

namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Services
{
    /// <summary>
    ///     ��̬����ע�������Ϣ
    /// </summary>
    public class DynamicServiceRegistResponseMessage : DynamicServiceMessage
    {
        #region Constructor

        /// <summary>
        ///     ��̬����ע�������Ϣ
        /// </summary>
        public DynamicServiceRegistResponseMessage()
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
        /// <summary>
        ///     ��ȡ����������Shell֪������ص�ַ�б�
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public AddressUri[] Addresses { get; set; }

        #endregion
    }
}