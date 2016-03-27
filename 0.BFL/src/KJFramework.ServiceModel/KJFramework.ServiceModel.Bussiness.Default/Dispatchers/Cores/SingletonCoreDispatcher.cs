using KJFramework.ServiceModel.Bussiness.Default.Services;
using KJFramework.ServiceModel.Core.Objects;

namespace KJFramework.ServiceModel.Bussiness.Default.Dispatchers.Cores
{
    /// <summary>
    ///     ���ڵ�һʵ��ģʽ�ĺ��ķַ������ṩ����صĻ���������
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