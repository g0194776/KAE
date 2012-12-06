using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.SMC.Common.Performances
{
    /// <summary>
    ///     服务性能信息
    /// </summary>
    public class ServicePerformanceItem : IntellectObject
    {
        #region Members

        /// <summary>
        ///     获取或设置性能标识
        /// </summary>
        [IntellectProperty(0, IsRequire = true)]
        public string PerformanceName { get; set; }
        /// <summary>
        ///     获取或设置性能值
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public string PerformanceValue { get; set; }

        #endregion
    }
}