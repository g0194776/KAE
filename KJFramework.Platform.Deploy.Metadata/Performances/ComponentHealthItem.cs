using KJFramework.Enums;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Performances
{
    /// <summary>
    ///     组件健康检查项
    /// </summary>
    public class ComponentHealthItem : IntellectObject
    {
        #region Members

        /// <summary>
        ///     获取或设置组件名称
        /// </summary>
        [IntellectProperty(0, IsRequire = true)]
        public string ComponentName { get; set; }
        /// <summary>
        ///     获取或设置健康状态
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public HealthStatus Status { get; set; }

        #endregion
    }
}