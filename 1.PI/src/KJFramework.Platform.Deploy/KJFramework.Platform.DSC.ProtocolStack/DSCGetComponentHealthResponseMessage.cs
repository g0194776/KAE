using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Performances;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     ��ȡ�������״̬������Ϣ
    /// </summary>
    public class DSCGetComponentHealthResponseMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ�������״̬������Ϣ
        /// </summary>
        public DSCGetComponentHealthResponseMessage()
        {
            Header.ProtocolId = 15;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ������Ҫ��ȡ�������״̬�����Ƽ���
        ///     <para>����Э��Ҫ�������ֵΪ: *ALL*, ��㱨��������Ľ���״̬</para>
        /// </summary>
        [IntellectProperty(11, IsRequire = false)]
        public ComponentHealthItem[] Items { get; set; }
        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     ��ȡ��������������Ϣ
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public string LastError { get; set; }
        /// <summary>
        ///     ��ȡ�����û�����
        /// </summary>
        [IntellectProperty(14, IsRequire = true)]
        public string MachineName { get; set; }

        #endregion
    }
}