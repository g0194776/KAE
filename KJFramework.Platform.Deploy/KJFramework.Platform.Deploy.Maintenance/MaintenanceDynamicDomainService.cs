using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using KJFramework.Dynamic.Components;
using KJFramework.IO.Helper;
using KJFramework.Logger;
using KJFramework.Net.Channels;
using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Schedulers;
using KJFramework.Platform.Deploy.Maintenance.Configurations;
using KJFramework.Platform.Deploy.Maintenance.Nodes;
using KJFramework.Platform.Deploy.Maintenance.Properties;
using KJFramework.Platform.Deploy.Metadata.Objects;
using KJFramework.Platform.Deploy.Metadata.Performances;
using KJFramework.Platform.Deploy.SMC.Common.Performances;
using KJFramework.Platform.Deploy.SMC.ProtocolStack.Services;
using KJFramework.Timer;
using FileInfo = KJFramework.Platform.Deploy.Metadata.Objects.FileInfo;

namespace KJFramework.Platform.Deploy.Maintenance
{
    /// <summary>
    ///     可维护动态预服务，提供了一系列基础的管理功能
    /// </summary>
    public class MaintenanceDynamicDomainService : DynamicDomainService
    {
        #region Members

        private NetworkNode<DynamicServiceMessage> _networkNode;
        private IRequestScheduler<DynamicServiceMessage> _requestScheduler;
        private LightTimer _reConnectTimer;
        private IPEndPoint _iep;

        #endregion

        #region Methods

        public override void Start()
        {
            base.Start();
            Global.DynamicService = this;
            Console.WriteLine(Resources.MaintenanceDynamicDomainService_Start_Reading_configurations______);
            string ip = MDDSSettingConfigSection.Current.Settings.ManageAddress;
            int port = MDDSSettingConfigSection.Current.Settings.ManagePort;
            if (string.IsNullOrEmpty(ip) || (port <= 0 || port > 65535))
            {
                ConsoleHelper.PrintLine("#WARING:\r\nNo config item #ManageUrl or Illegal #ManagePort, " + Infomation.Name + " service will be run at uncontrollable platform !", ConsoleColor.Yellow);
            }
            else
            {
                Console.WriteLine(Resources.MaintenanceDynamicDomainService_Start_Preparing_network_for_management_connection_____);
                _networkNode = new NetworkNode<DynamicServiceMessage>(new DynamicServiceProtocolStack());
                Console.WriteLine(Resources.MaintenanceDynamicDomainService_Start_Initializing_protocol_stack_____);
                _networkNode.ProtocolStack.Initialize();
                _iep = new IPEndPoint(IPAddress.Parse(ip), port);
                Console.Write(Resources.MaintenanceDynamicDomainService_Start_Connecting_to_management_node__Address___ + _iep + ".....");
                ITransportChannel transportChannel = new TcpTransportChannel(_iep);
                if (!_networkNode.Connect((IRawTransportChannel) transportChannel))
                {
                    ConsoleHelper.PrintLine("Failed !", ConsoleColor.DarkRed);
                    ConsoleHelper.PrintLine("#WARING:\r\nCan not connect to remoting management node, " + Infomation.Name + " service will be run at uncontrollable platform !", ConsoleColor.Yellow);
                }
                else
                {
                    _networkNode.TransportChannelRemoved += TransportChannelRemoved;
                    Global.ChannelId = transportChannel.Key;
                    ConsoleHelper.PrintLine("Done !", ConsoleColor.DarkGreen);
                    Console.WriteLine(Resources.MaintenanceDynamicDomainService_Start_Initializing_task_scheduler______);
                    _requestScheduler = new RequestScheduler<DynamicServiceMessage>(50);
                    Console.WriteLine(Resources.MaintenanceDynamicDomainService_Start_Regist_network_node_in_task_scheduler______);
                    _requestScheduler.Regist(_networkNode);
                    Console.WriteLine(Resources.MaintenanceDynamicDomainService_Start_Regist_funtion_node_in_task_scheduler______);
                    _requestScheduler.Regist(new DynamicServiceFunctionNode());
                    Console.WriteLine(Resources.MaintenanceDynamicDomainService_Start_Starting_task_scheduler______);
                    _requestScheduler.Start();
                    Regist();
                }
            }
        }

        /// <summary>
        ///     重新连接
        /// </summary>
        private void Reconnect()
        {
            ITransportChannel transportChannel = new TcpTransportChannel(_iep);
            if (_networkNode.Connect((IRawTransportChannel) transportChannel))
            {
                Global.ChannelId = transportChannel.Key;
                ConsoleHelper.PrintLine("#Reconnect to remote management node successfully !");
                _reConnectTimer.Stop();
                _reConnectTimer = null;
                Regist();
            }
        }

