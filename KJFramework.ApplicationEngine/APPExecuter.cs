using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using KJFramework.ApplicationEngine.Commands;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Finders;
using KJFramework.ApplicationEngine.Loggers;
using KJFramework.ApplicationEngine.Managers;
using KJFramework.ApplicationEngine.Objects;
using KJFramework.ApplicationEngine.Proxies;
using KJFramework.ApplicationEngine.Resources;
using KJFramework.Counters;
using KJFramework.EventArgs;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.ValueStored;
using KJFramework.Net;
using KJFramework.Net.Configurations;
using KJFramework.Net.Identities;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.ValueStored;
using KJFramework.Results;
using KJFramework.Tracing;

namespace KJFramework.ApplicationEngine
{
    /// <summary>
    ///     APP执行器
    /// </summary>
    internal static class APPExecuter
    {
        #region Members.

        public static Application Application;
        private static readonly object _commandExecLockObj = new object();
        private static readonly IKAEStateLogger _stateLogger = new KAEStateLogger(_tracing);
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(APPExecuter));

        #region Performance Counters.

        private static readonly LightPerfCounter _errorRspCounter = new NumberOfItems64PerfCounter("KAE::COMMUNICATION::RSP::ERROR", "It used for counting how many RSP messages had occured error.");
        private static readonly LightPerfCounter _rspRemainningCounter = new NumberOfItems64PerfCounter("KAE::COMMUNICATION::RSP::REMAINNING", "It used for counting how many RSP messages are waitting for sends to the remoting network resource.");

        #endregion

        #endregion

        #region Methods.

        /// <summary>
        ///     初始化
        /// </summary>
        public static void Initialize()
        {
            _tracing.DebugInfo("#Initializing KAE internal system resource factory...");
            KAESystemInternalResource.Factory.Initialize();
            _tracing.DebugInfo("\t#Initializing network resources...");
            if (!KAEHostNetworkResourceManager.IsInitialized)
            {
                KAEHostNetworkResourceManager.MetadataNewTransaction += MetadataNewTransaction;
                KAEHostNetworkResourceManager.Initialize();
            }
        }

