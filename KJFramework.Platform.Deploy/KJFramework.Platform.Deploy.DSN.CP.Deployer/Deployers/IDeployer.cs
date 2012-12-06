using KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers.Steps;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Reporters;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers
{
    /// <summary>
    ///     部署者元接口，提供了相关的基本操作。
    /// </summary>
    public interface IDeployer
    {
        /// <summary>
        ///     获取请求令牌
        /// </summary>
        string RequestToken { get; }
        /// <summary>
        ///     获取状态汇报器
        /// </summary>
        IDeployStatusReporter Reporter { get; }
        /// <summary>
        ///     增加一个部署步骤
        /// </summary>
        /// <param name="deployStep">部署步骤</param>
        void Add(IDeployStep deployStep);
        /// <summary>
        ///     部署
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>返回部署的结果</returns>
        bool Deploy(params object[] args);
        /// <summary>
        ///     获取部署中出现的最后一个异常
        /// </summary>
        System.Exception LastException { get; }
    }
}