using System;
using System.ServiceProcess;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers.Steps
{
    /// <summary>
    ///     检测WINDOWS SERVICE是否存在于本机的部署步骤
    /// </summary>
    public class CheckWindowServiceExistsDeployStep : DeployStep
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
            if (args == null || args.Length != 1)
            {
                throw new ArgumentException("Wrong arguments.");
            }
            string serviceName = (string) args[0];
            foreach (var service in ServiceController.GetServices())
            {
                if (service.ServiceName == serviceName)
                {
                    context = new object[] {true, serviceName};
                    return true;
                }
            }
            context = null;
            return false;
        }

        #endregion
    }
}