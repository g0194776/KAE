using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Services
{
    /// <summary>
    ///     ��ȡ�������״̬������Ϣ
    /// </summary>
    public class DynamicServiceGetComponentHealthRequestMessage : DynamicServiceMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ�������״̬������Ϣ
        /// </summary>
        public DynamicServiceGetComponentHealthRequestMessage()
        {
            Header.ProtocolId = 9;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ������Ҫ��ȡ�������״̬�����Ƽ���
        ///     <para>����Э��Ҫ�������ֵΪ: *ALL*, ��㱨��������Ľ���״̬</para>
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string[] Components { get; set; }

        #endregion
    }
}