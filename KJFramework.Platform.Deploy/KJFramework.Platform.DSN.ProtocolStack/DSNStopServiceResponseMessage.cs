using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     ֹͣ���������Ϣ
    /// </summary>
    public class DSNStopServiceResponseMessage : DSNMessage
    {
        #region Constructor

        /// <summary>
        ///     ֹͣ���������Ϣ
        /// </summary>
        public DSNStopServiceResponseMessage()
        {
            Header.ProtocolId = 14;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�Ƿ�������ɹ�
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public bool IsSuccess { get; set; }
        /// <summary>
        ///     ��ȡ�����ô�����Ϣ
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