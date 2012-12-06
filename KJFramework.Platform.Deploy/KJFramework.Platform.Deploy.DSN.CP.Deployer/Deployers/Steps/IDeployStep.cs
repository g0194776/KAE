using System;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Reporters;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers.Steps
{
    /// <summary>
    ///     部署步骤元接口，提供了相关的基本操作。
    /// </summary>
    public interface IDeployStep
    {
        /// <summary>
        ///     执行步骤
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="args">相关参数</param>
        /// <returns>返回执行的结果</returns>
        bool Execute(out Object[] context, params Object[] args);
        /// <summary>
        ///     获取执行步骤当中出现的异常
        /// </summary>
        System.Exception Exception { get; }
        /// <summary>
        ///     获取或设置部署状态汇报器
        ///     <para>* 此属性会由部署者统一注入</para>
        /// </summary>
        IDeployStatusReporter Reporter { get; set; }
    }
}