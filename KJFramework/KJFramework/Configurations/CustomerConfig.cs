using System.Configuration;

namespace KJFramework.Configurations
{
    /// <summary>
    ///     客户自定义配置节
    /// </summary>
    public class CustomerConfig : IConfigurationSectionHandler
    {
        #region IConfigurationSectionHandler 成员

        /// <summary>
        /// Creates a configuration section handler.
        /// </summary>
        /// <returns>
        /// The created section handler object.
        /// </returns>
        /// <param name="parent">Parent object.
        ///                 </param><param name="configContext">Configuration context object.
        ///                 </param><param name="section">Section XML node.
        ///                 </param><filterpriority>2</filterpriority>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            return null;
        }

        #endregion
    }
}