        private void Regist()
        {
            Console.Write(Resources.MaintenanceDynamicDomainService_Regist_Starting_regist_to_remote_management_node__Address___ + _iep + "......");
            //连接成功，开始注册
            DynamicServiceRegistRequestMessage registRequestMessage = new DynamicServiceRegistRequestMessage();
            registRequestMessage.ServiceName = Infomation.ServiceName;
            registRequestMessage.Name = Infomation.Name;
            registRequestMessage.Version = Infomation.Version;
            registRequestMessage.Description = Infomation.Description;
            registRequestMessage.ShellVersion = Global.ShellVersion;
            registRequestMessage.ProcessName = Process.GetCurrentProcess().ProcessName;
            registRequestMessage.SupportDomainPerformance = AppDomain.MonitoringIsEnabled;
            registRequestMessage.ComponentCount = ComponentCount;
            if (registRequestMessage.ComponentCount > 0)
            {
                ComponentDetailItem[] items = new ComponentDetailItem[registRequestMessage.ComponentCount];
                int offset = 0;
                foreach (var domainObject in _dynamicObjects.Values)
                {
                    ComponentDetailItem item = new ComponentDetailItem();
                    item.Name = domainObject.Component.Name;
                    item.Status = domainObject.CheckHealth();
                    item.LastUpdateTime = domainObject.LastUpdateTime;
                    item.Version = domainObject.Infomation.Version;
                    item.Description = domainObject.Infomation.Description;
                    item.CatalogName = domainObject.Infomation.CatalogName;
                    item.ServiceName = domainObject.Infomation.ServiceName;
                    items[offset++] = item;
                }
                registRequestMessage.Items = items;
            }
            registRequestMessage.Bind();
            _networkNode.Send(Global.ChannelId, registRequestMessage.Body);
        }

        /// <summary>
        ///     更新组件
        /// </summary>
        /// <param name="componentName">组件名称</param>
        /// <param name="fileName">
        ///     文件名称
        /// <para>* 此属性作为扩展位，暂时不启用。</para>
        /// </param>
        /// <param name="clientTag">客户端标示</param>
        /// <param name="taskId">任务编号</param>
        /// <param name="result">更新结果</param>
        /// <returns>返回更新的结果</returns>
        public ComponentUpdateResultItem[] Update(string componentName, string fileName, string clientTag, string taskId, out bool result)
        {
            ComponentUpdateResultItem[] items = null;
            if (componentName == "*ALL*")
            {
                if (_dynamicObjects.Count > 0)
                {
                    #region Update

                    items = new ComponentUpdateResultItem[_dynamicObjects.Count];
                    int offset = 0;
                    foreach (var dynamicDomainObject in _dynamicObjects.Values)
                        items[offset++] = InnerUpdate(dynamicDomainObject, clientTag, taskId);

                    #endregion
                }
            }
            else
            {
                //这里用分号分隔(如果存在更新多个组件的话)
                string[] coms = componentName.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries);
                if (coms.Length > 0)
                {
                    #region Part Update

                    items = new ComponentUpdateResultItem[coms.Length];
                    int offset = 0;
                    foreach (var dynamicDomainObject in _dynamicObjects.Values)
                    {
                        for (int j = 0; j < coms.Length; j++)
                        {
                            if (dynamicDomainObject.Component.PluginInfo.ServiceName.Equals(coms[j]))
                                items[offset++] = InnerUpdate(dynamicDomainObject, clientTag, taskId);
                        }
                    }

                    #endregion
                }
            }
            result = true;
            return items;
        }

        /// <summary>
        ///     内部更新
        /// </summary>
        /// <param name="dynamicDomainObject">动态域对象</param>
        /// <param name="clientTag">客户端标示</param>
        /// <param name="taskId">任务编号</param>
        private ComponentUpdateResultItem InnerUpdate(DynamicDomainObject dynamicDomainObject, string clientTag, string taskId)
        {
            ComponentUpdateResultItem item = new ComponentUpdateResultItem();
            item.LastUpdateTime = DateTime.Now;
            item.ComponentName = dynamicDomainObject.Infomation.ServiceName;
            try
            {
                NotifyUpdate(dynamicDomainObject.Component.Name, clientTag, taskId, "Updating......");
                dynamicDomainObject.Update();
                NotifyUpdate(dynamicDomainObject.Component.Name, clientTag, taskId, "Update finished !");
                item.Result = true;
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                item.Result = false;
                item.ErrorTrace = ex.Message;
                NotifyUpdate(dynamicDomainObject.Component.Name, clientTag, taskId, "Update failed, Error message : " + ex.Message);
            }
            return item;
        }

