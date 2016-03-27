using KJFramework.Messages.Contracts;
using KJFramework.Messages.Types;

namespace KJFramework.ApplicationEngine.Helpers
{
    internal static class ResourceBlockExtend
    {
        #region Methods.

        /// <summary>
        ///     Attempts packing a ResourceBlock as a MetadataContainer 
        /// </summary>
        /// <param name="resource">Targeted ResourceBlock object.</param>
        /// <returns>return a packed MetadataContainer object.</returns>
        public static MetadataContainer AsMetadataContainer(this ResourceBlock resource)
        {
            return new MetadataContainer(resource.GetMetaDataDictionary());
        }

        #endregion
    }
}