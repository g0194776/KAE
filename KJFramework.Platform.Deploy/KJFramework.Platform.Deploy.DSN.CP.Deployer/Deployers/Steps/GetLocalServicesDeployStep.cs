using System.ServiceProcess;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers.Steps
{
    /// <summary>
    ///     ��ȡ���ط��������Ϣ������
    /// </summary>
    public class GetLocalServicesDeployStep : DeployStep
    {
        #region Overrides of DeployStep

        /// <summary>
        ///     ִ�в���
        /// </summary>
        /// <param name="context">������</param>
        /// <param name="args">��ز���</param>
        /// <returns>����ִ�еĽ��</returns>
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