        /// <summary>
        ///     通知更新状态
        /// </summary>
        /// <param name="component">组件名称</param>
        /// <param name="clientTag">客户端标示</param>
        /// <param name="taskId">任务编号</param>
        /// <param name="content">更新状态</param>
        private void NotifyUpdate(string component, string clientTag, string taskId, string content)
        {
            try
            {
                DynamicServiceUpdateProcessingMessage message = new DynamicServiceUpdateProcessingMessage();
                message.Header.ClientTag = clientTag;
                message.Header.TaskId = taskId;
                message.ServiceName = Infomation.ServiceName;
                message.ComponentName = component;
                message.Content = content;
                message.Bind();
                if (!message.IsBind)
                {
                    Logs.Logger.Log("Can not bind a DynamicServiceUpdateProcessingMessage, #info = " + Infomation.ServiceName + ", component name = " + component);
                    return;
                }
                _networkNode.Send(Global.ChannelId, message.Body);

            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
            }
        }

        /// <summary>
        ///     获取组件状态
        /// </summary>
        /// <returns></returns>
        public ComponentHealthItem[] GetComponentHealth(string[] components)
        {
            ComponentHealthItem[] items = null;
            //查询所有组件的健康状态
            if (components.Length == 1 && components[0] == "*ALL*")
            {
                if (_dynamicObjects.Count > 0)
                {
                    items = new ComponentHealthItem[_dynamicObjects.Count];
                    int offset = 0;
                    foreach (var dynamicDomainObject in _dynamicObjects.Values)
                        items[offset++] = InnerGetHealth(dynamicDomainObject);
                }
            }
            else
            {
                int offset = 0;
                items = new ComponentHealthItem[components.Length];
                foreach (var dynamicDomainObject in _dynamicObjects.Values)
                {
                    for (int j = 0; j < components.Length; j++)
                    {
                        if (components[j].Equals(dynamicDomainObject.Component.Name))
                            items[offset++] = InnerGetHealth(dynamicDomainObject);
                    }
                }
            }
            return items;
        }

        /// <summary>
        ///     内部获取组件健康状态
        /// </summary>
        /// <param name="dynamicDomainObject">动态域对象</param>
        /// <returns>返回健康状态</returns>
        private ComponentHealthItem InnerGetHealth(DynamicDomainObject dynamicDomainObject)
        {
            ComponentHealthItem item = new ComponentHealthItem();
            item.ComponentName = dynamicDomainObject.Component.Name;
            item.Status = dynamicDomainObject.CheckHealth();
            return item;
        }

        /// <summary>
        ///     心跳
        /// </summary>
        /// <returns>返回心跳回馈包</returns>
        internal DynamicServiceHeartBeatResponseMessage Heartbeat(string clientTag)
        {
            #region 心跳

            DynamicServiceHeartBeatResponseMessage responseMessage = new DynamicServiceHeartBeatResponseMessage();
            responseMessage.Header.ClientTag = clientTag;
            responseMessage.ServiceName = Infomation.ServiceName;
            responseMessage.ComponentCount = _dynamicObjects.Count;

            #region 拥有动态域组件

            //拥有动态域组件
            if (responseMessage.ComponentCount > 0)
            {
                responseMessage.ComponentItems = new ComponentHealthItem[responseMessage.ComponentCount];
                //可以捕获应用程序域的性能
                if (AppDomain.MonitoringIsEnabled)
                {
                    responseMessage.DomainItems = new DomainPerformanceItem[responseMessage.ComponentCount];
                }
                int offset = 0;
                foreach (var dynamicDomainObject in _dynamicObjects.Values)
                {
                    ComponentHealthItem componentHealthItem = new ComponentHealthItem();
                    componentHealthItem.ComponentName = dynamicDomainObject.Component.Name;
                    componentHealthItem.Status = dynamicDomainObject.CheckHealth();
                    responseMessage.ComponentItems[offset] = componentHealthItem;
                    if (AppDomain.MonitoringIsEnabled)
                    {
                        DomainPerformanceItem domainPerformanceItem = new DomainPerformanceItem();
                        AppDomain appDomain = dynamicDomainObject.GetDomain();
                        domainPerformanceItem.AppDomainName = appDomain.FriendlyName;
                        domainPerformanceItem.Cpu = appDomain.MonitoringTotalProcessorTime.TotalMilliseconds;
                        domainPerformanceItem.Memory = appDomain.MonitoringTotalAllocatedMemorySize;
                        responseMessage.DomainItems[offset] = domainPerformanceItem;
                    }
                    offset++;
                }
            }

            #endregion

            #region 应用程序性能

            responseMessage.PerformanceItems = new ServicePerformanceItem[4]
                                                          {new ServicePerformanceItem{PerformanceName = "Total processor time", PerformanceValue = Process.GetCurrentProcess().TotalProcessorTime.ToString()}, 
                                                           new ServicePerformanceItem{PerformanceName = "User processor time", PerformanceValue = Process.GetCurrentProcess().UserProcessorTime.ToString()}, 
                                                           new ServicePerformanceItem{PerformanceName = "Private memory size", PerformanceValue = Process.GetCurrentProcess().PrivateMemorySize64.ToString()}, 
                                                           new ServicePerformanceItem{PerformanceName = "Physical memory usage", PerformanceValue = Process.GetCurrentProcess().WorkingSet64.ToString()}};

            #endregion

            responseMessage.Bind();
            _networkNode.Send(Global.ChannelId, responseMessage.Body);
            return responseMessage;

            #endregion
        }
        
