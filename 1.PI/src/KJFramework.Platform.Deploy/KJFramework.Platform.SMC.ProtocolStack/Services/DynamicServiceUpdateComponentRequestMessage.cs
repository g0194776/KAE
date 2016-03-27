using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Services
{
    /// <summary>
    ///     ��̬����������������Ϣ
    /// </summary>
    public class DynamicServiceUpdateComponentRequestMessage : DynamicServiceMessage
    {
        #region Constructor

        /// <summary>
        ///     ��̬����������������Ϣ
        /// </summary>
        public DynamicServiceUpdateComponentRequestMessage()
        {
            Header.ProtocolId = 6;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�������������
        ///     <para>* ����Э��涨�������ֵΪ��*ALL*, ���ʾ���е������Ҫ����</para>
        /// </summary>
        [IntellectProperty(11, IsRequire = false)]
        public string ComponentName { get; set; }
        /// <summary>
        ///     ��ȡ�������ļ�����
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public string FileName { get; set; }

        #endregion
    }
}