using KJFramework.Cache.Containers;

namespace KJFramework.Platform.Deploy.CSN.NetworkLayer
{
    /// <summary>
    ///     全局内存管理器
    /// </summary>
    public static class CSNGlobalMemory
    {
        #region Members

        /// <summary>
        ///     内存片段容器
        /// </summary>
        public static readonly IMemoryChunkCacheContainer SegmentContainer = new MemoryChunkCacheContainer(5120, 5120000);

        private static bool _initialized;

        #endregion

        #region Methods

        /// <summary>
        ///     Initialize global memory pool.
        /// </summary>
        public static void Initialize()
        {
            if (_initialized) return;
            CSNChannelCounter.Instance.Initialize();
            _initialized = true;
        }

        #endregion
    }
}