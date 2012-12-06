using System;
using KJFramework.Logger;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers.Steps;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Packages;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers
{
    /// <summary>
    ///     ��̬��������
    /// </summary>
    internal class DynamicServiceDeployer : Deployer
    {
        #region Constructor

        /// <summary>
        ///     �����߳����࣬�ṩ����صĻ�������
        /// </summary>
        /// <param name="requestToken">��������</param>
        /// <param name="package">�ļ���</param>
        /// <param name="channelId">�������ͨ�����</param>
        public DynamicServiceDeployer(string requestToken, IFilePackage package, Guid channelId)
            : base(requestToken, package, channelId)
        {
        }

        /// <summary>
        ///     �����߳����࣬�ṩ����صĻ�������
        /// </summary>
        /// <param name="requestToken">��������</param>
        /// <param name="package">�ļ���</param>
        /// <param name="channelId">�������ͨ�����</param>
        /// <param name="reportDetail">�㱨״̬��ʾ</param>
        public DynamicServiceDeployer(string requestToken, IFilePackage package, Guid channelId, bool reportDetail)
            : base(requestToken, package, channelId, reportDetail)
        {
        }

        #endregion

        #region Overrides of Deployer

        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="args">����</param>
        /// <returns>���ز���Ľ��</returns>
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