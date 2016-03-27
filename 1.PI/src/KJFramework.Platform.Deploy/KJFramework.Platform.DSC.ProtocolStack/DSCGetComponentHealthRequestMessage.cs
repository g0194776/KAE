using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     ��ȡ�������״̬������Ϣ
    /// </summary>
    public class DSCGetComponentHealthRequestMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ�������״̬������Ϣ
        /// </summary>
        public DSCGetComponentHealthRequestMessage()
        {
            Header.ProtocolId = 14;
        }

        #endregion

        #region Members

        [IntellectProperty(11, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     ��ȡ������Ҫ��ȡ�������״̬�����Ƽ���
        ///     <para>����Э��Ҫ�������ֵΪ: *ALL*, ��㱨��������Ľ���״̬</para>
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public string[] Components { get; set; }
        /// <summary>
        ///     ��ȡ�����û�����
        /// </summary>
        [IntellectProperty(13, IsRequire = true)]
        public string MachineName { get; set; }

        #endregion
    }
}