using KJFramework.Cache;
using KJFramework.Cache.Containers;

namespace KJFramework.Platform.Deploy.CSN.NetworkLayer
{
    internal static class CSNChannelConst
    {
        /// <summary>
        ///     传输通道缓冲区大小
        /// </summary>
        public static int RecvBufferSize = 4096;
        /// <summary>
        ///     底层SocketAsyncEventArgs缓存个数
        ///     <para>* 此类型缓存将会持有内存缓冲区</para>
        /// </summary>
        public static int BuffStubPoolSize = 1000;
        /// <summary>
        ///     底层SocketAsyncEventArgs缓存个数
        /// </summary>
        public static int NoBuffStubPoolSize = 1000;
        /// <summary>
        ///     此字段用于判断一个消息是否需要分包传输
        /// </summary>
        public static int MaxMessageDataLength = 1024000;
        public static ICacheTenant Tenant = new CacheTenant();
        public static IFixedCacheContainer<CSNBuffSocketStub> BuffAsyncStubPool = Tenant.Rent<CSNBuffSocketStub>("Pool::BuffSocketIOStub", BuffStubPoolSize);
        public static IFixedCacheContainer<CSNNoBuffSocketStub> NoBuffAsyncStubPool = Tenant.Rent<CSNNoBuffSocketStub>("Pool::NoBuffSocketIOStub", NoBuffStubPoolSize);
    }
}
