using KJFramework.Cache;
using KJFramework.Cache.Containers;
using KJFramework.Net.Channels.Caches;
using KJFramework.Net.Channels.Configurations;

namespace KJFramework.Net.Channels
{
    /// <summary>
    ///   KJFramework内部网络层核心初始化基础类
    /// </summary>
    public static class ChannelConst
    {
        /// <summary>
        ///     传输通道缓冲区大小
        /// </summary>
        public static int RecvBufferSize = ChannelModelSettingConfigSection.Current == null || ChannelModelSettingConfigSection.Current.Settings == null
                                               ? 4096
                                               : ChannelModelSettingConfigSection.Current.Settings.RecvBufferSize;
        /// <summary>
        ///     底层SocketAsyncEventArgs缓存个数
        ///     <para>* 此类型缓存将会持有内存缓冲区</para>
        /// </summary>
        public static int BuffStubPoolSize = ChannelModelSettingConfigSection.Current == null || ChannelModelSettingConfigSection.Current.Settings == null
                                               ? 200000
                                               : ChannelModelSettingConfigSection.Current.Settings.BuffStubPoolSize;
        /// <summary>
        ///     底层提供给命名管道使用的缓冲区缓存个数
        ///     <para>* 此类型缓存将会持有内存缓冲区</para>
        /// </summary>
        public static int NamedPipeBuffStubPoolSize = ChannelModelSettingConfigSection.Current == null || ChannelModelSettingConfigSection.Current.Settings == null
                                               ? 200000
                                               : ChannelModelSettingConfigSection.Current.Settings.NamedPipeBuffStubPoolSize;
        /// <summary>
        ///     底层SocketAsyncEventArgs缓存个数
        /// </summary>
        public static int NoBuffStubPoolSize = ChannelModelSettingConfigSection.Current == null || ChannelModelSettingConfigSection.Current.Settings == null
                                               ? 200000
                                               : ChannelModelSettingConfigSection.Current.Settings.NoBuffStubPoolSize;
        /// <summary>
        ///     此字段用于判断一个消息是否需要分包传输
        /// </summary>
        public static int MaxMessageDataLength = ChannelModelSettingConfigSection.Current == null || ChannelModelSettingConfigSection.Current.Settings == null
                                               ? 5120
                                               : ChannelModelSettingConfigSection.Current.Settings.MaxMessageDataLength;
        /// <summary>
        ///     缓冲区内存片段大小
        /// </summary>
        public static int SegmentSize = ChannelModelSettingConfigSection.Current == null || ChannelModelSettingConfigSection.Current.Settings == null
                                               ? 5120
                                               : ChannelModelSettingConfigSection.Current.Settings.SegmentSize;
        /// <summary>
        ///     需要申请的缓冲区内存总大小
        /// </summary>
        public static int MemoryChunkSize = SegmentSize * (BuffStubPoolSize + NamedPipeBuffStubPoolSize);


        private static bool _initialized;
        /// <summary>
        ///     内存片段容器
        /// </summary>
        public static readonly IMemoryChunkCacheContainer SegmentContainer = new MemoryChunkCacheContainer(SegmentSize, MemoryChunkSize);
        public static ICacheTenant Tenant = new CacheTenant();
        public static IFixedCacheContainer<SocketBuffStub> BuffAsyncStubPool = Tenant.Rent<SocketBuffStub>("Pool::BuffSocketIOStub", BuffStubPoolSize);
        public static IFixedCacheContainer<BuffStub> NamedPipeBuffPool = Tenant.Rent<BuffStub>("Pool::NamedPipeIOStub", NamedPipeBuffStubPoolSize);
        public static IFixedCacheContainer<NoBuffSocketStub> NoBuffAsyncStubPool = Tenant.Rent<NoBuffSocketStub>("Pool::NoBuffSocketIOStub", NoBuffStubPoolSize);

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