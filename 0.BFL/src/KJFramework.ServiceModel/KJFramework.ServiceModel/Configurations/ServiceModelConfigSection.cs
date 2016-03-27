using KJFramework.Attribute;
using KJFramework.Configurations;

namespace KJFramework.ServiceModel.Configurations
{
    [CustomerSection("System")]
    internal class ServiceModelConfigSection : CustomerSection<ServiceModelConfigSection>
    {
        [CustomerField("Encoder")]
        public EncoderItem EncoderItem;
    }
}