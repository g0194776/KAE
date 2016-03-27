using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     ��ʼ����������Ϣ
    /// </summary>
    public class DSNBeginDeployMessage : DSNMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ʼ����������Ϣ
        /// </summary>
        public DSNBeginDeployMessage()
        {
            Header.ProtocolId = 6;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ��������������
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string RequestToken { get; set; }
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ���Ƿ���Ҫ�㱨����ϸ��
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public bool ReportDetail { get; set; }

        #endregion
    }
}