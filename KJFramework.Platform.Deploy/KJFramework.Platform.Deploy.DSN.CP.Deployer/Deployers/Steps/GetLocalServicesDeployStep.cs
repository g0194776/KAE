using System.ServiceProcess;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers.Steps
{
    /// <summary>
    ///     获取本地服务相关信息部署步骤
    /// </summary>
    public class GetLocalServicesDeployStep : DeployStep
    {
        #region Overrides of DeployStep

        /// <summary>
        ///     执行步骤
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="args">相关参数</param>
        /// <returns>返回执行的结果</returns>
        protected override bool InnerExecute(out object[] context, params object[] args)
        {
            ServiceController[] services = ServiceController.GetServices();
            ServiceInfoItem[] items = new ServiceInfoItem[services.Length];
            _reporter.Notify("#Begin get local services......");
            for (int i = 0; i < services.Length; i++)
            {
                ServiceInfoItem item = new ServiceInfoItem();
                ServiceController service = services[i];
                item.ServiceName = service.ServiceName;
                item.Name = service.DisplayName;
                item.Status = service.Status;
                items[i] = item;
            }
            context = new[] { items };
            _reporter.Notify("#End get local services!");
            return true;
        }

        #endregion
    }
}