using KJFramework.Attribute;
using KJFramework.Configurations;
namespace KJFramework.Net.Configurations
{
    [CustomerSection("KJFramework")]
    public sealed class LocalConfiguration : CustomerSection<LocalConfiguration>
    {
        /// <summary>
        ///     �����������
        /// </summary>
        [CustomerField("KJFramework.Net")]
        public NetworkLayerConfiguration NetworkLayer;
    }
}