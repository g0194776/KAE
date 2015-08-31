using System;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Finders;
using KJFramework.ApplicationEngine.Loggers;
using KJFramework.ApplicationEngine.Managers;
using KJFramework.ApplicationEngine.Objects;
using KJFramework.ApplicationEngine.Proxies;
using KJFramework.ApplicationEngine.Resources;
using KJFramework.Messages.Contracts;
using KJFramework.Net.Identities;
using KJFramework.Results;

namespace KJFramework.ApplicationEngine.Commands
{
    /// <summary>
    ///     上架指定KPP到KAE宿主的命令
    /// </summary>
    internal class InstallKPPCommand : IKAESystemCommand
    {
        #region Members.

        /// <summary>
        ///     获取当前KAE系统命令所分配的网络信令P/S/D
        /// </summary>
        public MessageIdentity SupportedCommand
        {
            get
            {
                return new MessageIdentity {ProtocolId = 0xFF, ServiceId = 0x00, DetailsId = 0x00};
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
            if (hostedAppManager.Exists(kppUniqueId)) return ExecuteResult.Fail((byte)KAEErrorCodes.KPPAlreadyInstalled, string.Empty);
            string downloadAddress = msg.GetAttributeAsType<string>(10);
            if (string.IsNullOrEmpty(downloadAddress)) return ExecuteResult.Fail((byte)KAEErrorCodes.IllegalArgument, string.Empty);
            stateLogger.Log(string.Format("#[Installing KPP] Preparing to download targeted KPP package file in KAE hosting process {0}. #KPP Address: {1}", host.UniqueName, downloadAddress));
            IRemotingApplicationDownloader downloader = (IRemotingApplicationDownloader)KAESystemInternalResource.Factory.GetResource(KAESystemInternalResource.APPDownloader);
            IRemotingProtocolRegister protocolRegister = (IRemotingProtocolRegister)KAESystemInternalResource.Factory.GetResource(KAESystemInternalResource.ProtocolRegister);
            string downloadedFilePath = downloader.DownloadFromUrl(host.WorkRoot, downloadAddress);
            stateLogger.Log(string.Format("#[Installing KPP] Downloaded kpp from given remote accessable path: {0}", downloadAddress));
            IApplicationFinder appFinder = (IApplicationFinder)KAESystemInternalResource.Factory.GetResource(KAESystemInternalResource.APPFinder);
            stateLogger.Log(string.Format("#[Installing KPP] Extracting kpp instance from downloaded file: {0}", downloadAddress));
            Tuple<string, ApplicationEntryInfo, KPPDataStructure> kppInfo = appFinder.ReadKPPFrom(downloadedFilePath);
            ApplicationDynamicObject app = new ApplicationDynamicObject(kppInfo.Item2, kppInfo.Item3, host.ChannelInternalConfigSettings, host.ResourceProxy, host.HandleSucceedSituation, host.HandleErrorSituation);
            stateLogger.Log(string.Format("#[Installing KPP] Initialized KPP instance... #Package-Name: {0}, #Unique Id: {1}", app.PackageName, app.GlobalUniqueId));
            hostedAppManager.RegisterApp(app);
            stateLogger.Log(string.Format("#[Installing KPP] Registered KPP instance to the KAE hosting process {0}. #Package-Name {1}", host.UniqueName, app.PackageName));
            protocolRegister.Register(app);
            stateLogger.Log(string.Format("#[Installing KPP] Registered KPP protocols to the remote ZooKeeper {0}. #Package-Name {1}", host.UniqueName, app.PackageName));
            return ExecuteResult.Succeed(null);
        }

        #endregion
    }
}