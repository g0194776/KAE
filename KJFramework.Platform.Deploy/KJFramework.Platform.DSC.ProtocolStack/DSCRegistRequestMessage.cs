using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Objects;
using KJFramework.Platform.Deploy.SMC.Common.Performances;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     DSCע��������Ϣ
    /// </summary>
    public class DSCRegistRequestMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     DSCע��������Ϣ
        /// </summary>
        public DSCRegistRequestMessage()
        {
            Header.ProtocolId = 0;
        }

        #endregion

        #region Members
        
        /// <summary>
        ///     ��ȡ�����û�����
        ///     <para>* Э���й涨��ÿ��SMC������Ļ�������ΪΨһ��ʶ</para>
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string MachineName { get; set; }
        /// <summary>
        ///     ��ȡ������ӵ�з������ϸ��Ϣ
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public OwnServiceItem[] OwnServics { get; set; }
        /// <summary>
        ///     ��ȡ���������������
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public ServicePerformanceItem[] PerformanceItems { get; set; }
        /// <summary>
        ///     ��ȡ������ע�����
        ///     <para>* ������ܰ���������࣬���磺����Ԫ����SMC</para>
        /// </summary>
        [IntellectProperty(14, IsRequire = true)]
        public string Category { get; set; }
        /// <summary>
        ///     ��ȡ�����ò����ַ
        ///     <para>* ������ֻ��������Ԫ</para>
        /// </summary>
        [IntellectProperty(15, IsRequire = false)]
        public string DeployAddress { get; set; }
        /// <summary>
        ///     ��ȡ������SMC�Ŀ��Ƶ�ַ
        /// </summary>
        [IntellectProperty(16, IsRequire = false)]
        public string ControlAddress { get; set; }

        #endregion

    }
}