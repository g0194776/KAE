using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
        /// <summary>
        ///     ж�ز��������Ϣ
        /// </summary>
    public class DSNUnDeployServiceResponseMessage : DSNMessage
    {
        #region Constrcutor

        /// <summary>
        ///     ж�ز��������Ϣ
        /// </summary>
        public DSNUnDeployServiceResponseMessage()
        {
            Header.ProtocolId = 10;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�Ƿ��Ѿ����ж�ط��������
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public bool IsSuccess { get; set; }
        /// <summary>
        ///     ��ȡ���������Ĵ�����Ϣ
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public string LastError { get; set; }

        #endregion
    }
}