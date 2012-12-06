using KJFramework.Attribute;
using KJFramework.Configurations;

namespace KJFramework.Dynamic.Configurations
{
    [CustomerSection("Service")]
    public class ServiceDescriptionConfigSection : CustomerSection<ServiceDescriptionConfigSection>
    {
        /// <summary>
        ///   Dynamic Service配置项
        /// </summary>
        [CustomerField("Infomation")]
        public InfoConfiguration Details;
    }
}