using KJFramework.Attribute;
using KJFramework.Configurations;

namespace KJFramework.Tracing.Configuration
{
    [CustomerSection("Tracing")]
    public class TracingDescriptionConfigSection : CustomerSection<TracingDescriptionConfigSection>
    {
        /// <summary>
        ///   Dynamic Service配置项
        /// </summary>
        [CustomerField("TracingItem")]
        public TracingItemConfigration Details;
    }
}