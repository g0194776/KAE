using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     ж�ز���������Ϣ
    /// </summary>
    public class DSNUnDeployServiceRequestMessage : DSNMessage
    {
        #region Constrcutor

        /// <summary>
        ///     ж�ز���������Ϣ
        /// </summary>
        public DSNUnDeployServiceRequestMessage()
        {
            Header.ProtocolId = 9;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     ��ȡ������ж�ص�ԭ��
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public string Reason { get; set; }
        /// <summary>
        ///     ��ȡ�������Ƿ���Ҫж�ر���
        /// </summary>
        [IntellectProperty(13, IsRequire = true)]
        public bool IsDetailReport { get; set; }

        #endregion
    }
}