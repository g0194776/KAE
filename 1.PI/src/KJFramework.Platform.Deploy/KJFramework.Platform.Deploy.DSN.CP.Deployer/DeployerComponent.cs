using System;
using KJFramework.Basic.Enum;
using KJFramework.Dynamic.Components;
using KJFramework.IO.Helper;
using KJFramework.Net.Channels.HostChannels;
using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Schedulers;
using KJFramework.Platform.Deploy.DSN.Common.Configurations;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Nodes;
using KJFramework.Platform.Deploy.DSN.ProtocolStack;
using KJFramework.Plugin;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer
{
    public class DeployerComponent : DynamicDomainComponent
    {
        #region Constructor

        public DeployerComponent()
        {
            _name = "DSN服务部署功能组件";
            _pluginInfo = new PluginInfomation();
            _pluginInfo.CatalogName = "Plugins";
            _pluginInfo.Version = "0.0.0.1";
            _pluginInfo.ServiceName = "DSN.Components.Deployer";
            _pluginInfo.Description = "DSN服务部署功能组件，提供了动态部署服务的相关能力";
        }

        #endregion

        #region Members

        private IRequestScheduler<DSNMessage> _requestScheduler;

        #endregion

        #region Overrides of DynamicDomainComponent

        protected override void InnerStart()
        {
            int port = DSNSettingConfigSection.Current.Settings.HostPort;
            if (port <= 0 || port > 65535)
            {
                string message = "#WARNING: Illegal local port: " + port + ", this program will be close !";
                ConsoleHelper.PrintLine(message, ConsoleColor.DarkRed);
                throw new System.Exception(message);
            }
            NetworkNode<DSNMessage> networkNode = new NetworkNode<DSNMessage>(new DSNProtocolStack());
            Global.CommunicationNode = networkNode;
            networkNode.ProtocolStack.Initialize();
            networkNode.Regist(new TcpHostTransportChannel(port));
            ConsoleHelper.PrintLine("Regist local tcp port #" + port + "......Done!", ConsoleColor.DarkGreen);
            Console.WriteLine("Preparing to initialize task scheduler......");
            _requestScheduler = new RequestScheduler<DSNMessage>();
            Console.WriteLine("Regist network node......");
            _requestScheduler.Regist(networkNode);
            Console.WriteLine("Regist function node......");
            _requestScheduler.Regist(new DeployerFunctionNode());
            _requestScheduler.Start();
            Console.WriteLine("Task scheduler started!");

        }

        protected override void InnerStop()
        {
            Console.WriteLine("Stoping deploy service node......");
            if (_requestScheduler != null)
            {
                _requestScheduler.Stop();
                _requestScheduler = null;
            }
        }

        protected override void InnerOnLoading()
        {
            Console.WriteLine("Component : #DeployerComponent loading......!");
        }

        protected override HealthStatus InnerCheckHealth()
        {
            return _requestScheduler == null ? HealthStatus.Death : HealthStatus.Good;
        }

        #endregion
    }
}
