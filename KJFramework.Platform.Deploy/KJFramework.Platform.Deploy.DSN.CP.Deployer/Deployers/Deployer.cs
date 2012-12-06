using System;
using System.Collections.Generic;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers.Steps;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Packages;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Reporters;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers
{
    /// <summary>
    ///     部署者抽象父类，提供了相关的基本操作
    /// </summary>
    internal abstract class Deployer : IDeployer
    {
        #region Constructor

        /// <summary>
        ///     部署者抽象父类，提供了相关的基本操作
        /// </summary>
        /// <param name="requestToken">请求令牌</param>
        /// <param name="package">文件包</param>
        /// <param name="channelId">相关联的通道编号</param>
        protected Deployer(string requestToken, IFilePackage package, Guid channelId)
            : this(requestToken, package, channelId, true)
        {
        }


        /// <summary>
        ///     部署者抽象父类，提供了相关的基本操作
        /// </summary>
        /// <param name="requestToken">请求令牌</param>
        /// <param name="package">文件包</param>
        /// <param name="channelId">相关联的通道编号</param>
        /// <param name="reportDetail">汇报状态表示</param>
        protected Deployer(string requestToken, IFilePackage package, Guid channelId, bool reportDetail)
        {
            if (string.IsNullOrEmpty(requestToken))
            {
                throw new ArgumentNullException(requestToken);
            }
            _requestToken = requestToken;
            _package = package;
            _reportDetail = reportDetail;
            _reporter = new DeployStatusReporter(channelId, requestToken, _reportDetail);
        }

        #endregion

        #region Implementation of IDeployer

        protected IFilePackage _package;
        protected readonly bool _reportDetail;
        protected string _requestToken;
        protected System.Exception _lastException;
        protected List<IDeployStep> _steps = new List<IDeployStep>();
        protected IDeployStatusReporter _reporter;

        /// <summary>
        ///     获取请求令牌
        /// </summary>
        public string RequestToken
        {
            get { return _requestToken; }
        }

        /// <summary>
        ///     获取状态汇报器
        /// </summary>
        public IDeployStatusReporter Reporter
        {
            get { return _reporter; }
        }

        /// <summary>
        ///     增加一个部署步骤
        /// </summary>
        /// <param name="deployStep">部署步骤</param>
        public void Add(IDeployStep deployStep)
        {
            if (deployStep == null)
            {
                throw new ArgumentNullException("deployStep");
            }
            deployStep.Reporter = _reporter;
            _steps.Add(deployStep);
        }

        /// <summary>
        ///     部署
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>返回部署的结果</returns>
        public abstract bool Deploy(params object[] args);

        /// <summary>
        ///     获取部署中出现的最后一个异常
        /// </summary>
        public System.Exception LastException
        {
            get { return _lastException; }
        }

        #endregion
    }
}