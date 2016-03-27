using System;
using KJFramework.Basic.Enum;
using KJFramework.Dynamic.Components;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.HostChannels;
using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Schedulers;
using KJFramework.Platform.Deploy.DSC.CP.Connector.Configurations;
using KJFramework.Platform.Deploy.DSC.CP.Connector.Nodes;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;
using KJFramework.Plugin;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector
{
    public class ConnectorComponent : DynamicDomainComponent
    {
        //for test
        private NetworkNode<DSCMessage> _node;
        private Guid _id;
        public ConnectorComponent()
        {
            _name = "DSC基础连接器功能组件";
            _pluginInfo = new PluginInfomation();
            _pluginInfo.CatalogName = "Plugins";
            _pluginInfo.Version = "0.0.0.1";
            _pluginInfo.ServiceName = "DSC.Components.Connector";
            _pluginInfo.Description = "DSC基础连接器组件，提供了与子域控制器连接的功能";
            Global.ComponentInstance = this;
            _ruleTable.Add("GetServices", args =>
                                              {
                                                  DSCGetServicesRequestMessage msg = new DSCGetServicesRequestMessage();
                                                  msg.ServiceName = "*ALL*";
                                                  msg.MachineName = Environment.MachineName;
                                                  msg.Bind();
                                                 _node.Send(_id, msg.Body);
                                                  return null;
                                              });
            _ruleTable.Add("UpdateComponent", args =>
                                                {
                                                    DSCUpdateComponentRequestMessage msg = new DSCUpdateComponentRequestMessage();
                                                    msg.ComponentName = "*ALL*";
                                                    msg.ServiceName = "Service.Test";
                                                    msg.MachineName = Environment.MachineName;
                                                    msg.Bind();
                                                    _node.Send(_id, msg.Body);
                                                    return null;
                                                });
            _ruleTable.Add("GetHealth", args =>
            {
                DSCGetComponentHealthRequestMessage msg = new DSCGetComponentHealthRequestMessage();
                msg.Components = new[] {"*ALL*"};
                msg.ServiceName = "Service.Test";
                msg.MachineName = Environment.MachineName;
                msg.Bind();
                _node.Send(_id, msg.Body);
                return null;
            });
            _ruleTable.Add("Reset", args =>
            {
                DSCResetHeartBeatTimeRequestMessage msg = new DSCResetHeartBeatTimeRequestMessage();
                msg.MachineName = Environment.MachineName;
                msg.Target = "*SMC*";
                msg.Interval = int.Parse(args[0].ToString());
                msg.Bind();
                _node.Send(_id, msg.Body);
                return null;
            });
            _ruleTable.Add("ResetS", args =>
            {
                DSCResetHeartBeatTimeRequestMessage msg = new DSCResetHeartBeatTimeRequestMessage();
                msg.MachineName = Environment.MachineName;
                msg.Target = "Others";
                msg.Interval = int.Parse(args[0].ToString());
                msg.Bind();
                _node.Send(_id, msg.Body);
                return null;
            });
            _ruleTable.Add("ResetD", args =>
            {
                DynamicServices.NotifyResetHeartbeat(int.Parse(args[0].ToString()));
                return null;
            });
            _ruleTable.Add("GetFiles", args =>
            {
                DSCGetFileInfomationRequestMessage msg = new DSCGetFileInfomationRequestMessage();
                msg.ServiceName = "Service.Test";
                msg.MachineName = Environment.MachineName;
                msg.Files = (string) args[0];
                msg.Bind();
                _node.Send(_id, msg.Body);
                return null;
            });
        }

        #region Members

        private IRequestScheduler<DSCMessage> _requestScheduler;

        #endregion

        #region Overrides of DynamicDomainComponent

        protected override void InnerStart()
        {
            int port = DSCSettingConfigSection.Current.Settings.HostPort;
            Console.WriteLine("Initializing network node......#Port: " + port);
            NetworkNode<DSCMessage> networkNode = new NetworkNode<DSCMessage>(new DSCProtocolStack());
            Console.WriteLine("Initializing protocol stack......");
            networkNode.TransportChannelRemoved += TransportChannelRemoved;
            networkNode.Regist(new TcpHostTransportChannel(port));
            Console.WriteLine("Initializing task scheduler......");
            _requestScheduler = new RequestScheduler<DSCMessage>(100);
            Console.WriteLine("Regist network node......");
            _requestScheduler.Regist(networkNode);
            Console.WriteLine("Regist function node......");
            _requestScheduler.Regist(new DSCFunctionNode());
            _requestScheduler.Start();
            Console.WriteLine("Task scheduler started......");
            Console.WriteLine("Dynamic Service Center started !");
            Global.CommnicationNode = networkNode;

            //for test
            _node = networkNode;
            _node.NewTransportChannelCreated += _node_NewTransportChannelCreated;
        }

        void TransportChannelRemoved(object sender, EventArgs.LightSingleArgEventArgs<Guid> e)
        {
            DynamicServices.UnRegist(e.Target);
        }

        void _node_NewTransportChannelCreated(object sender, EventArgs.LightSingleArgEventArgs<IRawTransportChannel> e)
        {
            _id = e.Target.Key;
        }

        protected override void InnerStop()
        {
            Console.WriteLine("Preparing to stop dynamic service center......");
            if (_requestScheduler != null)
            {
                _requestScheduler.Stop();
                _requestScheduler = null;
            }
            Console.WriteLine("Task scheduler stoped !");
            Console.WriteLine("Dynamic service center stoped!");
        }

        protected override void InnerOnLoading()
        {
            Console.WriteLine("Component : #ConnectorComponent loading......!");
        }

        protected override HealthStatus InnerCheckHealth()
        {
            return _requestScheduler != null ? HealthStatus.Good : HealthStatus.Death;
        }

        #endregion
    }
}
