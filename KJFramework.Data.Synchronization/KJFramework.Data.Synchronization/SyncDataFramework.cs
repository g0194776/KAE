using KJFramework.Messages.Helpers;
using KJFramework.Messages.TypeProcessors.Maps;
using KJFramework.Net.Channels;
using KJFramework.Net.Transaction.Identities;
using KJFramework.Net.Transaction.Processors;

namespace KJFramework.Data.Synchronization
{
    /// <summary>
    ///     数据同步框架初始化全局函数
    /// </summary>
    public static class SyncDataFramework
    {
        #region Members

        private static bool _initialized;

        #endregion

        #region Methods

        /// <summary>
        ///     初始化全局信息
        /// </summary>
        public static void Initialize()
        {
            if (_initialized) return;
            FixedTypeManager.Add(typeof(MessageIdentity), 5);
            FixedTypeManager.Add(typeof(TransactionIdentity), 18);
            IntellectTypeProcessorMapping.Instance.Regist(new MessageIdentityProcessor());
            IntellectTypeProcessorMapping.Instance.Regist(new TransactionIdentityProcessor());
            SyncCounter.Instance.Initialize();
            GlobalMemory.Initialize();
            _initialized = true;
        }

        #endregion
    }
}