using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     ��ȡ���ķ�����Ϣ������Ϣ
    /// </summary>
    public class DSCGetCoreServiceRequestMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ���ķ�����Ϣ������Ϣ
        /// </summary>
        public DSCGetCoreServiceRequestMessage()
        {
            Header.ProtocolId = 21;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ������Ҫ��ȡ�ĺ��ķ������
        ///     <para>* SMC or DSN or *SERVICE*</para>
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string Category { get; set; }

        #endregion
    }
}