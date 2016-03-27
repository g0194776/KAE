using System;
using KJFramework.Basic.Enum;
using KJFramework.Dynamic.Components;
using KJFramework.Net.Channels.HostChannels;
using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Schedulers;
using KJFramework.Platform.Client.ProtocolStack;
using KJFramework.Platform.Deploy.DSC.CP.Client.Configurations;
using KJFramework.Platform.Deploy.DSC.CP.Connector.Nodes;
using KJFramework.Plugin;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector
{
    /// <summary>
    ///     客户端代理组件
    /// </summary>
    public class ClientProxyComponent : DynamicDomainComponent
    {
        #region Constructor

        public ClientProxyComponent()
        {
            _name = "DSC基础客户端代理功能组件";
            _pluginInfo = new PluginInfomation();
            _pluginInfo.CatalogName = "Plugins";
            _pluginInfo.Version = "0.0.0.1";
            _pluginInfo.ServiceName = "DSC.Components.ClientProxy";
            _pluginInfo.Description = "DSC基础客户端代理功能组件，提供了与客户端联通的基础能力";
            Global.ClientComponentInstance = this;
        }

        #endregion

        #region Members

        private IRequestScheduler<ClientMessage> _requestScheduler;

        #endregion

        #region Overrides of DynamicDomainComponent

        protected override void InnerStart()
        {
            Console.WriteLine("Begin initializing client proxy network node......");
            Global.ClientCommnicationNode = new NetworkNode<ClientMessage>(new ClientProtocolStack());
            Console.WriteLine("Initializing client proxy protocol stack......");
            Console.WriteLine("Regist local tcp port #" + ClientSettingConfigSection.Current.Settings.ProxyPort + "......");
            Global.ClientCommnicationNode.Regist(new TcpHostTransportChannel(ClientSettingConfigSection.Current.Settings.ProxyPort));
            Console.WriteLine("Initializing client proxy task scheduler......");
            _requestScheduler = new RequestScheduler<ClientMessage>(100);
            Console.WriteLine("Regist client proxy network node......");
            _requestScheduler.Regist(Global.ClientCommnicationNode);
            Console.WriteLine("Regist client proxy function node......");
            _requestScheduler.Regist(new ClientProxyFunctionNode());
            _requestScheduler.Start();
            Console.WriteLine("Client proxy task scheduler started!");
            Console.WriteLine("ClientProxyComponent started!");
        }

        protected override void InnerStop()
        {
            Console.WriteLine("#ClientProxyComponent stoping......");
            if (_requestScheduler != null)
            {
                _requestScheduler.Stop();
                _requestScheduler = null;
            }
            Console.WriteLine("#ClientProxyComponent stoped!");
        }

        protected override void InnerOnLoading()
        {
            Console.WriteLine("Component : #ClientProxyCompoennt loading......!");
        }

        protected override HealthStatus InnerCheckHealth()
        {
            return _requestScheduler == null ? HealthStatus.Death : HealthStatus.Good;
        }

        #endregion
    }
}