        /// <summary>
        ///     获取文件详细信息
        /// </summary>
        /// <param name="msg">请求消息</param>
        /// <returns>返回文件详细信息</returns>
        internal DynamicServiceGetFileInfomationResponseMessage GetFileInfomation(DynamicServiceGetFileInfomationRequestMessage msg)
        {
            DynamicServiceGetFileInfomationResponseMessage responseMessage = new DynamicServiceGetFileInfomationResponseMessage();
            responseMessage.Header.ClientTag = msg.Header.ClientTag;
            responseMessage.Header.TaskId = msg.Header.TaskId;
            responseMessage.ServiceName = Infomation.ServiceName;
            if (msg .Files== "*ALL*")
            {
                string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.*", SearchOption.AllDirectories);
                if (files != null)
                {
                    responseMessage.Files = new FileInfo[files.Length];
                    for (int i = 0; i < files.Length; i++)
                    {
                        responseMessage.Files[i] = GetFileInfo(files[i]);
                    }
                }
            }
            else
            {
                string[] files = msg.Files.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
                if (files.Length > 0)
                {
                    List<FileInfo> results = new List<FileInfo>();
                    string[] orgFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.*", SearchOption.AllDirectories);
                    for (int i = 0; i < files.Length; i++)
                    {
                        var result = orgFiles.Where(f => Path.GetFileName(f) == files[i]);
                        if (result != null)
                        {
                            results.AddRange(result.Select(GetFileInfo));
                        }
                    }
                    responseMessage.Files = results.ToArray();
                }
            }
            return responseMessage;
        }

        /// <summary>
        ///     获取详细信息
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>返回文件详细信息</returns>
        private FileInfo GetFileInfo(string filePath)
        {
            FileInfo fileInfo = new FileInfo();
            fileInfo.IsExists = File.Exists(filePath);
            if (fileInfo.IsExists)
            {
                fileInfo.FileName = Path.GetFileName(filePath);
                fileInfo.LastModifyTime = File.GetLastWriteTime(filePath);
                fileInfo.CreateTime = File.GetCreationTime(filePath);
                fileInfo.Directory = Path.GetDirectoryName(filePath);
                fileInfo.Size = new System.IO.FileInfo(filePath).Length;
            }
            return fileInfo;
        }

        #endregion

        #region Events

        //channel disconnected.
        void TransportChannelRemoved(object sender, EventArgs.LightSingleArgEventArgs<Guid> e)
        {
            ConsoleHelper.PrintLine("#WARING:\r\nRemote management node disconnected, " + Infomation.Name + " service will be run at uncontrollable platform !", ConsoleColor.Yellow);
            if (_reConnectTimer == null)
            {
                _reConnectTimer = LightTimer.NewTimer(MDDSSettingConfigSection.Current.Settings.ReconnectInterval, -1);
            }
            _reConnectTimer.Start(Reconnect, null);
        }

        #endregion

        #region For test

        internal void Schedule(DynamicServiceMessage message, IMessageTransportChannel<DynamicServiceMessage> channel)
        {
            _requestScheduler.Schedule(message, _networkNode, channel);
        }

        #endregion
    }
}