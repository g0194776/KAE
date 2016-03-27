using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.SMC.Common.Performances
{
    /// <summary>
    ///     ������
    /// </summary>
    public class DomainPerformanceItem : IntellectObject
    {
        #region Members

        /// <summary>
        ///     ��ȡ������Ӧ�ó���������
        /// </summary>
        [IntellectProperty(0, IsRequire = true)]
        public string AppDomainName { get; set; }
        /// <summary>
        ///     ��ȡ������CPUʹ����
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public double Cpu { get; set; }
        /// <summary>
        ///     ��ȡ�������ڴ�ʹ����
        /// </summary>
        [IntellectProperty(2, IsRequire = true)]
        public double Memory { get; set; }

        #endregion
    }
}