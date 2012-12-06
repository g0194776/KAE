using KJFramework.Attribute;

namespace KJFramework.Dynamic.Configurations
{
    /// <summary>
    ///     服务详细信息
    /// </summary>
    public class InfoConfiguration
    {
        /// <summary>
        ///   服务名称
        /// </summary>
        [CustomerField("Name")]
        public string Name;
        /// <summary>
        ///   服务全名
        /// </summary>
        [CustomerField("ServiceName")]
        public string ServiceName;
        /// <summary>
        ///   服务描述
        /// </summary>
        [CustomerField("Description")]
        public string Description;
        /// <summary>
        ///   服务版本
        /// </summary>
        [CustomerField("Version")]
        public string Version;
    }
}