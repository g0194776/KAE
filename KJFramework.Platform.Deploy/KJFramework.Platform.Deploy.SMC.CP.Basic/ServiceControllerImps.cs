using System;
using System.ServiceProcess;
using KJFramework.Platform.Deploy.Metadata.Enums;
using KJFramework.Platform.Deploy.SMC.Contracts;
using KJFramework.Services;

namespace KJFramework.Platform.Deploy.SMC.CP.Basic
{
    public class ServiceControllerImps : IServiceControllerContract
    {
        #region Implementation of IServiceControllerContract

        /// <summary>
        ///     开启服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns>返回服务状态</returns>
        public ServiceStatus Open(string serviceName)
        {
            if (String.IsNullOrEmpty(serviceName))
            {
                throw new ArgumentNullException("serviceName");
            }
            WindowsService windowsService = new WindowsService(serviceName);
            return windowsService.Start() ? ServiceStatus.Opend : ServiceStatus.Closed;
        }

        /// <summary>
        ///     关闭服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns>返回服务状态</returns>
        public ServiceStatus Close(string serviceName)
        {
            if (String.IsNullOrEmpty(serviceName))
            {
                throw new ArgumentNullException("serviceName");
            }
            WindowsService windowsService = new WindowsService(serviceName);
            return windowsService.Stop() ? ServiceStatus.Closed : ServiceStatus.Unknown;
        }

        /// <summary>
        ///     暂停服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns>返回服务状态</returns>
        public ServiceStatus Pause(string serviceName)
        {
            if (String.IsNullOrEmpty(serviceName))
            {
                throw new ArgumentNullException("serviceName");
            }
            ServiceController[] serviceController = ServiceController.GetServices();
            for (int i = 0; i < serviceController.Length; i++)
            {
                if (serviceController[i].ServiceName == serviceName)
                {
                    serviceController[i].Pause();
                    serviceController[i].Refresh();
                    return serviceController[i].Status == ServiceControllerStatus.Paused? ServiceStatus.Paused : ServiceStatus.Pausing;
                }
            }
            return ServiceStatus.Unknown;
        }

        /// <summary>
        ///     查询服务状态
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns>返回服务状态</returns>
        public ServiceStatus GetStatus(string serviceName)
        {
            if (String.IsNullOrEmpty(serviceName))
            {
                throw new ArgumentNullException("serviceName");
            }
            ServiceController[] serviceController = ServiceController.GetServices();
            for (int i = 0; i < serviceController.Length; i++)
            {
                if (serviceController[i].ServiceName == serviceName)
                {
                    ServiceController controller = serviceController[i];
                    if (controller.Status == ServiceControllerStatus.StartPending)
                    {
                        return ServiceStatus.Opening;
                    }
                    if (controller.Status == ServiceControllerStatus.Running)
                    {
                        return ServiceStatus.Opend;
                    }
                    if (controller.Status == ServiceControllerStatus.PausePending)
                    {
                        return ServiceStatus.Pausing;
                    }
                    if (controller.Status == ServiceControllerStatus.Paused)
                    {
                        return ServiceStatus.Paused;
                    }
                    if (controller.Status == ServiceControllerStatus.Stopped)
                    {
                        return ServiceStatus.Opend;
                    }
                    if (controller.Status == ServiceControllerStatus.StopPending)
                    {
                        return ServiceStatus.Closing;
                    }
                }
            }
            return ServiceStatus.Unknown;
        }

        #endregion
    }
}