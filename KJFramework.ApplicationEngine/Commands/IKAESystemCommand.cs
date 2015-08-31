using KJFramework.ApplicationEngine.Loggers;
using KJFramework.ApplicationEngine.Managers;
using KJFramework.Messages.Contracts;
using KJFramework.Net.Identities;
using KJFramework.Results;

namespace KJFramework.ApplicationEngine.Commands
{
    /// <summary>
    ///     KAE系统命令接口
    /// </summary>
    internal interface IKAESystemCommand
    {
        #region Members.

        /// <summary>
        ///     获取当前KAE系统命令所分配的网络信令P/S/D
        /// </summary>
        MessageIdentity SupportedCommand { get; }

        #endregion

        #region Methods.

        /// <summary>
        ///     执行一个KAE系统命令
        /// </summary>
        /// <param name="msg">执行命令的请求消息</param>
        /// <param name="host">被执行命令的KAE宿主实例</param>
        /// <param name="hostedAppManager">KAE宿主实例内部所包含的APP实例管理器</param>
        /// <param name="stateLogger">KAE宿主状态记录器实例</param>
        /// <returns>返回操作的结果</returns>
        IExecuteResult Execute(MetadataContainer msg, KAEHost host, IKAEHostAppManager hostedAppManager, IKAEStateLogger stateLogger);

        #endregion
    }
}