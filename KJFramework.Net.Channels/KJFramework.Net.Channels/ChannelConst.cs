using KJFramework.Cache;
using KJFramework.Cache.Containers;
using KJFramework.Net.Channels.Caches;
using KJFramework.Net.Channels.Configurations;

namespace KJFramework.Net.Channels
{
    internal static class ChannelConst
    {
        /// <summary>
        ///     传输通道缓冲区大小
        /// </summary>
        public static int RecvBufferSize = ChannelModelSettingConfigSection.Current.Settings == null
                                               ? 4096
                                               : ChannelModelSettingConfigSection.Current.Settings.RecvBufferSize;
        /// <summary>
        ///     底层SocketAsyncEventArgs缓存个数
        ///     <para>* 此类型缓存将会持有内存缓冲区</para>
        /// </summary>
        public static int BuffStubPoolSize = ChannelModelSettingConfigSection.Current.Settings == null
                                               ? 200000
                                               : ChannelModelSettingConfigSection.Current.Settings.BuffStubPoolSize;
        /// <summary>
        ///     底层SocketAsyncEventArgs缓存个数
        /// </summary>
        public static int NoBuffStubPoolSize = ChannelModelSettingConfigSection.Current.Settings == null
                                               ? 200000
                                               : ChannelModelSettingConfigSection.Current.Settings.NoBuffStubPoolSize;
        /// <summary>
        ///     此字段用于判断一个消息是否需要分包传输
        /// </summary>
        public static int MaxMessageDataLength = ChannelModelSettingConfigSection.Current.Settings == null
                                               ? 5120
                                               : ChannelModelSettingConfigSection.Current.Settings.MaxMessageDataLength;
        public static ICacheTenant Tenant = new CacheTenant();
        public static IFixedCacheContainer<BuffSocketStub> BuffAsyncStubPool = Tenant.Rent<BuffSocketStub>("Pool::BuffSocketIOStub", BuffStubPoolSize);
        public static IFixedCacheContainer<NoBuffSocketStub> NoBuffAsyncStubPool = Tenant.Rent<NoBuffSocketStub>("Pool::NoBuffSocketIOStub", NoBuffStubPoolSize);
    }
}