using KJFramework.Attribute;

namespace KJFramework.Tracing.Configuration
{
    /// <summary>
    ///     服务详细信息
    /// </summary>
    public class TracingItemConfigration
    {
        /// <summary>
        ///   日志级别
        /// </summary>
        [CustomerField("Level")]
        public string Level;
        /// <summary>
        ///   
        /// </summary>
        [CustomerField("Provider")]
        public string Provider;
        /// <summary>
        ///   
        /// </summary>
        [CustomerField("Datasource")]
        public string Datasource;
    }
}