using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.SMC.Common.Performances
{
    /// <summary>
    ///     域性能
    /// </summary>
    public class DomainPerformanceItem : IntellectObject
    {
        #region Members

        /// <summary>
        ///     获取或设置应用程序域名称
        /// </summary>
        [IntellectProperty(0, IsRequire = true)]
        public string AppDomainName { get; set; }
        /// <summary>
        ///     获取或设置CPU使用率
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public double Cpu { get; set; }
        /// <summary>
        ///     获取或设置内存使用率
        /// </summary>
        [IntellectProperty(2, IsRequire = true)]
        public double Memory { get; set; }

        #endregion
    }
}