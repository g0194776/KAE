using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     ��������������Ϣ
    /// </summary>
    public class DSNEndDeployMessage : DSNMessage
    {
        #region Constructor

        /// <summary>
        ///    ��������������Ϣ
        /// </summary>
        public DSNEndDeployMessage()
        {
            Header.ProtocolId = 7;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ��������������
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string RequestToken { get; set; }
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ���Ƿ��Ѿ��������
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public bool IsDeployed { get; set; }
        /// <summary>
        ///     ��ȡ���������Ĵ�����Ϣ
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public string LastError { get; set; }

        #endregion
    }
}