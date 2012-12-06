using System;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Reporters;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers.Steps
{
    /// <summary>
    ///     部署步骤，提供了相关的基本操作。
    /// </summary>
    public abstract class DeployStep : IDeployStep
    {
        #region Implementation of IDeployStep

        protected System.Exception _exception;
        protected IDeployStatusReporter _reporter;

        /// <summary>
        ///     执行步骤
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="args">相关参数</param>
        /// <returns>返回执行的结果</returns>
        public bool Execute(out Object[] context, params object[] args)
        {
            try
            {
                return InnerExecute(out context, args);
            }
            catch (System.Exception ex)
            {
                _exception = ex;
                context = null;
                return false;
            }
        }

        /// <summary>
        ///     获取执行步骤当中出现的异常
        /// </summary>
        public System.Exception Exception
        {
            get { return _exception; }
        }

        /// <summary>
        ///     获取或设置部署状态汇报器
        ///     <para>* 此属性会由部署者统一注入</para>
        /// </summary>
        public IDeployStatusReporter Reporter
        {
            get { return _reporter; }
            set { _reporter = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     执行步骤
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="args">相关参数</param>
        /// <returns>返回执行的结果</returns>
        protected abstract bool InnerExecute(out Object[] context, params object[] args);

        #endregion
    }
}