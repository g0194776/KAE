using KJFramework.Messages.Contracts;

namespace KJFramework.ApplicationEngine.Processors
{
    /// <summary>
    ///    KAE - Metadata协议的消息处理器
    /// </summary>
    public abstract class MetadataKAEProcessor : KAEProcessor<MetadataContainer>
    {
        #region Constructor

        /// <summary>
        ///    KAE - Metadata协议的消息处理器
        /// </summary>
        protected MetadataKAEProcessor(IApplication application)
            : base(application)
        {
        }

        #endregion
    }
}