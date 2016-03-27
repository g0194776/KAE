using KJFramework.ServiceModel.Bussiness.Default.Services;
using KJFramework.ServiceModel.Core.Objects;

namespace KJFramework.ServiceModel.Bussiness.Default.Dispatchers.Cores
{
    /// <summary>
    ///     基于单一实例模式的核心分发器，提供了相关的基本操作。
    /// </summary>
    internal class SingletonCoreDispatcher : CoreDispatcher
    {
        #region Overrides of CoreDispatcher

        protected override void InitServiceMethodPickupObject(ServiceMethodPickupObject instance, IHostService hostService)
        {
            instance.Method.Instance = hostService.GetServiceObject();
        }

        #endregion
    }
}