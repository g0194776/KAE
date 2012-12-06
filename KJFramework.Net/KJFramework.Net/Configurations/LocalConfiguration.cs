using KJFramework.Attribute;
using KJFramework.Configurations;
namespace KJFramework.Net.Configurations
{
    [CustomerSection("KJFramework")]
    public sealed class LocalConfiguration : CustomerSection<LocalConfiguration>
    {
        /// <summary>
        ///     Õ¯¬Á≤„≈‰÷√œÓ
        /// </summary>
        [CustomerField("KJFramework.Net")]
        public NetworkLayerConfiguration NetworkLayer;
    }
}