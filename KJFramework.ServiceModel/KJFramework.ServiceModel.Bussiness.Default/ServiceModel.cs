using KJFramework.Cache;
using KJFramework.Cache.Containers;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.TypeProcessors.Maps;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.ProtocolStacks;
using KJFramework.Net.Transaction.Processors;
using KJFramework.ServiceModel.Bussiness.Default.Messages;
using KJFramework.ServiceModel.Bussiness.Default.Objects;
using KJFramework.ServiceModel.Configurations;
using KJFramework.ServiceModel.Core.Objects;

namespace KJFramework.ServiceModel.Bussiness.Default
{
    /// <summary>
    ///     服务模型中的全局变量存放类
    /// </summary>
    internal class ServiceModel
    {
        #region 静态全局变量区

        public static ICacheTenant Tenant = new CacheTenant();
        public static IFixedCacheContainer<ServiceReturnValue> FixedReturnValues = Tenant.Rent<ServiceReturnValue>("Fixed:ServiceReturnValue", ServiceModelSettingConfigSection.Current.NetworkLayer.ServiceCallContextPoolCount);
        public static IFixedCacheContainer<ResponseServiceMessage> FixedResponseMessage = Tenant.Rent<ResponseServiceMessage>("Fixed:ResponseServiceMessage", ServiceModelSettingConfigSection.Current.NetworkLayer.ResponseServiceMessagePoolCount);
        private static bool _initialize;
        internal static IFixedCacheContainer<RequestServiceMessage> FixedRequestMessage;
        internal static IFixedCacheContainer<RequestCenterWaitObject> FixedRequestWaitObject;
        internal static IProtocolStack<Message> ProtocolStack = new ServiceModelProtocolStack();

        #endregion

        #region Methods

        /// <summary>
        ///     初始化
        /// </summary>
        public static void Initialize()
        {
            if (_initialize) return;
            FixedTypeManager.Add(typeof(MessageIdentity), 3);
            FixedTypeManager.Add(typeof(TransactionIdentity), 18);
            IntellectTypeProcessorMapping.Instance.Regist(new MessageIdentityProcessor());
            IntellectTypeProcessorMapping.Instance.Regist(new TransactionIdentityProcessor());
            _initialize = true;
        }

        #endregion
    }
}