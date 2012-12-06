using System;
using KJFramework.ServiceModel.Bussiness.Default.Dispatchers.Cores;
using KJFramework.ServiceModel.Bussiness.Default.Messages;
using KJFramework.ServiceModel.Bussiness.Default.Services;
using KJFramework.ServiceModel.Enums;

namespace KJFramework.ServiceModel.Bussiness.Default.Dispatchers
{
    /// <summary>
    ///     服务请求分发器，提供了相关的基本操作。
    /// </summary>
    internal class SeviceRequestDispatcher
    {
        #region Implementation of IServiceRequestDispatcher

        /// <summary>
        ///     分发请求
        ///     <para>* 此操作将会根据基础服务的类型进行特定方式的请求分发。</para>
        /// </summary>
        /// <param name="serviceHandle">服务控制器</param>
        /// <param name="message">请求</param>
        /// <param name="key">传输通道唯一标示</param>
        public static void Dispatch(ServiceHandle serviceHandle, RequestServiceMessage message, Guid key)
        {
            switch (serviceHandle.GetService().Contract.ServiceConcurrentType)
            {
                case ServiceConcurrentTypes.Concurrent:
                    using (ICoreDispatcher dispatcher = new ConcurrentCoreDispatcher())
                    {
                        dispatcher.Dispatch(serviceHandle, message, key);
                    }
                    break;
                case ServiceConcurrentTypes.Multi:
                    using (ICoreDispatcher dispatcher = new ThreadCoreDispatcher())
                    {
                        dispatcher.Dispatch(serviceHandle, message, key);
                    }
                    break;
                case ServiceConcurrentTypes.Singleton:
                    using (ICoreDispatcher dispatcher = new SingletonCoreDispatcher())
                    {
                        dispatcher.Dispatch(serviceHandle, message, key);
                    }
                    break;
            }
        }

        #endregion
    }
}