        /// <summary>
        ///     运行一个用的定指户KEA APP
        /// </summary>
        /// <param name="etcdClusterUrl">远程的ETCD集群地址</param>
        /// <param name="appFilePath">目标待行执的APP全路径</param>
        public static void Run(string etcdClusterUrl, string appFilePath)
        {
            if (string.IsNullOrEmpty(etcdClusterUrl)) throw new ArgumentNullException("etcdClusterUrl");
            if (string.IsNullOrEmpty(appFilePath)) throw new ArgumentNullException("appFilePath");
            string appRealPath = string.Empty;
            if (appFilePath.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase))
                appRealPath = DownloadAPP(appFilePath);
            else appRealPath = appFilePath;
            _tracing.DebugInfo("\t#Loading KPPs...");
            Tuple<string, ApplicationEntryInfo, KPPDataStructure> tuple = ((IApplicationFinder)KAESystemInternalResource.Factory.GetResource(KAESystemInternalResource.APPFinder)).ReadKPPFrom(appRealPath);
            if (tuple == null)
            {
                _tracing.DebugInfo("\t#An unhandled exception occured during unzipping the KPP file you specified! {0}", ConsoleColor.DarkRed, appRealPath);
                return;
            }
            //does a copy of current AppDomain's global network layer settings for each of installing KPP.
            ChannelInternalConfigSettings settings = new ChannelInternalConfigSettings
            {
                BuffStubPoolSize = ChannelConst.BuffStubPoolSize,
                MaxMessageDataLength = ChannelConst.MaxMessageDataLength,
                NamedPipeBuffStubPoolSize = ChannelConst.NamedPipeBuffStubPoolSize,
                NoBuffStubPoolSize = ChannelConst.NoBuffStubPoolSize,
                RecvBufferSize = ChannelConst.RecvBufferSize,
                SegmentSize = ChannelConst.SegmentSize
            };
            Application = new Application(HandleSucceedSituation, HandleErrorSituation);
            Application.Initialize(tuple.Item3, settings, new KAEHostResourceProxy());
        }

        private static string DownloadAPP(string remoteAPPPath)
        {
            string defaultDownloadPath = string.Empty;
            try
            {
                string tmpPath = SystemWorker.ConfigurationProxy.GetField("kae-system", "default-app-download-storage-path");
                defaultDownloadPath = tmpPath;
                if(!Directory.Exists(defaultDownloadPath)) throw new DirectoryNotFoundException(defaultDownloadPath);
            }
            catch (KeyNotFoundException) { defaultDownloadPath = "."; }
            string destAPPPath = Path.Combine(defaultDownloadPath, Path.GetFileName(remoteAPPPath));
            using (WebClient client = new WebClient())
                client.DownloadFile(remoteAPPPath, destAPPPath);
            return destAPPPath;
        }

        internal static void HandleErrorSituation(MetadataMessageTransaction transaction, KAEErrorCodes errorCode, string reason)
        {
            _rspRemainningCounter.Decrement();
            _errorRspCounter.Increment();
            MessageIdentity msgIdentity = transaction.Request.GetAttributeAsType<MessageIdentity>(0x00);
            msgIdentity.DetailsId += 1;
            MetadataContainer rspMsg = new MetadataContainer();
            rspMsg.SetAttribute(0x00, new MessageIdentityValueStored(msgIdentity));
            rspMsg.SetAttribute(0x0A, new ByteValueStored((byte)errorCode));
            rspMsg.SetAttribute(0x0B, new StringValueStored(reason ?? string.Empty));
            transaction.SendResponse(rspMsg);
        }

        internal static void HandleSucceedSituation(MetadataMessageTransaction transaction, MetadataContainer rspMessage)
        {
            _rspRemainningCounter.Decrement();
            MessageIdentity msgIdentity = transaction.Request.GetAttributeAsType<MessageIdentity>(0x00);
            msgIdentity.DetailsId += 1;
            rspMessage.SetAttribute(0x00, new MessageIdentityValueStored(msgIdentity));
            transaction.SendResponse(rspMessage);
        }

        private static void HandleSystemCommand(IMessageTransaction<MetadataContainer> transaction)
        {
            //KAE hosting will ensures that it always executes ONLY ONE command each time.
            lock (_commandExecLockObj)
            {
                MetadataContainer request = transaction.Request;
                IExecuteResult result = KAECommandsExector.Execute(request, _stateLogger);
                MetadataContainer rspMsg = new MetadataContainer();
                rspMsg.SetAttribute(0x0A, new ByteValueStored(result.ErrorId));
                if (result.ErrorId != (byte)KAEErrorCodes.OK)
                    rspMsg.SetAttribute(0x0B, new StringValueStored(result.GetResult<string>()));
                string retValue = result.GetResult<string>();
                if (!string.IsNullOrEmpty(retValue))
                    rspMsg.SetAttribute(0x0C, new StringValueStored(retValue));
                transaction.SendResponse(rspMsg);
            }
        }

        /*
         *  处理外部请求的总入口
         *  
         *  [RSP MESSAGE]
         *  ===========================================
         *      0x00 - Message Identity
         *      0x01 - Transaction Identity
         *      0x02 - Requested Targeting APP Level (REQUIRED)
         *      ...
         *      Other Business Fields
         */
        private static void HandleBusiness(Tuple<KAENetworkResource, ApplicationLevel> tag, MetadataMessageTransaction transaction, MessageIdentity reqMsgIdentity, object reqMsg, Guid kppUniqueId, TransactionIdentity transactionIdentity)
        {
            _rspRemainningCounter.Increment();
            if (Application.GlobalUniqueId != kppUniqueId)
            {
                HandleErrorSituation(transaction, KAEErrorCodes.SpecifiedKPPNotFound, "#Specified KPP's unique ID had not found!");
                return;
            }
            Application.HandleBusiness(tag, transaction, reqMsgIdentity, reqMsg, transactionIdentity);
        }

        #endregion

        #region Events.

        static void MetadataNewTransaction(object sender, LightSingleArgEventArgs<IMessageTransaction<MetadataContainer>> e)
        {
            MetadataConnectionAgent agent = (MetadataConnectionAgent)sender;
            IMessageTransaction<MetadataContainer> transaction = e.Target;
            MetadataContainer reqMsg = transaction.Request;
            Tuple<KAENetworkResource, ApplicationLevel> tag = new Tuple<KAENetworkResource, ApplicationLevel>((KAENetworkResource)agent.Tag, (reqMsg.IsAttibuteExsits(0x05) ? (ApplicationLevel)reqMsg.GetAttributeAsType<byte>(0x05) : ApplicationLevel.Stable));
            MessageIdentity reqMsgIdentity = reqMsg.GetAttributeAsType<MessageIdentity>(0x00);
            TransactionIdentity transactionIdentity = reqMsg.GetAttributeAsType<TransactionIdentity>(0x01);
            Guid uniqueId = reqMsg.GetAttributeAsType<Guid>(0x03);
            /*
             * We always makes a checking on the Metadata protocol network communication. 
             * Because all of ours internal system communications are constructed by this kind of MSG protocol.
             */
            if (reqMsgIdentity.ProtocolId >= 0xFC) HandleSystemCommand(transaction);
            //sends it to the appropriate application.
            else HandleBusiness(tag, (MetadataMessageTransaction) transaction, reqMsgIdentity, reqMsg, uniqueId, transactionIdentity);
        }

        #endregion
    }
}