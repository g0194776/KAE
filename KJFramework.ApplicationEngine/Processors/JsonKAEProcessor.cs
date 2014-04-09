namespace KJFramework.ApplicationEngine.Processors
{
    /// <summary>
    ///    KAE - JSON协议的消息处理器
    /// </summary>
    public abstract class JsonKAEProcessor : KAEProcessor<string>
    {
        #region Constructor

        /// <summary>
        ///    KAE - JSON协议的消息处理器
        /// </summary>
        protected JsonKAEProcessor(IApplication application)
            : base(application)
        {
        }

        #endregion
    }
}