using KJFramework.Cache;
using KJFramework.Cache.Containers;
using KJFramework.Net.Channels.Caches;
using KJFramework.Net.Channels.Configurations;

namespace KJFramework.Net.Channels
{
    /// <summary>
    ///   KJFramework�ڲ��������ĳ�ʼ��������
    /// </summary>
    public static class ChannelConst
    {
        /// <summary>
        ///     ����ͨ����������С
        /// </summary>
        public static int RecvBufferSize = ChannelModelSettingConfigSection.Current == null || ChannelModelSettingConfigSection.Current.Settings == null
                                               ? 4096
                                               : ChannelModelSettingConfigSection.Current.Settings.RecvBufferSize;
        /// <summary>
        ///     �ײ�SocketAsyncEventArgs�������
        ///     <para>* �����ͻ��潫������ڴ滺����</para>
        /// </summary>
        public static int BuffStubPoolSize = ChannelModelSettingConfigSection.Current == null || ChannelModelSettingConfigSection.Current.Settings == null
                                               ? 200000
                                               : ChannelModelSettingConfigSection.Current.Settings.BuffStubPoolSize;
        /// <summary>
        ///     �ײ��ṩ�������ܵ�ʹ�õĻ������������
        ///     <para>* �����ͻ��潫������ڴ滺����</para>
        /// </summary>
        public static int NamedPipeBuffStubPoolSize = ChannelModelSettingConfigSection.Current == null || ChannelModelSettingConfigSection.Current.Settings == null
                                               ? 200000
                                               : ChannelModelSettingConfigSection.Current.Settings.NamedPipeBuffStubPoolSize;
        /// <summary>
        ///     �ײ�SocketAsyncEventArgs�������
        /// </summary>
        public static int NoBuffStubPoolSize = ChannelModelSettingConfigSection.Current == null || ChannelModelSettingConfigSection.Current.Settings == null
                                               ? 200000
                                               : ChannelModelSettingConfigSection.Current.Settings.NoBuffStubPoolSize;
        /// <summary>
        ///     ���ֶ������ж�һ����Ϣ�Ƿ���Ҫ�ְ�����
        /// </summary>
        public static int MaxMessageDataLength = ChannelModelSettingConfigSection.Current == null || ChannelModelSettingConfigSection.Current.Settings == null
                                               ? 5120
                                               : ChannelModelSettingConfigSection.Current.Settings.MaxMessageDataLength;
        /// <summary>
        ///     �������ڴ�Ƭ�δ�С
        /// </summary>
        public static int SegmentSize = ChannelModelSettingConfigSection.Current == null || ChannelModelSettingConfigSection.Current.Settings == null
                                               ? 5120
                                               : ChannelModelSettingConfigSection.Current.Settings.SegmentSize;
        /// <summary>
        ///     ��Ҫ����Ļ������ڴ��ܴ�С
        /// </summary>
        public static int MemoryChunkSize = SegmentSize * (BuffStubPoolSize + NamedPipeBuffStubPoolSize);


        private static bool _initialized;
        /// <summary>
        ///     �ڴ�Ƭ������
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