using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using KJFramework.Basic.Enum;
using KJFramework.Dynamic.Components;
using KJFramework.IO.Helper;
using KJFramework.Net.Channels;
using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Schedulers;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;
using KJFramework.Platform.Deploy.DSN.Common.Configurations;
using KJFramework.Platform.Deploy.DSN.CP.Connector.Nodes;
using KJFramework.Platform.Deploy.SMC.Common.Performances;
using KJFramework.Plugin;
using KJFramework.Timer;

namespace KJFramework.Platform.Deploy.DSN.CP.Connector
{
    public class ConnectorComponent : DynamicDomainComponent
    {
        #region Constructor

        public ConnectorComponent()
        {
            _name = "DSN基础连接器功能组件";
            _pluginInfo = new PluginInfomation();
            _pluginInfo.CatalogName = "Plugins";
            _pluginInfo.Version = "0.0.0.1";
            _pluginInfo.ServiceName = "DSN.Components.Connector";
            _pluginInfo.Description = "DSN基础连接器组件，提供了与动态服务和服务中心连接的功能";
            Global.ConnectorInstance = this;
        }

        #endregion

        #region Members

        private IRequestScheduler<DSCMessage> _requestScheduler;
        internal AutoResetEvent RegistSignal;
        private LightTimer _heartbeatTimer;
        private LightTimer _connectTimer;
        private IPEndPoint _centerIEP;

        #endregion

        #region Overrides of DynamicDomainComponent

        protected override void InnerStart()
        {
            Console.WriteLine("Initializing Deploy Service Node......");
            string ipAddress = DSNSettingConfigSection.Current.Settings.CenterAddress;
            int port = DSNSettingConfigSection.Current.Settings.CenterPort;
            if (string.IsNullOrEmpty(ipAddress) || (port > 65536 || port <= 0))
            {
                ConsoleHelper.PrintLine("#WARING:\r\nNo config item #CenterUrl, 'ConnectorComponent' will be run at uncontrollable platform !", ConsoleColor.Yellow);
            }
            else
            {
                Global.CenterNode = new NetworkNode<DSCMessage>(new DSCProtocolStack());
                Global.CenterNode.TransportChannelRemoved += TransportChannelRemoved;
                Console.WriteLine("Initializing protocol stack......");
                Global.CenterNode.ProtocolStack.Initialize();
                Console.WriteLine("Preparing connect to center node......");
                //Disconnected.
                if (!ConnectCenter())
                {
                    ConsoleHelper.PrintLine("#Can not connect to Center node. Address: " + ipAddress + ", Port: " + port, ConsoleColor.DarkRed);
                    ConsoleHelper.PrintLine("#Deploy service node will be run at uncontrollable environment !", ConsoleColor.DarkYellow);
                    ConsoleHelper.PrintLine("#Enable reconnecting......", ConsoleColor.DarkYellow);
                    //Reconenct logic.
                    ReConnectCenter();
                }
                Console.WriteLine("Preparing initialize task scheduler......");
                _requestScheduler = new RequestScheduler<DSCMessage>(100);
                Console.WriteLine("Preparing regist network node......");
                _requestScheduler.Regist(Global.CenterNode);
                Console.WriteLine("Preparing regist function node......");
                _requestScheduler.Regist(new ConnectorFunctionNode());
                _requestScheduler.Start();
                Regist();
                Console.WriteLine("Task scheduler started !");
            }
            Console.WriteLine("Deploy Service Node Started!");
        }

