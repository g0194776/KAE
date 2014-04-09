using KJFramework.Messages.Contracts;

namespace KJFramework.ApplicationEngine.Processors
{
    /// <summary>
    ///    KAE - IntellectObject协议的消息处理器
    /// </summary>
    public abstract class IntellegenceKAEProcessor : KAEProcessor<IntellectObject>
    {
        #region Constructor

        /// <summary>
        ///    KAE - IntellectObject协议的消息处理器
        /// </summary>
        protected IntellegenceKAEProcessor(IApplication application)
            : base(application)
        {
        }

        #endregion
    }
}