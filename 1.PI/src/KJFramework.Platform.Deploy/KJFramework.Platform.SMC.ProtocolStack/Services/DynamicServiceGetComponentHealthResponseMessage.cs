using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Performances;

namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Services
{
    /// <summary>
    ///     ��ȡ�������״̬������Ϣ
    /// </summary>
    public class DynamicServiceGetComponentHealthResponseMessage : DynamicServiceMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ�������״̬������Ϣ
        /// </summary>
        public DynamicServiceGetComponentHealthResponseMessage()
        {
            Header.ProtocolId = 10;
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

        #endregion
    }
}