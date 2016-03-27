using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Objects
{
    /// <summary>
    ///     关键字数据对象，提供了相关的基本属性结构
    /// </summary>
    public class KeyData : IntellectObject
    {
        #region Members

        /// <summary>
        ///     获取或设置数据行
        /// </summary>
        [IntellectProperty(0, IsRequire = false)]
        public DataRow[] Rows { get; set; }
        /// <summary>
        ///     获取或设置列名集合
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public string[] Columns { get; set; }

        #endregion
    }
}