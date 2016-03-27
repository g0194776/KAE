using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using KJFramework.Basic.Enum;
using KJFramework.IO.Helper;
using KJFramework.Logger;
using KJFramework.Net.Channels;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;
using KJFramework.Platform.Deploy.Metadata.Enums;
using KJFramework.Platform.Deploy.Metadata.Objects;
using KJFramework.Platform.Deploy.SMC.CP.Connector.Configurations;
using KJFramework.Platform.Deploy.SMC.CP.Connector.Objects;
using KJFramework.Platform.Deploy.SMC.ProtocolStack.Services;
using KJFramework.Timer;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector
{
    /// <summary>
    ///     服务性能器，提供了相关的基本操作
    /// </summary>
    internal class ServicePerformancer
    {
        #region Constructor

        /// <summary>
        ///     服务性能器，提供了相关的基本操作
        /// </summary>
        private ServicePerformancer()
        {
            
        }

        #endregion

        #region Members

        protected ConcurrentDictionary<string, IDynamicServiceItem> _services = new ConcurrentDictionary<string, IDynamicServiceItem>();
        public static readonly ServicePerformancer Instance = new ServicePerformancer();
        private LightTimer _serviceChecker;
        private LightTimer _deadChecker;

        #endregion

        #region Methods

        /// <summary>
        ///     注册一个动态服务项
        /// </summary>
        /// <param name="id">通道编号</param>
        /// <param name="message">请求消息</param>
        public bool Regist(Guid id, DynamicServiceRegistRequestMessage message)
        {
            try
            {
                IDynamicServiceItem item;
                if (_services.TryGetValue(message.ServiceName, out item))
                {
                    DynamicServiceItem nItem = (DynamicServiceItem) item;
                    nItem.ChannelId = id;
                    nItem.ServiceName = message.ServiceName;
                    nItem.Name = message.Name;
                    nItem.Description = message.Description;
                    nItem.ShellVersion = message.ShellVersion;
                    nItem.Version = message.Version;
                    nItem.SupportDomainPerformance = message.SupportDomainPerformance;
                    nItem.Components = message.Items;
                    nItem.ComponentCount = message.ComponentCount;
                    nItem.LiveStatus = ServiceLiveStatus.Connected;
                    //Try to notify center service, update service !
                    NotifyStatusChange(nItem, StatusChangeTypes.UpdateService);
                    return true;
                }
                DynamicServiceItem newItem= new DynamicServiceItem();
                newItem.ChannelId = id;
                newItem.ServiceName = message.ServiceName;
                newItem.Name = message.Name;
                newItem.Description = message.Description;
                newItem.Version = message.Version;
                newItem.ShellVersion = message.ShellVersion;
                newItem.SupportDomainPerformance = message.SupportDomainPerformance;
                newItem.Components = message.Items;
                newItem.ComponentCount = message.ComponentCount;
                newItem.LiveStatus = ServiceLiveStatus.Connected;
                if (!_services.TryAdd(message.ServiceName, newItem))
                {
                    Logs.Logger.Log("不能够注册一个新的动态服务项 ! #Service Name : " + item.ServiceName);
                    return false;
                }
                //Try to notify center service, add new service !
                NotifyStatusChange(newItem, StatusChangeTypes.NewService);
                return true;
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                return false;
            }
        }

        /// <summary>
        ///     状态变更通知
        /// </summary>
        /// <param name="nItem">服务原有项</param>
        /// <param name="statusChangeType">变更类型</param>
        public void NotifyStatusChange(DynamicServiceItem nItem, StatusChangeTypes statusChangeType)
        {
            DSCStatusChangeRequestMessage requestMessage = new DSCStatusChangeRequestMessage();
            requestMessage.MachineName = Environment.MachineName;
            requestMessage.StatusChangeType = statusChangeType;
            OwnServiceItem tempItem = new OwnServiceItem
                                          {
                                              ComponentCount = nItem.ComponentCount,
                                              Description = nItem.Description,
                                              Version = nItem.Version,
                                              Name = nItem.Name,
                                              ServiceName = nItem.ServiceName,
                                              LastHeartbeatTime = nItem.LastHeartBeatTime,
                                              LastUpdateTime = nItem.LastUpdateTime,
                                              LiveStatus = nItem.LiveStatus,
                                              SupportDomainPerformance = nItem.SupportDomainPerformance,
                                              ShellVersion = nItem.ShellVersion,
                                              PerformanceItems = nItem.GetPerformances(),
                                              DomainPerformanceItems = nItem.GetDomainPerformances()
                                          };

            if (nItem.Components != null)
            {
                tempItem.Componnets = new OwnComponentItem[nItem.Components.Length];
                for (int i = 0; i < nItem.Components.Length; i++)
                {
                    ComponentDetailItem componentDetailItem = nItem.Components[i];
                    OwnComponentItem tempComponent = new OwnComponentItem();
                    tempComponent.ComponentName = componentDetailItem.ServiceName;
                    tempComponent.Name = componentDetailItem.Name;
                    tempComponent.Version = componentDetailItem.Version;
                    tempComponent.Description = componentDetailItem.Description;
                    tempComponent.Status = componentDetailItem.Status;
                    tempComponent.LastUpdateTime = componentDetailItem.LastUpdateTime;
                    tempItem.Componnets[i] = tempComponent;
                }
            }
            requestMessage.Items = new[] { tempItem };
            requestMessage.Bind();
            Global.CenterNetworkNode.Send(Global.CenterId, requestMessage.Body);
        }

        /// <summary>
        ///     注销一个动态服务
        /// </summary>
        /// <param name="serviceName">动态服务名称</param>
        public void UnRegist(string serviceName)
        {
            IDynamicServiceItem item;
            if (_services.TryGetValue(serviceName, out item))
            {
                item.LiveStatus = ServiceLiveStatus.Disconnected;
                if (item.Components != null)
                {
                    foreach (ComponentDetailItem componentDetailItem in item.Components)
                    {
                        componentDetailItem.Status = HealthStatus.Death;
                    }
                }
                NotifyStatusChange((DynamicServiceItem)item, StatusChangeTypes.DelService);
                //for test
                ConsoleHelper.PrintLine("#Offline notice : service -> " + item.ServiceName + " has been offline.", ConsoleColor.DarkYellow);
            }
        }

        /// <summary>
        ///     注销一个动态服务
        /// </summary>
        /// <param name="channelId">通道编号</param>
        public void UnRegist(Guid channelId)
        {
            var result = _services.Values.Where(service => service.ChannelId == channelId);
            if (result != null && result.Count() > 0)
            {
                IDynamicServiceItem item = result.First();
                item.LiveStatus = ServiceLiveStatus.Disconnected;
                if (item.Components != null)
                {
                    foreach (ComponentDetailItem componentDetailItem in item.Components)
                    {
                        componentDetailItem.Status = HealthStatus.Death;
                    }
                }
                NotifyStatusChange((DynamicServiceItem)item, StatusChangeTypes.DelService);
                //for test
                ConsoleHelper.PrintLine("#Offline notice : service -> " + item.ServiceName + " has been offline.",ConsoleColor.DarkYellow);
            }
        }

        /// <summary>
        ///     心跳
        /// </summary>
        /// <param name="message">请求消息</param>
        public bool HeartBeat(DynamicServiceHeartBeatResponseMessage message)
        {
            try
            {
                IDynamicServiceItem item;
                if (_services.TryGetValue(message.ServiceName, out item))
                {
                    item.LastHeartBeatTime = DateTime.Now;
                    item.AppDomainCount = message.DomainItems == null ? 0 : message.DomainItems.Length;
                    item.SupportDomainPerformance = message.SupportDomainPerformance;
                    item.ComponentCount = message.ComponentCount;
                    item.LiveStatus = ServiceLiveStatus.Connected;
                    item.Update(message.DomainItems);
                    item.Update(message.PerformanceItems);
                    item.Update(message.ComponentItems);
                    return true;
                }
                return false;
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                return false;
            }
        }

        /// <summary>
        ///     通知服务组件更新结果
        /// </summary>
        /// <param name="message"></param>
        public void Update(DynamicServiceUpdateComponentResponseMessage message)
        {
            try
            {
                IDynamicServiceItem item;
                if (_services.TryGetValue(message.ServiceName, out item))
                {
                    item.LastUpdateTime = DateTime.Now;
                    if (!String.IsNullOrEmpty(message.ErrorTrace))
                    {
                        item.LastError = message.ErrorTrace;
                    }
                    item.Update(message.Items);
                }
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
            }
        }

        /// <summary>
        ///     通知服务组件健康状态结果
        /// </summary>
        /// <param name="message">健康状态消息</param>
        public void Update(DynamicServiceGetComponentHealthResponseMessage message)
        {
            try
            {
                IDynamicServiceItem item;
                if (_services.TryGetValue(message.ServiceName, out item))
                {
                    item.Update(message.Items);
                }
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
            }
        }

        /// <summary>
        ///     通知当前被控主机内的指定服务进行更新
        /// </summary>
        /// <param name="message"></param>
        public bool Update(DSCUpdateComponentRequestMessage message)
        {
            try
            {
                IDynamicServiceItem item;
                if (_services.TryGetValue(message.ServiceName, out item))
                {
                    DynamicServiceUpdateComponentRequestMessage requestMessage = new DynamicServiceUpdateComponentRequestMessage();
                    requestMessage.ComponentName = message.ComponentName;
                    requestMessage.Header.ClientTag = message.Header.ClientTag;
                    requestMessage.FileName = message.FileName;
                    ITransportChannel channel = Global.ServiceNetworkNode.GetTransportChannel(item.ChannelId);
                    if (channel != null && channel.IsConnected)
                    {
                        requestMessage.Bind();
                        channel.Send(requestMessage.Body);
                        return true;
                    }
                }
                //返回不成功的通知
                return false;
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                return false;
            }
        }

        /// <summary>
        ///     获取指定服务的相关文件详细信息
        /// </summary>
        /// <param name="message">请求消息</param>
        public bool GetFileInfos(DSCGetFileInfomationRequestMessage message)
        {
            try
            {
                IDynamicServiceItem item;
                if (_services.TryGetValue(message.ServiceName, out item))
                {
                    DynamicServiceGetFileInfomationRequestMessage requestMessage = new DynamicServiceGetFileInfomationRequestMessage();
                    requestMessage.Files = message.Files;
                    requestMessage.Header.ClientTag = message.Header.ClientTag;
                    ITransportChannel channel = Global.ServiceNetworkNode.GetTransportChannel(item.ChannelId);
                    if (channel != null && channel.IsConnected)
                    {
                        requestMessage.Bind();
                        channel.Send(requestMessage.Body);
                        return true;
                    }
                }
                //返回不成功的通知
                return false;
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                return false;
            }
        }

        /// <summary>
        ///     通知指定服务获取组件健康状态
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Update(DSCGetComponentHealthRequestMessage message)
        {
            try
            {
                IDynamicServiceItem item;
                if (_services.TryGetValue(message.ServiceName, out item))
                {
                    DynamicServiceGetComponentHealthRequestMessage requestMessage = new DynamicServiceGetComponentHealthRequestMessage();
                    requestMessage.Components = message.Components;
                    requestMessage.Header.ClientTag = message.Header.ClientTag;
                    ITransportChannel channel = Global.ServiceNetworkNode.GetTransportChannel(item.ChannelId);
                    if (channel != null && channel.IsConnected)
                    {
                        requestMessage.Bind();
                        channel.Send(requestMessage.Body);
                        return true;
                    }
                }
                //返回不成功的通知
                return false;
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                return false;
            }
        }

        /// <summary>
        ///     获取服务的详细信息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public DSCGetServicesResponseMessage GetServiceInfomation(DSCGetServicesRequestMessage message)
        {
            DSCGetServicesResponseMessage responseMessage = new DSCGetServicesResponseMessage();
            responseMessage.MachineName = Environment.MachineName;
            int offset = 0;
            if (message.ServiceName == "*ALL*")
            {
                if (_services.Count > 0)
                {
                    responseMessage.Items = new OwnServiceItem[_services.Count];
                    foreach (IDynamicServiceItem dynamicServiceItem in _services.Values)
                    {
                        OwnServiceItem item = new OwnServiceItem();
                        item.ServiceName = dynamicServiceItem.ServiceName;
                        item.Name = dynamicServiceItem.Name;
                        item.Description = dynamicServiceItem.Description;
                        item.Version = dynamicServiceItem.Version;
                        item.ComponentCount = dynamicServiceItem.ComponentCount;
                        item.LastHeartbeatTime = dynamicServiceItem.LastHeartBeatTime;
                        item.LastUpdateTime = dynamicServiceItem.LastUpdateTime;
                        item.LiveStatus = dynamicServiceItem.LiveStatus;
                        item.SupportDomainPerformance = dynamicServiceItem.SupportDomainPerformance;
                        item.PerformanceItems = dynamicServiceItem.GetPerformances();
                        item.DomainPerformanceItems = dynamicServiceItem.GetDomainPerformances();
                        if (dynamicServiceItem.Components != null && dynamicServiceItem.Components.Length > 0)
                        {
                            item.Componnets = new OwnComponentItem[dynamicServiceItem.Components.Length];
                            for (int i = 0; i < dynamicServiceItem.Components.Length; i++)
                            {
                                OwnComponentItem ownComponentItem = new OwnComponentItem();
                                ComponentDetailItem componentDetailItem = dynamicServiceItem.Components[i];
                                ownComponentItem.ComponentName = componentDetailItem.ServiceName;
                                ownComponentItem.Name = componentDetailItem.Name;
                                ownComponentItem.Version = componentDetailItem.Version;
                                ownComponentItem.Description = componentDetailItem.Description;
                                ownComponentItem.Status = componentDetailItem.Status;
                                ownComponentItem.LastUpdateTime = componentDetailItem.LastUpdateTime;
                                item.Componnets[i] = ownComponentItem;
                            }
                        }
                        responseMessage.Items[offset++] = item;
                    }
                }
            }
            else
            {
                string[] subs = message.ServiceName.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
                responseMessage.Items = new OwnServiceItem[subs.Length];
                foreach (IDynamicServiceItem dynamicServiceItem in _services.Values)
                {
                    for (int i = 0; i < subs.Length; i++)
                    {
                        if (dynamicServiceItem.ServiceName == subs[i])
                        {
                            OwnServiceItem item = new OwnServiceItem();
                            item.ServiceName = dynamicServiceItem.ServiceName;
                            item.Name = dynamicServiceItem.Name;
                            item.Description = dynamicServiceItem.Description;
                            item.Version = dynamicServiceItem.Version;
                            item.ComponentCount = dynamicServiceItem.ComponentCount;
                            item.LastHeartbeatTime = dynamicServiceItem.LastHeartBeatTime;
                            item.LastUpdateTime = dynamicServiceItem.LastUpdateTime;
                            item.LiveStatus = dynamicServiceItem.LiveStatus;
                            item.SupportDomainPerformance = dynamicServiceItem.SupportDomainPerformance;
                            item.PerformanceItems = dynamicServiceItem.GetPerformances();
                            item.DomainPerformanceItems = dynamicServiceItem.GetDomainPerformances();
                            responseMessage.Items[offset++] = item;
                            if (dynamicServiceItem.Components != null && dynamicServiceItem.Components.Length > 0)
                            {
                                item.Componnets = new OwnComponentItem[dynamicServiceItem.Components.Length];
                                for (int j = 0; j < dynamicServiceItem.Components.Length; j++)
                                {
                                    OwnComponentItem ownComponentItem = new OwnComponentItem();
                                    ComponentDetailItem componentDetailItem = dynamicServiceItem.Components[j];
                                    ownComponentItem.ComponentName = componentDetailItem.ServiceName;
                                    ownComponentItem.Name = componentDetailItem.Name;
                                    ownComponentItem.Version = componentDetailItem.Version;
                                    ownComponentItem.Description = componentDetailItem.Description;
                                    ownComponentItem.Status = componentDetailItem.Status;
                                    item.Componnets[j] = ownComponentItem;
                                }
                            }
                        }
                    }
                }
            }
            return responseMessage;
        }

        /// <summary>
        ///     通知更新进度
        /// </summary>
        /// <param name="message">更新进度消息</param>
        public void NotifyUpdateProcessing(DynamicServiceUpdateProcessingMessage message)
        {
            Console.WriteLine("Updating service #name : " + message.ServiceName + ", Status : " + message.Content);
        }

        /// <summary>
        ///     获取所有被控服务
        /// </summary>
        /// <returns>返回所有服务</returns>
        internal OwnServiceItem[] GetServices()
        {
            OwnServiceItem[] items = null;
            int count = _services.Count;
            if (count > 0)
            {
                int offset = 0;
                items = new OwnServiceItem[count];
                foreach (IDynamicServiceItem serviceItem in _services.Values)
                {
                    OwnServiceItem ownServiceItem = new OwnServiceItem();
                    ownServiceItem.ServiceName = serviceItem.ServiceName;
                    ownServiceItem.Description = serviceItem.Description;
                    ownServiceItem.Version = serviceItem.Version;
                    ownServiceItem.Name = serviceItem.Name;
                    ownServiceItem.ComponentCount = serviceItem.ComponentCount;
                    ownServiceItem.LastHeartbeatTime = serviceItem.LastHeartBeatTime;
                    ownServiceItem.LastUpdateTime = serviceItem.LastUpdateTime;
                    ownServiceItem.LiveStatus = serviceItem.LiveStatus;
                    ownServiceItem.PerformanceItems = serviceItem.GetPerformances();
                    ownServiceItem.DomainPerformanceItems = serviceItem.GetDomainPerformances();
                    if (serviceItem.Components != null && serviceItem.Components.Length > 0)
                    {
                        ownServiceItem.Componnets = new OwnComponentItem[serviceItem.Components.Length];
                        for (int i = 0; i < serviceItem.Components.Length; i++)
                        {
                            OwnComponentItem componentItem = new OwnComponentItem();
                            ComponentDetailItem detailItem = serviceItem.Components[i];
                            componentItem.ComponentName = detailItem.ServiceName;
                            componentItem.Description = detailItem.Description;
                            componentItem.Name = detailItem.Name;
                            componentItem.Version = detailItem.Version;
                            componentItem.Status = detailItem.Status;
                            componentItem.LastUpdateTime = detailItem.LastUpdateTime;
                            ownServiceItem.Componnets[i] = componentItem;
                        }
                    }
                    items[offset++] = ownServiceItem;
                }
            }
            return items;
        }

        /// <summary>
        ///     运行检查器
        /// </summary>
        public void StartChecker()
        {
            if (_serviceChecker == null)
            {
                _serviceChecker = LightTimer.NewTimer(SMCSettingConfigSection.Current.Settings.CheckInterval, -1);
            }
            _serviceChecker.Start(delegate
            {
                DynamicServiceHeartBeatRequestMessage requestMessage = new DynamicServiceHeartBeatRequestMessage();
                requestMessage.Bind();
                foreach (IDynamicServiceItem serviceItem in _services.Values)
                {
                    Global.ServiceNetworkNode.Send(serviceItem.ChannelId, requestMessage.Body);
                }
            }, null);
            if (_deadChecker == null)
            {
                _deadChecker = LightTimer.NewTimer(30000, -1);
            }
            _deadChecker.Start(
                delegate
                    {
                        DateTime now = DateTime.Now;
                        List<string> keys = new List<string>();
                        //collect infomation for dead service.
                        foreach (KeyValuePair<string, IDynamicServiceItem> dynamicServiceItem in _services)
                        {
                            if ((now - dynamicServiceItem.Value.LastHeartBeatTime).TotalSeconds > 30)
                            {
                                keys.Add(dynamicServiceItem.Key);
                            }
                        }
                        //remove dead service.
                        if (keys.Count > 0)
                        {
                            //batch notify to center.
                            DSCStatusChangeRequestMessage statusChangeRequestMessage = new DSCStatusChangeRequestMessage();
                            statusChangeRequestMessage.MachineName = Environment.MachineName;
                            statusChangeRequestMessage.StatusChangeType = StatusChangeTypes.DelService;
                            List<OwnServiceItem> items = new List<OwnServiceItem>();
                            for (int i = 0; i < keys.Count; i++)
                            {
                                IDynamicServiceItem item;
                                if(_services.TryRemove(keys[i], out item))
                                {
                                    items.Add(new OwnServiceItem
                                    {
                                        ServiceName = item.ServiceName,
                                        ComponentCount = item.ComponentCount
                                    });
                                }
                            }
                            statusChangeRequestMessage.Items = items.ToArray();
                            statusChangeRequestMessage.Bind();
                            Global.CenterNetworkNode.Send(Global.CenterId, statusChangeRequestMessage.Body);
                        }
                    }, null);
        }

        /// <summary>
        ///     重置与动态服务心跳的时间间隔
        /// </summary>
        /// <param name="interval"></param>
        internal void ResetServiceHeartbeatTime(int interval)
        {
            if (_serviceChecker != null)
            {
                _serviceChecker.Interval = interval;
            }
        }

        /// <summary>
        ///     停止检查器
        /// </summary>
        public void StopChecker()
        {
            if (_serviceChecker != null)
            {
                _serviceChecker.Stop();
                _serviceChecker = null;
            }
            if (_deadChecker != null)
            {
                _deadChecker.Stop();
                _deadChecker = null;
            }
        }

        #endregion
    }
}