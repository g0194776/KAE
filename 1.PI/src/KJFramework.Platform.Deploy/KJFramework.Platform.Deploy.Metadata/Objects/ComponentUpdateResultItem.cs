using System;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Objects
{
    public class ComponentUpdateResultItem : IntellectObject
    {
        #region Members

        /// <summary>
        ///     获取或设置组件名称
        /// </summary>
        [IntellectProperty(0, IsRequire = true)]
        public string ComponentName { get; set; }
        /// <summary>
        ///     获取或设置更新结果
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public bool Result { get; set; }
        /// <summary>
        ///     获取或设置错误信息
        /// </summary>
        [IntellectProperty(2, IsRequire = false)]
        public string ErrorTrace { get; set; }
        /// <summary>
        ///     获取或设置组件最后更新时间
        /// </summary>
        [IntellectProperty(3, IsRequire = false)]
        public DateTime LastUpdateTime { get; set; }

        #endregion
    }
}