using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     �������������Ϣ
    /// </summary>
    public class DSCUpdateComponentRequestMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     �������������Ϣ
        /// </summary>
        public DSCUpdateComponentRequestMessage()
        {
            Header.ProtocolId = 12;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     ��ȡ�������������
        ///     <para>* ����Э��涨�������ֵΪ��*ALL*, ���ʾ���е������Ҫ����</para>
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public string ComponentName { get; set; }
        /// <summary>
        ///     ��ȡ�������ļ�����
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public string FileName { get; set; }
        /// <summary>
        ///     ��ȡ�����û�����
        /// </summary>
        [IntellectProperty(14, IsRequire = false)]
        public string MachineName { get; set; }

        #endregion
    }
}