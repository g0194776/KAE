using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.SMC.Common.Performances
{
    /// <summary>
    ///     ����������Ϣ
    /// </summary>
    public class ServicePerformanceItem : IntellectObject
    {
        #region Members

        /// <summary>
        ///     ��ȡ���������ܱ�ʶ
        /// </summary>
        [IntellectProperty(0, IsRequire = true)]
        public string PerformanceName { get; set; }
        /// <summary>
        ///     ��ȡ����������ֵ
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public string PerformanceValue { get; set; }

        #endregion
    }
}