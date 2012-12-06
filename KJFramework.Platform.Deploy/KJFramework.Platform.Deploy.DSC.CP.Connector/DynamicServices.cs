using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using KJFramework.IO.Helper;
using KJFramework.Logger;
using KJFramework.Net.Channels;
using KJFramework.Platform.Deploy.DSC.CP.Connector.Objects;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;
using KJFramework.Platform.Deploy.Metadata.Objects;
using KJFramework.Platform.Deploy.SMC.Common.Performances;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector
{
    internal class DynamicServices
    {
        #region Members

        private static ConcurrentDictionary<string, MachineLog> _services = new ConcurrentDictionary<string, MachineLog>();

        #endregion

        #region Methods

        /// <summary>
        ///     注册一个控制器
        /// </summary>
        /// <param name="message">注册请求消息</param>
        /// <param name="channelId">信道编号</param>
        /// <returns>返回注册的结果</returns>
        public static bool Regist(DSCRegistRequestMessage message, Guid channelId)
        {
            MachineLog log;
            if (!_services.TryGetValue(message.MachineName, out log))
            {
                log = new MachineLog();
                log.MachineName = message.MachineName;
                log.Category = message.Category;
                log.ControlServiceAddress = message.ControlAddress;
                log.ChannelId = channelId;
                log.DeployAddress = message.DeployAddress;
                AddLog(message, log);
                return _services.TryAdd(log.MachineName, log);
            }
            //existed.
            log.MachineName = message.MachineName;
            log.Category = message.Category;
            log.ControlServiceAddress = message.ControlAddress;
            log.ChannelId = channelId;
            log.DeployAddress = message.DeployAddress;
            log.Services.Clear();
            AddLog(message, log);
            return true;
        }

        /// <summary>
        ///     心跳
        /// </summary>
        /// <param name="message">心跳请求消息</param>
        /// <returns>返回心跳的结果</returns>
        public static bool Heartbeat(DSCHeartBeatRequestMessage message)
        {
            MachineLog log;
            if (_services.TryGetValue(message.MachineName, out log))
            {
                log.MachineLastHeartbeatTime = DateTime.Now;
                if (log.PerformanceItems != null)
                {
                    foreach (ServicePerformanceItem servicePerformanceItem in message.PerformanceItems)
                    {
                        ServicePerformanceItem performanceItem = log.PerformanceItems[servicePerformanceItem.PerformanceName];
                        if (performanceItem == null)
                        {
                            performanceItem = new ServicePerformanceItem
                                                  {
                                                      PerformanceName = servicePerformanceItem.PerformanceName,
                                                      PerformanceValue = servicePerformanceItem.PerformanceValue
                                                  };
                            log.PerformanceItems.Add(performanceItem.PerformanceName ,performanceItem);
                        }
                        else
                        {
                            performanceItem.PerformanceValue = servicePerformanceItem.PerformanceValue;
                        }
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        ///     更新组件的健康状态
        /// </summary>
        /// <param name="responseMessage">获取组件健康状态回馈信息包</param>
        public static void Update(DSCGetComponentHealthResponseMessage responseMessage)
        {
            try
            {
                MachineLog machineLog;
                if (!_services.TryGetValue(responseMessage.MachineName, out machineLog))
                {
                    return;
                }
                machineLog.Update(responseMessage);
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
            }
        }

        /// <summary>
        ///     更新指定被控主机内的服务信息
        /// </summary>
        /// <param name="responseMessage">服务详细信息包</param>
        internal static void Update(DSCGetServicesResponseMessage responseMessage)
        {
            try
            {
                MachineLog machineLog;
                if (!_services.TryGetValue(responseMessage.MachineName, out machineLog))
                {
                    return;
                }
                machineLog.Update(responseMessage);
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
            }
        }

        /// <summary>
        ///     状态变更通知
        /// </summary>
        /// <param name="msg">状态变更通知消息</param>
        internal static void Update(DSCStatusChangeRequestMessage msg)
        {
            MachineLog machineLog;
            if (!_services.TryGetValue(msg.MachineName, out machineLog))
            {
                return;
            }
            machineLog.Update(msg);
        }

        /// <summary>
        ///     更新提示消息包
        /// </summary>
        /// <param name="msg">更新提示消息包</param>
        internal static void UpdateProcess(DSCUpdateProcessingMessage msg)
        {
            Console.WriteLine("Updating processing #Service: " + msg.ServiceName + ", Component: " + msg.ComponentName + ", Content: " + msg.Content);
        }

        /// <summary>
        ///     注销服务
        /// </summary>
        /// <param name="msg">注销服务请求包</param>
        internal static void UnRegist(DSCUnRegistRequestMessage msg)
        {
            try
            {
                MachineLog log;
                _services.TryRemove(msg.MachineName, out log);
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
            }
        }

        /// <summary>
        ///      因为SMC异常断开而产生的注销操作
        /// </summary>
        /// <param name="channelId">断开的通道编号</param>
        internal static void UnRegist(Guid channelId)
        {
            try
            {
                string key = null;
                MachineLog log;
                foreach (KeyValuePair<string, MachineLog> pair in _services)
                {
                    if (pair.Value.ChannelId == channelId)
                    {
                        key = pair.Key;
                    }
                }
                if (key != null)
                {
                    //remove successed.
                    if(_services.TryRemove(key, out log))
                    {
                        ConsoleHelper.PrintLine("#SMC disconnected! #key = " + key, ConsoleColor.DarkYellow);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
            }
        }

        /// <summary>
        ///     加入一个新的记录
        /// </summary>
        /// <param name="message">请求消息</param>
        /// <param name="log">主机记录项</param>
        private static void AddLog(DSCRegistRequestMessage message, MachineLog log)
        {
            if (message.OwnServics != null)
            {
                foreach (OwnServiceItem item in message.OwnServics)
                {
                    DynamicServiceLog serviceLog = new DynamicServiceLog();
                    serviceLog.LastHeartbeatTime = DateTime.Now;
                    serviceLog.ServiceName = item.ServiceName;
                    serviceLog.Name = item.Name;
                    serviceLog.Description = item.Description;
                    serviceLog.Version = item.Version;
                    serviceLog.ControlServiceAddress = item.ControlServiceAddress;
                    serviceLog.ComponentCount = item.ComponentCount;
                    serviceLog.LastHeartbeatTime = item.LastHeartbeatTime;
                    serviceLog.LastUpdateTime = item.LastUpdateTime;
                    if (item.Componnets.Length > 0)
                    {
                        for (int i = 0; i < item.Componnets.Length; i++)
                        {
                            OwnComponentItem orgItem = item.Componnets[i];
                            OwnComponentItem componentItem = new OwnComponentItem();
                            componentItem.ComponentName = orgItem.ComponentName;
                            componentItem.Description = orgItem.Description;
                            componentItem.Version = orgItem.Version;
                            componentItem.Name = orgItem.Name;
                            componentItem.LastUpdateTime = orgItem.LastUpdateTime;
                            componentItem.Status = orgItem.Status;
                            serviceLog.Componnets.Add(componentItem.ComponentName, componentItem);
                        }
                    }
                    log.Services.Add(serviceLog.ServiceName, serviceLog);
                }
            }
        }

        /// <summary>
        ///     返回包含指定服务名的被控制机信道编号
        /// </summary>
        /// <param name="machineName">机器名称</param>
        /// <param name="serviceName">服务名称</param>
        /// <returns>返回信道编号</returns>
        internal static List<Guid> GetServiceOnMachine(string machineName, string serviceName)
        {
            List<Guid> guids = new List<Guid>();
            #region 判断对于SMC消息的转换

            if (serviceName == "*SMC*")
            {
                if (machineName == null)
                {
                    Logs.Logger.Log("Can not find a target machine, #flag: \"*SMC*\" #machine code: NONE");
                    return guids;
                }
                if (machineName == "*ALL*")
                {
                    if (_services.Count > 0)
                    {
                        guids.AddRange(_services.Values.Select(value => value.ChannelId));
                    }
                }
                else
                {
                    MachineLog machineLog;
                    if (_services.TryGetValue(machineName, out machineLog))
                    {
                        guids.Add(machineLog.ChannelId);
                    }
                    else
                    {
                        Logs.Logger.Log("Can not find a target machine, #flag: \"*SMC*\" #machine code: " + machineName);
                    }
                }
                return guids;
            }

            #endregion

            if (machineName == "*ALL*" && serviceName == "*ALL*")
            {
                if (_services.Count > 0)
                {
                    guids.AddRange(_services.Values.Select(value => value.ChannelId));
                }
                return guids;
            }
            foreach (KeyValuePair<string, MachineLog> pair in _services)
            {
                if ((machineName == null || pair.Key == machineName || pair.Key == "*ALL*") && pair.Value.ContainService(serviceName))
                {
                    guids.Add(pair.Value.ChannelId);
                }
            }
            return guids;
        }

        /// <summary>
        ///     获取当前已注册的所有部署节点信息
        /// </summary>
        /// <returns>返回部署节点信息集合</returns>
        internal static OwnDeployNodeItem[] GetDeployNodes()
        {
            if (_services.Count == 0)
            {
                return null;
            }
            List<OwnDeployNodeItem> items = new List<OwnDeployNodeItem>();
            foreach (KeyValuePair<string, MachineLog> pair in _services)
            {
                if (pair.Value.Category == "DSN")
                {
                    items.Add(new OwnDeployNodeItem {MachineName = pair.Key, DeployAddress = pair.Value.DeployAddress});
                }
            }
            return items.Count == 0 ? null : items.ToArray();
        }

        /// <summary>
        ///     获取核心服务信息
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        internal static CoreServiceItem[] GetCoreService(string category)
        {
            if (category != "*SERVICE*")
            {
                return InnerGetCoreService(category);
            }
            return InnerGetCoreService();
        }

        /// <summary>
        ///     内部获取核心服务信息
        /// </summary>
        /// <param name="category">服务分类</param>
        /// <returns>返回核心服务信息</returns>
        private static CoreServiceItem[] InnerGetCoreService(string category)
        {
            var result = _services.Where(pair => pair.Value.Category == category);
            int count = result.Count();
            if (count <= 0)
            {
                return null;
            }
            int offset = 0;
            CoreServiceItem[] items = new CoreServiceItem[count];
            foreach (KeyValuePair<string, MachineLog> pair in result)
            {
                CoreServiceItem item = new CoreServiceItem();
                item.Category = category;
                item.MachineName = pair.Key;
                item.ControlAddress = pair.Value.ControlServiceAddress;
                item.DeployAddress = pair.Value.DeployAddress;
                item.LastHeartBeatTime = pair.Value.MachineLastHeartbeatTime;
                items[offset++] = item;
            }
            return items;
        }
        /// <summary>
        ///     内部获取核心服务信息
        /// </summary>
        /// <returns>返回核心服务信息</returns>
        private static CoreServiceItem[] InnerGetCoreService()
        {
            int count = _services.Count;
            if (count <= 0)
            {
                return null;
            }
            int offset = 0;
            CoreServiceItem[] items = new CoreServiceItem[count];
            foreach (KeyValuePair<string, MachineLog> pair in _services)
            {
                CoreServiceItem item = new CoreServiceItem();
                item.Category = pair.Value.Category;
                item.MachineName = pair.Key;
                item.ControlAddress = pair.Value.ControlServiceAddress;
                item.DeployAddress = pair.Value.DeployAddress;
                item.LastHeartBeatTime = pair.Value.MachineLastHeartbeatTime;
                if (pair.Value.Services.Count > 0)
                {
                    item.Services = new OwnServiceItem[pair.Value.Services.Count];
                    int innerOffset = 0;
                    foreach (KeyValuePair<string, DynamicServiceLog> servicePair in pair.Value.Services)
                    {
                        OwnServiceItem ownServiceItem = new OwnServiceItem();
                        ownServiceItem.ComponentCount = servicePair.Value.ComponentCount;
                        ownServiceItem.ControlServiceAddress = servicePair.Value.ControlServiceAddress;
                        ownServiceItem.Description = servicePair.Value.Description;
                        ownServiceItem.Version = servicePair.Value.Version;
                        ownServiceItem.ServiceName = servicePair.Key;
                        ownServiceItem.LastHeartbeatTime = servicePair.Value.LastHeartbeatTime;
                        ownServiceItem.LastUpdateTime = servicePair.Value.LastUpdateTime;
                        ownServiceItem.Name = servicePair.Value.Name;
                        ownServiceItem.Componnets = servicePair.Value.Componnets.Count > 0 ? servicePair.Value.Componnets.Values.ToArray() : null;
                        item.Services[innerOffset++] = ownServiceItem;
                    }
                }
                items[offset++] = item;
            }
            return items;
        }

        #endregion

        //for test

        public static void NotifyResetHeartbeat(int interval)
        {
            DSCResetHeartBeatTimeRequestMessage msg = new DSCResetHeartBeatTimeRequestMessage();
            msg.Target = "DSN";
            msg.Interval = interval;
            msg.Bind();
            var resut = _services.Where(services => services.Value.Category == "DSN");
            if (resut != null)
            {
                foreach (var machine in resut)
                {
                    ITransportChannel channel = Global.CommnicationNode.GetTransportChannel(machine.Value.ChannelId);
                    if (channel != null)
                    {
                        ConsoleHelper.PrintLine("#正在发送重置心跳间隔到机器: " + machine.Value.MachineName + "......", ConsoleColor.DarkGreen);
                        channel.Send(msg.Body);
                    }
                }
            }
        }
    }
}