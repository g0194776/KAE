using System;
using KJFramework.Logger;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers.Steps;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Packages;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers
{
    /// <summary>
    ///     动态服务部署者
    /// </summary>
    internal class DynamicServiceDeployer : Deployer
    {
        #region Constructor

        /// <summary>
        ///     部署者抽象父类，提供了相关的基本操作
        /// </summary>
        /// <param name="requestToken">请求令牌</param>
        /// <param name="package">文件包</param>
        /// <param name="channelId">相关联的通道编号</param>
        public DynamicServiceDeployer(string requestToken, IFilePackage package, Guid channelId)
            : base(requestToken, package, channelId)
        {
        }

        /// <summary>
        ///     部署者抽象父类，提供了相关的基本操作
        /// </summary>
        /// <param name="requestToken">请求令牌</param>
        /// <param name="package">文件包</param>
        /// <param name="channelId">相关联的通道编号</param>
        /// <param name="reportDetail">汇报状态表示</param>
        public DynamicServiceDeployer(string requestToken, IFilePackage package, Guid channelId, bool reportDetail)
            : base(requestToken, package, channelId, reportDetail)
        {
        }

        #endregion

        #region Overrides of Deployer

        /// <summary>
        ///     部署
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>返回部署的结果</returns>
        public override bool Deploy(params object[] args)
        {
            object[] context = null;
            bool executeResult = false;
            for (int i = 0; i < _steps.Count; i++)
            {
                IDeployStep deployStep = _steps[i];
                executeResult = deployStep.Execute(out context, i == 0 ? args : context);
                //Execute failed.
                if (!executeResult)
                {
                    Logs.Logger.Log("Deploy step execute failed, when processing step: " + deployStep);
                    if (deployStep.Exception != null)
                    {
                        Logs.Logger.Log("Exception details below......");
                        Logs.Logger.Log(deployStep.Exception);
                    }
                    return false;
                }
            }
            return executeResult;
        }

        #endregion
    }
}