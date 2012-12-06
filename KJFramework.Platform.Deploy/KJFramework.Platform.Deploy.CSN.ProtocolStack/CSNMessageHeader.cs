using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     ����ڵ���Ϣͷ���ṩ����صĻ���������
    /// </summary>
    public class CSNMessageHeader : IntellectObject
    {
        #region Members

        [IntellectProperty(0, IsRequire = true)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1, IsRequire = true)]
        public int ServiceId { get; set; }
        [IntellectProperty(2, IsRequire = false)]
        public int DetailsId { get; set; }
        [IntellectProperty(3, IsRequire = true)]
        public int SessionId { get; set; }
        [IntellectProperty(4, IsRequire = false)]
        public string ClientTag { get; set; }
        /// <summary>
        ///     ��ȡ�����÷����Ψһ����ֵ
        ///     <para>Ŀǰ��ֵ�涨Ϊ��MachineKey : ServiceName : ServiceVersion</para>
        /// </summary>
        [IntellectProperty(5, IsRequire = true)]
        public string ServiceKey { get; set; }

        #endregion
    }
}