using System;
using KJFramework.Basic.Enum;
using KJFramework.Dynamic.Components;
using KJFramework.IO.Helper;
using KJFramework.Net.Channels.HostChannels;
using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Schedulers;
using KJFramework.Platform.Deploy.CSN.Common.Configurations;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Nodes;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Subscribers;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;
using KJFramework.Plugin;

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
            Global.ComponentInstance = this;
        }

        #endregion

        #region Members

        private IRequestScheduler<CSNMessage> _requestScheduler;

        #endregion

        #region Overrides of DynamicDomainComponent

        protected override void InnerStart()
        {
            Console.WriteLine("Initializing network node......");
            Global.ClientNode = new NetworkNode<CSNMessage>(new CSNProtocolStack());
            Global.ClientNode.TransportChannelRemoved += TransportChannelRemoved;
            Console.WriteLine("Initializing CSN protocol stack......");
            Console.WriteLine("Regist network node at local tcp port: " + CSNSettingConfigSection.Current.Settings.HostPort);
            Global.ClientNode.Regist(new TcpHostTransportChannel(CSNSettingConfigSection.Current.Settings.HostPort));
            Console.WriteLine("Initializing task scheduler......");
            _requestScheduler = new RequestScheduler<CSNMessage>(100);
            _requestScheduler.Regist(Global.ClientNode);
            Console.WriteLine("Initializing function node......");
            _requestScheduler.Regist(new ConfigFunctionNode());
            _requestScheduler.Start();
            Console.WriteLine("CSN task scheduler started!");
        }

        protected override void InnerStop()
        {
            Console.WriteLine("CSN task scheduler stoping......");
            if (_requestScheduler != null)
            {
                _requestScheduler.Stop();
                _requestScheduler = null;
            }
            Console.WriteLine("CSN task scheduler stoped.");
        }

        protected override void InnerOnLoading()
        {
            Console.WriteLine("Component : #ConnectorComponent loading......!");
        }

        protected override HealthStatus InnerCheckHealth()
        {
            return _requestScheduler == null ? HealthStatus.Death : HealthStatus.Good;
        }

        #endregion

        #region Events

        //tcp channel disconnected.
        void TransportChannelRemoved(object sender, EventArgs.LightSingleArgEventArgs<Guid> e)
        {
            IConfigSubscriber subscriber = ConfigSubscriberManager.GetSubscriber(e.Target);
            if (subscriber != null)
            {
                subscriber.Cancel();
                ConsoleHelper.PrintLine("#Config subscriber disconnected! #key: " + subscriber.SubscriberKey, ConsoleColor.DarkYellow);
            }
        }

        #endregion
    }
}