using KJFramework.Messages.ValueStored.DataProcessor.Mapping;
using KJFramework.Net;
using KJFramework.Net.Transaction.ValueStored;

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
            ExtensionTypeMapping.Regist(typeof(MessageIdentityValueStored));
            ExtensionTypeMapping.Regist(typeof(TransactionIdentityValueStored));
            SyncCounter.Instance.Initialize();
            ChannelConst.Initialize();
            _initialized = true;
        }

        #endregion
    }
}