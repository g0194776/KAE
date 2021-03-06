﻿using System;
using System.Configuration;
using System.Net;
using System.Threading;
using KJFramework.Data.Synchronization;
using KJFramework.Data.Synchronization.Enums;
using KJFramework.Data.Synchronization.Factories;
using KJFramework.Dynamic;
using KJFramework.Dynamic.Components;
using KJFramework.Enums;
using KJFramework.EventArgs;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.TypeProcessors.Maps;
using KJFramework.Net;
using KJFramework.Net.Disconvery;
using KJFramework.Net.Disconvery.Protocols;
using KJFramework.Net.HostChannels;
using KJFramework.Net.Identities;
using KJFramework.Net.ProtocolStacks;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.Comparers;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Net.Transaction.Objects;
using KJFramework.Net.Transaction.Processors;
using KJFramework.Net.Transaction.Schedulers;
using KJFramework.Platform.Deploy.CSN.Common.Configurations;
using KJFramework.Platform.Deploy.CSN.Common.Datas;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Processors;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;
using KJFramework.Tracing;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector
{
    /// <summary>
    ///    CSN服务组件
    /// </summary>
    public class ConnectorComponent : DynamicDomainComponent
    {
        #region Constructor

        /// <summary>
        ///    CSN服务组件
        /// </summary>
        public ConnectorComponent()
        {
            _name = "CSN";
            _pluginInfo = new PluginInfomation();
            _pluginInfo.CatalogName = "Plugins";
            _pluginInfo.Version = "0.0.0.1";
            _pluginInfo.ServiceName = "CSN.Components.ConnectorComponent";
            _pluginInfo.Description = "CSN";
        }

        #endregion

        #region Members

        private IRequestScheduler<BaseMessage> _requestScheduler;
        private MessageTransactionManager _transactionManager;
        private IProtocolStack _protocolStack;
        private DiscoveryOnputPin _outputPin;
        private CommonBoradcastProtocol _sendObj;
        private Thread _thread;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(ConnectorComponent));
        private IDataPublisher<string, string[]> _defaultPublisher;

        #endregion

        #region Overrides of DynamicDomainComponent

        protected override void InnerStart()
        {
            Console.WriteLine("Initializing network channels......");
            TcpHostTransportChannel hostChannel = new TcpHostTransportChannel(CSNSettingConfigSection.Current.Settings.HostPort);
            bool regist = hostChannel.Regist();
            Console.WriteLine("Regist network result: " + regist);
            if (!regist) throw new Exception("#CSN regist network failed!");
            hostChannel.ChannelCreated += ChannelCreated;
            Console.WriteLine("Regist network node at local tcp port: " + CSNSettingConfigSection.Current.Settings.HostPort);
            Console.WriteLine("Openning data publisher......");
            _defaultPublisher = DataPublisherFactory.Instance.Create<string, string[]>("*", new NetworkResource(CSNSettingConfigSection.Current.Settings.UpdatingPublisher));
            if (_defaultPublisher.Open() != PublisherState.Open)
            {
                _tracing.Critical("#CSN couldn't open a defaut remoting server publisher.");
                throw new Exception("#CSN couldn't open a defaut remoting server publisher.");
            }
            Console.WriteLine("Initializing CSN protocol stack......");
            _protocolStack = new CSNProtocolStack();
            Global.ProtocolStack = (CSNProtocolStack) _protocolStack;
            _transactionManager = new MessageTransactionManager(new TransactionIdentityComparer());
            Console.WriteLine("Initializing scheduler......");
            _requestScheduler = new BaseMessageRequestScheduler()
            .Regist(new Protocols { ProtocolId = 0, ServiceId = 2, DetailsId = 0 }, new CSNGetDataTableRequestMessageProcessor())
            .Regist(new Protocols { ProtocolId = 0, ServiceId = 3, DetailsId = 0 }, new CSNGetKeyValueItemRequestMessageProcessor())
            .Regist(new Protocols { ProtocolId = 0, ServiceId = 4, DetailsId = 0 }, new CSNGetPartialConfigProcessor());
            Console.WriteLine("Initializing database(s)......");
            InitializeDatabases();
            Console.WriteLine("CSN task scheduler started!");
            CSNBoradcastStart();
            Console.WriteLine("CSN is started sucessfully!");
        }

        protected override void InnerStop()
        {
            Console.WriteLine("CSN task scheduler stoping......");
            _requestScheduler = null;
            Console.WriteLine("CSN task scheduler stoped.");
        }

        protected override void InnerOnLoading()
        {
            Console.WriteLine("Component : #ConnectorComponent loading......!");
            ChannelConst.Initialize();
            FixedTypeManager.Add(typeof(MessageIdentity), 5);
            IntellectTypeProcessorMapping.Instance.Regist(new MessageIdentityProcessor());
            IntellectTypeProcessorMapping.Instance.Regist(new TransactionIdentityProcessor());
        }

        protected override HealthStatus InnerCheckHealth()
        {
            return _requestScheduler == null ? HealthStatus.Death : HealthStatus.Good;
        }

        #endregion

        #region Methods

        private void InitializeDatabases()
        {
            //start to regist db
            for (int i = 0; i < ConfigurationManager.ConnectionStrings.Count; i++)
            {
                try
                {
                    ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[i];

                    Database database = Database.GetDatabase(connectionStringSettings.ConnectionString);
                    Global.DBCacheFactory.RegistDatabase(connectionStringSettings.Name, database);
                    Global.DBCacheFactory.Initialize();
                    Console.WriteLine("#Database registed. #name: " + connectionStringSettings.Name);
                }
                catch (Exception ex)
                {
                    _tracing.Error(ex, null);
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        ///     CSNÆô¶¯ºó»ñÈ¡¼°·¢ËÍ¹ã²¥°ü
        /// </summary>
        private void CSNBoradcastStart()
        {
            string broadcastAddress = ConfigurationManager.AppSettings["BroadcastAddress"];
            string environment = ConfigurationManager.AppSettings["Environment"];
            string localAddress = ConfigurationManager.AppSettings["LocalAddress"];
            if (string.IsNullOrEmpty(broadcastAddress) || string.IsNullOrEmpty(environment) || string.IsNullOrEmpty(localAddress))
                throw new Exception("Cannot find any BroadcastAddress or Environment or LocalAddress in CSN config file");
            int offset = broadcastAddress.LastIndexOf(':');
            string iep = broadcastAddress.Substring(0, offset);
            int port = int.Parse(broadcastAddress.Substring(offset + 1, broadcastAddress.Length - (offset + 1)));
            _outputPin = new DiscoveryOnputPin(new IPEndPoint(IPAddress.Parse(iep), port));
            _sendObj = new CommonBoradcastProtocol { Key = "CSN", Environment = environment, Value = localAddress };
            //Æô¶¯Ñ­»··¢ËÍ¹ã²¥°üµÄÏß³Ì
            _thread = new Thread(SendProc) { Name = "Thread::SendCSNInfo", IsBackground = true };
            _thread.Start();
        }

        /// <summary>
        ///     CSNÆô¶¯ºóÃ¿¸ô5S·¢ËÍCSN¹ã²¥°üµÄÏß³Ì
        /// </summary>
        private void SendProc()
        {
            while (true)
            {
                //send interval: 5s.
                _outputPin.Send(_sendObj);
                Thread.Sleep(5000);
            }
        }

        void ChannelCreated(object sender, LightSingleArgEventArgs<ITransportChannel> e)
        {
            IMessageTransportChannel<BaseMessage> msgChannel = new MessageTransportChannel<BaseMessage>((IRawTransportChannel)e.Target, _protocolStack);
            IServerConnectionAgent<BaseMessage> agent = new IntellectObjectConnectionAgent(msgChannel, _transactionManager);
            _requestScheduler.Regist(agent);
        }

        #endregion
    }
}