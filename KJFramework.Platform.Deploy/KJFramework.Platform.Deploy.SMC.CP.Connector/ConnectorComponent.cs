using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using KJFramework.Basic.Enum;
using KJFramework.Dynamic.Components;
using KJFramework.Dynamic.Visitors;
using KJFramework.EventArgs;
using KJFramework.IO.Helper;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.HostChannels;
using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Schedulers;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;
using KJFramework.Platform.Deploy.SMC.Common.Performances;
using KJFramework.Platform.Deploy.SMC.CP.Connector.Configurations;
using KJFramework.Platform.Deploy.SMC.CP.Connector.Nodes;
using KJFramework.Platform.Deploy.SMC.ProtocolStack.Services;
using KJFramework.Plugin;
using KJFramework.Timer;
using SettingConfiguration = KJFramework.Platform.Deploy.SMC.CP.Connector.Configurations.SettingConfiguration;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector
{
    /// <summary>
    ///     SMC基础连接器组件
    /// </summary>
    public class ConnectorComponent : DynamicDomainComponent
    {
        #region Constructor

        /// <summary>
        ///     SMC基础连接器组件
        /// </summary>
        public ConnectorComponent()
        {
            _name = "SMC基础连接器功能组件";
            _pluginInfo = new PluginInfomation();
            _pluginInfo.CatalogName = "Plugins";
            _pluginInfo.Version = "0.0.0.1";
            _pluginInfo.ServiceName = "SMC.Components.Connector";
            _pluginInfo.Description = "SMC基础连接器组件，提供了与动态服务和服务中心连接的功能";
            Global.ConnectorInstance = this;
        }

        #endregion

        #region Members

        private RequestScheduler<DynamicServiceMessage> _requestScheduler;
        private RequestScheduler<DSCMessage> _centerScheduler;
        private IPEndPoint _centerIEP;
        private LightTimer _connectTimer;
        private LightTimer _heartbeatTimer;
        internal AutoResetEvent RegistSignal;

        #endregion

        #region Overrides of DynamicDomainComponent

        protected override void InnerStart()
        {
            Console.WriteLine("Reading configurations......");
            SettingConfiguration settingConfiguration = SMCSettingConfigSection.Current.Settings;
            Global.CenterNetworkNode = new NetworkNode<DSCMessage>(new DSCProtocolStack());
            Global.CenterNetworkNode.ProtocolStack.Initialize();
            Global.CenterNetworkNode.TransportChannelRemoved += CenterTransportChannelRemoved;
            if (string.IsNullOrEmpty(settingConfiguration.CenterAddress))
            {
                ConsoleHelper.PrintLine("#WARING:\r\nNo config item #CenterUrl, 'ConnectorComponent' will be run at uncontrollable platform !", ConsoleColor.Yellow);
            }
            else
            {
                Console.WriteLine("Preparing connect to dynamic service center......");
                _centerScheduler = new RequestScheduler<DSCMessage>(100);
                _centerScheduler.Regist(Global.CenterNetworkNode);
                _centerScheduler.Regist(new ServiceCenterFunctionNode());
                _centerScheduler.Start();
                //connect to service center.););
                if (ConnectCenter())
                {
                    Console.Write("Dynamic service center......");
                    ConsoleHelper.PrintLine("Connected!", ConsoleColor.DarkGreen);
                    Regist();
                }
                else
                {
                    ConsoleHelper.PrintLine("#Can not connect to dynamic service center #Address: " + _centerIEP.Address + ", #Port: " + _centerIEP.Port + "!", ConsoleColor.DarkRed);
                    ReConnectCenter();
                }
            }
            Console.WriteLine("Preparing host local port......");
            if (settingConfiguration.HostPort <= 0 || settingConfiguration.HostPort > 65535)
            {
                ConsoleHelper.PrintLine("Illegal host port #" + settingConfiguration.HostPort + " !", ConsoleColor.Red);
                Console.WriteLine("Press any key to exit.");
                Console.ReadLine();
                return;
            }
            NetworkNode<DynamicServiceMessage> dynamicNetworkNode = new NetworkNode<DynamicServiceMessage>(new DynamicServiceProtocolStack());
            dynamicNetworkNode.TransportChannelRemoved += TransportChannelRemoved;
            dynamicNetworkNode.ProtocolStack.Initialize();
            dynamicNetworkNode.Regist(new TcpHostTransportChannel(settingConfiguration.HostPort));
            Console.WriteLine("Regist local tcp port #" + settingConfiguration.HostPort + " fine !");
            Global.ServiceNetworkNode = dynamicNetworkNode;
            Console.WriteLine("Preparing task scheduler......");
            _requestScheduler = new RequestScheduler<DynamicServiceMessage>();
            _requestScheduler.Regist(dynamicNetworkNode);
            _requestScheduler.Regist(new DynamicServiceFunctionNode());
            Console.WriteLine("Starting task scheduler......");
            _requestScheduler.Start();
            Console.WriteLine("Request scheduler started !");
            Console.WriteLine("Prepare starting dynamic service heartbeat checker.....");
            ServicePerformancer.Instance.StartChecker();
            Console.WriteLine("Component : #ConnectorComponent  started !");
        }

        void CenterTransportChannelRemoved(object sender, LightSingleArgEventArgs<Guid> e)
        {
            ConsoleHelper.PrintLine("Dynamic service center disconnected, Prepare to reconnect !", ConsoleColor.DarkRed);
            ReConnectCenter();
        }

        /// <summary>
        ///     连接服务中心
        /// </summary>
        /// <returns>返回连接状态</returns>
        private bool ConnectCenter()
        {
            _centerIEP = new IPEndPoint(IPAddress.Parse(SMCSettingConfigSection.Current.Settings.CenterAddress), SMCSettingConfigSection.Current.Settings.CenterPort);
            ITransportChannel centerChannel = new TcpTransportChannel(_centerIEP);
            //connect to service center.
            if(Global.CenterNetworkNode.Connect((IRawTransportChannel) centerChannel))
            {
                Global.CenterId = centerChannel.Key;
                return true;
            }
            return false;
        }

        private void ReConnectCenter()
        {
            if (_connectTimer != null)
            {
                _connectTimer.Stop();
                _connectTimer = null;
            }
            _connectTimer = LightTimer.NewTimer(SMCSettingConfigSection.Current.Settings.ReconnectTimeout, -1);
            _connectTimer.Start(delegate
            {
                if (ConnectCenter())
                {
                    _connectTimer.Stop();
                    _connectTimer = null;
                    Console.Write("Dynamic service center......");
                    ConsoleHelper.PrintLine("Connected!", ConsoleColor.DarkGreen);
                    Regist();
                }
                else
                {
                    ConsoleHelper.PrintLine("#Reconnect to dynamic service center #Address: " + _centerIEP.Address + ", #Port: " + _centerIEP.Port + " failed!", ConsoleColor.DarkYellow);
                }
            }, null);
        }

        protected override void InnerStop()
        {
            StopHeartBeat();
            Console.WriteLine("Component : #ConnectorComponent  stoping......!");
            Console.WriteLine("Stoping scheduler......!");
            if (_requestScheduler != null)
            {
                _requestScheduler.Stop();
            }
            if (_centerScheduler != null)
            {
                _centerScheduler.Stop();
            }
            Console.WriteLine("Prepare stoping dynamic service heartbeat checker.....");
            ServicePerformancer.Instance.StopChecker();
            Console.WriteLine("Component : #ConnectorComponent  stoped......!");
        }

        protected override void InnerOnLoading()
        {
            Console.WriteLine("Component : #ConnectorComponent  loading......!");
        }

        protected override HealthStatus InnerCheckHealth()
        {
            return _requestScheduler != null ? HealthStatus.Good : HealthStatus.Death;
        }

        /// <summary>
        ///     重置心跳时间
        /// </summary>
        /// <param name="interval"></param>
        internal void ResetHeartbeatTime(int interval)
        {
            if (_heartbeatTimer != null)
            {
                _heartbeatTimer.Interval = interval;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     注册DSC回调
        /// </summary>
        /// <param name="result"></param>
        internal void CallbackRegist(bool result)
        {
            if (result)
            {
                Console.Write("#Regist to dynamic service center......");
                ConsoleHelper.PrintLine("Successed!", ConsoleColor.DarkGreen);
                StartHeartBeat();
            }
            else
            {
                ConsoleHelper.PrintLine("#Regist to dynamic service center failed, This program will be run at uncontrolable environment!", ConsoleColor.DarkYellow);
            }
        }

        /// <summary>
        ///     开始心跳
        /// </summary>
        internal void StartHeartBeat()
        {
            StopHeartBeat();
            _heartbeatTimer = LightTimer.NewTimer(SMCSettingConfigSection.Current.Settings.HeartBeatInterval, -1);
            _heartbeatTimer.Start(delegate
                                      {
                                          #region 心跳

                                          DSCHeartBeatRequestMessage requestMessage = new DSCHeartBeatRequestMessage();
                                          requestMessage.MachineName = Environment.MachineName;
                                          #region 应用程序性能

                                          requestMessage.PerformanceItems = new ServicePerformanceItem[4]
                                                          {new ServicePerformanceItem{PerformanceName = "Total processor time", PerformanceValue = Process.GetCurrentProcess().TotalProcessorTime.ToString()}, 
                                                           new ServicePerformanceItem{PerformanceName = "User processor time", PerformanceValue = Process.GetCurrentProcess().UserProcessorTime.ToString()}, 
                                                           new ServicePerformanceItem{PerformanceName = "Private memory size", PerformanceValue = Process.GetCurrentProcess().PrivateMemorySize64.ToString()}, 
                                                           new ServicePerformanceItem{PerformanceName = "Physical memory usage", PerformanceValue = Process.GetCurrentProcess().WorkingSet64.ToString()}};

                                          #endregion
                                          requestMessage.Bind();
                                          Global.CenterNetworkNode.Send(Global.CenterId, requestMessage.Body);

                                          #endregion
                                      }, null);
        }

        /// <summary>
        ///     停止心跳
        /// </summary>
        internal void StopHeartBeat()
        {
            if (_heartbeatTimer != null)
            {
                _heartbeatTimer.Stop();
                _heartbeatTimer = null;
            }
        }

        /// <summary>
        ///     注册到服务中心
        /// </summary>
        private void Regist()
        {
            Console.WriteLine("Preparing to regist to dynamic service center......");
            DSCRegistRequestMessage registRequestMessage = new DSCRegistRequestMessage();
            registRequestMessage.Category = "SMC";
            registRequestMessage.MachineName = Environment.MachineName;
            registRequestMessage.PerformanceItems = new[]
                                                          {new ServicePerformanceItem{PerformanceName = "Total processor time", PerformanceValue = Process.GetCurrentProcess().TotalProcessorTime.ToString()}, 
                                                           new ServicePerformanceItem{PerformanceName = "User processor time", PerformanceValue = Process.GetCurrentProcess().UserProcessorTime.ToString()}, 
                                                           new ServicePerformanceItem{PerformanceName = "Private memory size", PerformanceValue = Process.GetCurrentProcess().PrivateMemorySize64.ToString()}, 
                                                           new ServicePerformanceItem{PerformanceName = "Physical memory usage", PerformanceValue = Process.GetCurrentProcess().WorkingSet64.ToString()}};
            registRequestMessage.OwnServics = ServicePerformancer.Instance.GetServices();
            IDynamicObjectVisitor vistor = Visitor.Create((DynamicDomainComponent)OwnService.GetObject("SMC基础远程服务功能组件"));
            if (vistor != null)
            {
                registRequestMessage.ControlAddress = vistor.GetObject<string>("GetRpcAddress");
            }
            registRequestMessage.Bind();
            Global.CenterNetworkNode.Send(Global.CenterId, registRequestMessage.Body);
            Console.WriteLine("Waiting for regist dynamic service center result......#Interval: " + SMCSettingConfigSection.Current.Settings.RegistTimeout);
            RegistSignal = new AutoResetEvent(false);
            //timeout.
            if (!RegistSignal.WaitOne(SMCSettingConfigSection.Current.Settings.RegistTimeout))
            {
                CallbackRegist(false);
                RegistSignal = null;
            }
            //End regist to dynamic service center
        }

        #endregion

        #region Events

        //channel disconnected.
        void TransportChannelRemoved(object sender, EventArgs.LightSingleArgEventArgs<Guid> e)
        {
            ServicePerformancer.Instance.UnRegist(e.Target);
        }

        #endregion
    }
}
