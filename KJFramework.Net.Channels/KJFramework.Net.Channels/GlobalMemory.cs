using KJFramework.Cache.Containers;
using KJFramework.Net.Channels.Configurations;

namespace KJFramework.Net.Channels
{
    /// <summary>
    ///     全局内存管理器
    /// </summary>
    public static class GlobalMemory
    {
        #region Members

        /// <summary>
        ///     内存片段容器
        /// </summary>
        public static readonly IMemoryChunkCacheContainer SegmentContainer = new MemoryChunkCacheContainer(ChannelModelSettingConfigSection.Current.Settings.SegmentSize, ChannelModelSettingConfigSection.Current.Settings.SegmentBuffer);

        private static bool _initialized;

        #endregion

        #region Methods

        /// <summary>
        ///     Initialize global memory pool.
        /// </summary>
        public static void Initialize()
        {
            if (_initialized) return;
            ChannelCounter.Instance.Initialize();
            _initialized = true;
        }

        #endregion
    }
}