        protected override void InnerStop()
        {
            Console.WriteLine("Stoping task scheduler......");
            if (_requestScheduler != null)
            {
                _requestScheduler.Stop();
                _requestScheduler = null;
            }
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

        #region Methods

        /// <summary>
        ///     注册到服务中心
        /// </summary>
        private void Regist()
        {
            Console.WriteLine("Preparing regist to dyamic center service......");
            DSCRegistRequestMessage registRequestMessage = new DSCRegistRequestMessage();
            registRequestMessage.MachineName = Global.MachineName;
            registRequestMessage.PerformanceItems = new[]
                                                          {new ServicePerformanceItem{PerformanceName = "Total processor time", PerformanceValue = Process.GetCurrentProcess().TotalProcessorTime.ToString()}, 
                                                           new ServicePerformanceItem{PerformanceName = "User processor time", PerformanceValue = Process.GetCurrentProcess().UserProcessorTime.ToString()}, 
                                                           new ServicePerformanceItem{PerformanceName = "Private memory size", PerformanceValue = Process.GetCurrentProcess().PrivateMemorySize64.ToString()}, 
                                                           new ServicePerformanceItem{PerformanceName = "Physical memory usage", PerformanceValue = Process.GetCurrentProcess().WorkingSet64.ToString()}};
            registRequestMessage.Category = "DSN";
            registRequestMessage.DeployAddress = string.Format("tcp://localhost:{0}/", DSNSettingConfigSection.Current.Settings.HostPort);
            registRequestMessage.Bind();
            Global.CenterNode.Send(Global.CenterId, registRequestMessage.Body);
            Console.WriteLine("Waiting for regist dynamic service center result......#Interval: " + DSNSettingConfigSection.Current.Settings.RegistTimeout);
            RegistSignal = new AutoResetEvent(false);
            //timeout.
            if (!RegistSignal.WaitOne(DSNSettingConfigSection.Current.Settings.RegistTimeout))
            {
                CallbackRegist(false);
                RegistSignal = null;
            }
            //End regist to dynamic service center
        }

        /// <summary>
        ///     注册DSC回调
        /// </summary>
        /// <param name="result"></param>
        internal void CallbackRegist(bool result)
        {
            if (result)
            {
                Console.WriteLine("#Regist to dynamic service center......Successed!");
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
            _heartbeatTimer = LightTimer.NewTimer(DSNSettingConfigSection.Current.Settings.HeartBeatInterval, -1);
            _heartbeatTimer.Start(delegate
            {
                #region 心跳

                DSCHeartBeatRequestMessage requestMessage = new DSCHeartBeatRequestMessage();
                requestMessage.MachineName = Global.MachineName;
                #region 应用程序性能

                requestMessage.PerformanceItems = new ServicePerformanceItem[4]
                                                          {new ServicePerformanceItem{PerformanceName = "Total processor time", PerformanceValue = Process.GetCurrentProcess().TotalProcessorTime.ToString()}, 
                                                           new ServicePerformanceItem{PerformanceName = "User processor time", PerformanceValue = Process.GetCurrentProcess().UserProcessorTime.ToString()}, 
                                                           new ServicePerformanceItem{PerformanceName = "Private memory size", PerformanceValue = Process.GetCurrentProcess().PrivateMemorySize64.ToString()}, 
                                                           new ServicePerformanceItem{PerformanceName = "Physical memory usage", PerformanceValue = Process.GetCurrentProcess().WorkingSet64.ToString()}};

                #endregion
                requestMessage.Bind();
                Global.CenterNode.Send(Global.CenterId, requestMessage.Body);

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
        ///     连接服务中心
        /// </summary>
        /// <returns>返回连接状态</returns>
        private bool ConnectCenter()
        {
            _centerIEP = new IPEndPoint(IPAddress.Parse(DSNSettingConfigSection.Current.Settings.CenterAddress), DSNSettingConfigSection.Current.Settings.CenterPort);
            ITransportChannel centerChannel = new TcpTransportChannel(_centerIEP);
            //connect to service center.
            if (Global.CenterNode.Connect((IRawTransportChannel) centerChannel))
            {
                Global.CenterId = centerChannel.Key;
                return true;
            }
            return false;
        }

        /// <summary>
        ///     重新连接服务中心
        /// </summary>
        private void ReConnectCenter()
        {
            if (_connectTimer != null)
            {
                _connectTimer.Stop();
                _connectTimer = null;
            }
            _connectTimer = LightTimer.NewTimer(DSNSettingConfigSection.Current.Settings.ReconnectTimeout, -1);
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

        /// <summary>
        ///     重置心跳时间
        /// </summary>
        /// <param name="interval">心跳间隔</param>
        internal void ResetHeartbeatTime(int interval)
        {
            if (_heartbeatTimer != null)
            {
                _heartbeatTimer.Interval = interval;
            }
        }

        #endregion

        #region Events

        void TransportChannelRemoved(object sender, EventArgs.LightSingleArgEventArgs<Guid> e)
        {
            ConsoleHelper.PrintLine("#Dynamic service center disconnected, enable reconnecting......", ConsoleColor.DarkYellow);
            ReConnectCenter();
        }

        #endregion
    }
}