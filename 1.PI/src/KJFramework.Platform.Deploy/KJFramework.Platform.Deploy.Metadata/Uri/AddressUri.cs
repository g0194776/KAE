using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Uri
{
    /// <summary>
    ///     地址路径对象
    /// </summary>
    public class AddressUri : IntellectObject
    {
        #region Members

        /// <summary>
        ///     获取或设置地址关键字
        /// </summary>
        [IntellectProperty(0, IsRequire = true)]
        public string Key { get; set; }
        /// <summary>
        ///     获取或设置地址值
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public string Value { get; set; }
        /// <summary>
        ///     获取或设置地址描述
        /// </summary>
        [IntellectProperty(2, IsRequire = false)]
        public string Description { get; set; }
        /// <summary>
        ///     获取或设置地址附加属性字段
        /// </summary>
        [IntellectProperty(3, IsRequire = false)]
        public string Tag { get; set; }

        #endregion
    }
}