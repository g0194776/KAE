using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Services
{
    /// <summary>
    ///     ��ȡ�ļ���ϸ��Ϣ������Ϣ
    /// </summary>
    public class DynamicServiceGetFileInfomationRequestMessage : DynamicServiceMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ�ļ���ϸ��Ϣ������Ϣ
        /// </summary>
        public DynamicServiceGetFileInfomationRequestMessage()
        {
            Header.ProtocolId = 11;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ������Ҫ��ȡ��ϸ��Ϣ���ļ��б�
        ///     <para>* Э��涨�������ֵΪ��*ALL*�򷵻������ļ�����ϸ��Ϣ</para>
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string Files { get; set; }

        #endregion
    }
}