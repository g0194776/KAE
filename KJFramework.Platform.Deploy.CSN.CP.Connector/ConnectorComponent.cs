using KJFramework.Basic.Enum;
using KJFramework.Dynamic.Components;
using KJFramework.IO.Helper;
using KJFramework.Logger;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.TypeProcessors.Maps;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.HostChannels;
using KJFramework.Net.ProtocolStacks;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.Identities;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Net.Transaction.Objects;
using KJFramework.Net.Transaction.Processors;
using KJFramework.Net.Transaction.Schedulers;
using KJFramework.Platform.Deploy.CSN.Common.Configurations;
using KJFramework.Platform.Deploy.CSN.Common.Datas;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Processors;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;
using KJFramework.Plugin;
using System;
using System.Configuration;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector
{
    /// <summary>
    ///     CSN基础连接器组件，用于将配置分发给订阅者
    /// </summary>
    public class ConnectorComponent : DynamicDomainComponent
    {
        #region Constructor

        /// <summary>
        ///     CSN基础连接器组件，用于将配置分发给订阅者
        /// </summary>
        public ConnectorComponent()
        {
            _name = "CSN基础连接器功能组件";
            _pluginInfo = new PluginInfomation();
            _pluginInfo.CatalogName = "Plugins";
            _pluginInfo.Version = "0.0.0.1";
            _pluginInfo.ServiceName = "CSN.Components.ConnectorComponent";
            _pluginInfo.Description = "CSN基础连接器组件，用于将配置分发给订阅者";
        }

        #endregion

        #region Members

        private IRequestScheduler _requestScheduler;
        private static MessageTransactionManager _transactionManager;
        private static IProtocolStack<BaseMessage> _protocolStack;

        #endregion

        #region Overrides of DynamicDomainComponent

        protected override void InnerStart()
        {
            Console.WriteLine("Initializing network channels......");
            TcpHostTransportChannel hostChannel = new TcpHostTransportChannel(CSNSettingConfigSection.Current.Settings.HostPort);
            bool regist = hostChannel.Regist();
            Console.WriteLine("Regist network result: " + regist);
            if (!regist) throw new System.Exception("#CSN regist network failed!");
            hostChannel.ChannelCreated += ChannelCreated;
            Console.WriteLine("Regist network node at local tcp port: " + CSNSettingConfigSection.Current.Settings.HostPort);
            Console.WriteLine("Initializing CSN protocol stack......");
            _protocolStack = new CSNProtocolStack();
            Global.ProtocolStack = (CSNProtocolStack) _protocolStack;
            Console.WriteLine("Initializing scheduler......");
            _requestScheduler = new RequestScheduler()
            .Regist(new Protocols { ProtocolId = 0, ServiceId = 2, DetailsId = 0 }, new CSNGetDataTableRequestMessageProcessor())
            .Regist(new Protocols { ProtocolId = 0, ServiceId = 3, DetailsId = 0 }, new CSNGetKeyValueItemRequestMessageProcessor());
            Console.WriteLine("Initializing database(s)......");
            InitializeDatabases();
            Console.WriteLine("CSN task scheduler started!");
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
            GlobalMemory.Initialize();
            FixedTypeManager.Add(typeof(MessageIdentity), 5);
            FixedTypeManager.Add(typeof(TransactionIdentity), 18);
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
                    ConsoleHelper.PrintLine("#Database registed. #name: " + connectionStringSettings.Name, ConsoleColor.DarkGray);
                }
                catch (System.Exception ex)
                {
                    Logs.Logger.Log(ex);
                    ConsoleHelper.PrintLine(ex.Message, ConsoleColor.DarkRed);
                }
            }
        }

        void ChannelCreated(object sender, EventArgs.LightSingleArgEventArgs<ITransportChannel> e)
        {
            IMessageTransportChannel<BaseMessage> msgChannel = new MessageTransportChannel<BaseMessage>((IRawTransportChannel)e.Target, _protocolStack);
            IServerConnectionAgent agent = new ConnectionAgent(msgChannel, _transactionManager);
            _requestScheduler.Regist(agent);
        }

        #endregion
    }
}