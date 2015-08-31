using System;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Loggers;
using KJFramework.ApplicationEngine.Managers;
using KJFramework.ApplicationEngine.Objects;
using KJFramework.ApplicationEngine.Proxies;
using KJFramework.Messages.Contracts;
using KJFramework.Net.Identities;
using KJFramework.Results;

namespace KJFramework.ApplicationEngine.Commands
{
    /// <summary>
    ///     从一个KAE宿主中移除指定已上架的KPP实例命令
    /// </summary>
    internal class UninstallKPPCommand : IKAESystemCommand
    {
        #region Members.

        /// <summary>
        ///     获取当前KAE系统命令所分配的网络信令P/S/D
        /// </summary>
        public MessageIdentity SupportedCommand
        {
            get
            {
                return new MessageIdentity {ProtocolId = 0xFF, ServiceId = 0x00, DetailsId = 0x02};
            }
        }

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
        public IExecuteResult Execute(MetadataContainer msg, KAEHost host, IKAEHostAppManager hostedAppManager, IKAEStateLogger stateLogger)
        {
            Guid kppUniqueId = msg.GetAttributeAsType<Guid>(0x03);
            if (kppUniqueId == Guid.Empty) return ExecuteResult.Fail((byte)KAEErrorCodes.IllegalArgument, string.Empty);
            IRemotingProtocolRegister protocolRegister = (IRemotingProtocolRegister)KAESystemInternalResource.Factory.GetResource(KAESystemInternalResource.ProtocolRegister);
            ApplicationDynamicObject app = hostedAppManager.GetApp(kppUniqueId);
            if (app == null) return ExecuteResult.Fail((byte)KAEErrorCodes.SpecifiedKPPNotFound, string.Empty);
            stateLogger.Log(string.Format("#[Uninstalling KPP] Removing communication protocols to the remote ZooKeeper,  #KAE application: {0}, Internal un-rsp count: {1}", kppUniqueId, app.UnRspCount));
            protocolRegister.UnRegister(app);
            stateLogger.Log(string.Format("#[Uninstalling KPP] Try uninstalling targeted KAE application: {0}, Internal un-rsp count: {1}", kppUniqueId, app.UnRspCount));
            hostedAppManager.Remove(kppUniqueId);
            app.DelayStop();
            stateLogger.Log(string.Format("#[Uninstalling KPP] Uninstalled KAE application: {0}, Internal un-rsp count: {1}", kppUniqueId, app.UnRspCount));
            return ExecuteResult.Succeed(null);
        }

        #endregion
    }
}