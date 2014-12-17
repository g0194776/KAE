using KJFramework.ApplicationEngine.Configurations.Settings;
using KJFramework.ApplicationEngine.Proxies;
using KJFramework.ApplicationEngine.RRCS.Componnets.BasicComponent.Helpers;
using KJFramework.ApplicationEngine.RRCS.Componnets.BasicComponent.Processors;
using KJFramework.Datas;
using KJFramework.Dynamic.Components;
using KJFramework.Enums;
using KJFramework.EventArgs;
using KJFramework.Messages.Contracts;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.HostChannels;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.Comparers;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.Objects;
using KJFramework.Net.Transaction.ProtocolStack;
using KJFramework.Net.Transaction.Schedulers;
using System;
using System.Configuration;
using KJFramework.Tracing;

namespace KJFramework.ApplicationEngine.RRCS.Componnets.BasicComponent
{
    public class BasicComponent : DynamicDomainComponent
    {
        #region Members.

        private IHostTransportChannel _hostChannel;
        private readonly MetadataProtocolStack _protocolStack = new MetadataProtocolStack();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (BasicComponent));
        private readonly MetadataMessageRequestScheduler _scheduler = new MetadataMessageRequestScheduler();
        private readonly MetadataTransactionManager _transactionManager = new MetadataTransactionManager(new TransactionIdentityComparer());

        #endregion

        #region Methods.

        protected override void InnerStart()
        {
            _hostChannel = new TcpHostTransportChannel(int.Parse(ConfigurationManager.AppSettings["Port"]));
            _hostChannel.ChannelCreated += ChannelCreated;
            if (!_hostChannel.Regist()) throw new System.Exception("#RRCS couldn't open TCP network resource. #Port: " + ConfigurationManager.AppSettings["Port"]);
            RemotingServerManager.Initialize();
            Console.WriteLine("RRCS Started.");
        }

        protected override void InnerStop()
        {
            if (_hostChannel != null)
            {
                _hostChannel.ChannelCreated -= ChannelCreated;
                _hostChannel.UnRegist();
                _hostChannel = null;
            }
            Console.WriteLine("RRCS Stopped.");
        }

        protected override void InnerOnLoading()
        {
            //initializes something before actual business starting.
            SystemWorker.Initialize("RRCS", RemoteConfigurationSetting.Default, new SolitaryRemoteConfigurationProxy());
            string connectionStr = SystemWorker.ConfigurationProxy.GetField("APMS", "DatabaseConnection");
            ApplicationHelper.Initialize(new Database(connectionStr, 120));
            _scheduler.Regist(new Protocols {ProtocolId = 0xFC, ServiceId = 0, DetailsId = 0}, new RegisterProcessor());
        }

        protected override HealthStatus InnerCheckHealth()
        {
            return HealthStatus.Good;
        }

        #endregion

        #region Events.

        void ChannelCreated(object sender, LightSingleArgEventArgs<ITransportChannel> e)
        {
            ITransportChannel channel = e.Target;
            IMessageTransportChannel<MetadataContainer> msgChannel = new MessageTransportChannel<MetadataContainer>((IRawTransportChannel) channel, _protocolStack);
            MetadataConnectionAgent agent = new MetadataConnectionAgent(msgChannel, _transactionManager);
            _scheduler.Regist(agent);
        }

        #endregion
    }
}
