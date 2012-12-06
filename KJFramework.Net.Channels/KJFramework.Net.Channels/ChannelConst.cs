using KJFramework.Cache;
using KJFramework.Cache.Containers;
using KJFramework.Net.Channels.Caches;
using KJFramework.Net.Channels.Configurations;

namespace KJFramework.Net.Channels
{
    internal static class ChannelConst
    {
        /// <summary>
        ///     ����ͨ����������С
        /// </summary>
        public static int RecvBufferSize = ChannelModelSettingConfigSection.Current.Settings == null
                                               ? 4096
                                               : ChannelModelSettingConfigSection.Current.Settings.RecvBufferSize;
        /// <summary>
        ///     �ײ�SocketAsyncEventArgs�������
        ///     <para>* �����ͻ��潫������ڴ滺����</para>
        /// </summary>
        public static int BuffStubPoolSize = ChannelModelSettingConfigSection.Current.Settings == null
                                               ? 200000
                                               : ChannelModelSettingConfigSection.Current.Settings.BuffStubPoolSize;
        /// <summary>
        ///     �ײ�SocketAsyncEventArgs�������
        /// </summary>
        public static int NoBuffStubPoolSize = ChannelModelSettingConfigSection.Current.Settings == null
                                               ? 200000
                                               : ChannelModelSettingConfigSection.Current.Settings.NoBuffStubPoolSize;
        /// <summary>
        ///     ���ֶ������ж�һ����Ϣ�Ƿ���Ҫ�ְ�����
        /// </summary>
        public static int MaxMessageDataLength = ChannelModelSettingConfigSection.Current.Settings == null
                                               ? 5120
                                               : ChannelModelSettingConfigSection.Current.Settings.MaxMessageDataLength;
        public static ICacheTenant Tenant = new CacheTenant();
        public static IFixedCacheContainer<BuffSocketStub> BuffAsyncStubPool = Tenant.Rent<BuffSocketStub>("Pool::BuffSocketIOStub", BuffStubPoolSize);
        public static IFixedCacheContainer<NoBuffSocketStub> NoBuffAsyncStubPool = Tenant.Rent<NoBuffSocketStub>("Pool::NoBuffSocketIOStub", NoBuffStubPoolSize);
    }
}