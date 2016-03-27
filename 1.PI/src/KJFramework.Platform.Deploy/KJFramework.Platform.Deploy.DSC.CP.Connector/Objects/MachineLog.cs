using System;
using System.Collections.Generic;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;
using KJFramework.Platform.Deploy.Metadata.Enums;
using KJFramework.Platform.Deploy.Metadata.Objects;
using KJFramework.Platform.Deploy.SMC.Common.Performances;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector.Objects
{
    public class MachineLog
    {
        #region Constructor

        public MachineLog()
        {
            Services = new Dictionary<string, DynamicServiceLog>();
        }

        #endregion

        #region Members

        public string MachineName { get; set; }
        public Dictionary<string, DynamicServiceLog> Services { get; set; }
        public string Category { get; set; }
        public DateTime MachineLastHeartbeatTime { get; set; }
        public Dictionary<string, ServicePerformanceItem> PerformanceItems { get; set; }
        public string ControlServiceAddress { get; set; }
        public Guid ChannelId { get; set; }
        public string DeployAddress { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     更新组件的健康状态
        /// </summary>
        /// <param name="responseMessage">获取组件健康状态回馈信息包</param>
        internal void Update(DSCGetComponentHealthResponseMessage responseMessage)
        {
            DynamicServiceLog log;
            if (Services == null || !Services.TryGetValue(responseMessage.ServiceName, out log))
            {
                return;
            }
            log.Update(responseMessage);
        }

        /// <summary>
        ///     更新指定被控主机内的服务信息
        /// </summary>
        /// <param name="responseMessage">服务详细信息包</param>
        internal void Update(DSCGetServicesResponseMessage responseMessage)
        {
            if (Services == null || responseMessage.Items == null)
            {
                return;
            }
            foreach (OwnServiceItem ownServiceItem in responseMessage.Items)
            {
                DynamicServiceLog log;
                //New service log.
                if (!Services.TryGetValue(ownServiceItem.ServiceName, out log))
                {
                    InnerAddService(ownServiceItem);
                    continue;
                }
                //Update value.
                InnerUpdateItem(ownServiceItem, log);
            }
        }

        /// <summary>
        ///     内部更新一个服务的方法
        /// </summary>
        /// <param name="ownServiceItem">服务描述项</param>
        /// <param name="log">原有服务项</param>
        private void InnerUpdateItem(OwnServiceItem ownServiceItem, DynamicServiceLog log)
        {
            log.ComponentCount = ownServiceItem.ComponentCount;
            log.ControlServiceAddress = ownServiceItem.ControlServiceAddress;
            log.Description = ownServiceItem.Description;
            log.Version = ownServiceItem.Version;
            log.Name = ownServiceItem.Name;
            log.LastHeartbeatTime = ownServiceItem.LastHeartbeatTime;
            log.LastUpdateTime = ownServiceItem.LastUpdateTime;
            log.ShellVersion = ownServiceItem.ShellVersion;
            if (ownServiceItem.Componnets != null)
            {
                if (log.Componnets == null)
                {
                    log.Componnets = new Dictionary<string, OwnComponentItem>();
                }
                foreach (OwnComponentItem componentItem in ownServiceItem.Componnets)
                {
                    OwnComponentItem item;
                    //Not item......add.
                    if (!log.Componnets.TryGetValue(componentItem.ComponentName, out item))
                    {
                        log.Componnets.Add(componentItem.ComponentName,
                                           new OwnComponentItem
                                               {
                                                   ComponentName = componentItem.ComponentName,
                                                   Name = componentItem.Name,
                                                   Version = componentItem.Version,
                                                   Description = componentItem.Description,
                                                   Status = componentItem.Status,
                                                   LastUpdateTime = componentItem.LastUpdateTime
                                               });
                        continue;
                    }
                    //Update value.
                    item.Name = componentItem.Name;
                    item.Version = componentItem.Version;
                    item.Description = componentItem.Description;
                    item.Status = componentItem.Status;
                    item.LastUpdateTime = componentItem.LastUpdateTime;
                }
            }
            else
            {
                log.Componnets = null;
            }
        }

        /// <summary>
        ///     内部添加一个新服务的方法
        /// </summary>
        /// <param name="ownServiceItem">服务描述项</param>
        private void InnerAddService(OwnServiceItem ownServiceItem)
        {
            DynamicServiceLog log;
            log = new DynamicServiceLog
                      {
                          ServiceName = ownServiceItem.ServiceName,
                          ComponentCount = ownServiceItem.ComponentCount,
                          ControlServiceAddress = ownServiceItem.ControlServiceAddress,
                          Description = ownServiceItem.Description,
                          Version = ownServiceItem.Version,
                          Name = ownServiceItem.Name,
                          LastHeartbeatTime = ownServiceItem.LastHeartbeatTime,
                          LastUpdateTime = ownServiceItem.LastUpdateTime,
                          ShellVersion = ownServiceItem.ShellVersion
                      };
            if (ownServiceItem.Componnets != null)
            {
                log.Componnets = new Dictionary<string, OwnComponentItem>();
                foreach (OwnComponentItem componentItem in ownServiceItem.Componnets)
                {
                    log.Componnets.Add(componentItem.ComponentName,
                                       new OwnComponentItem
                                           {
                                               ComponentName = componentItem.ComponentName,
                                               Name = componentItem.Name,
                                               Version = componentItem.Version,
                                               Description = componentItem.Description,
                                               Status = componentItem.Status,
                                               LastUpdateTime = componentItem.LastUpdateTime
                                           });
                }
            }
            Services.Add(log.ServiceName, log);
        }

        /// <summary>
        ///     状态变更通知
        /// </summary>
        /// <param name="msg">状态变更通知消息</param>
        internal void Update(DSCStatusChangeRequestMessage msg)
        {
            if (msg.Items != null)
            {
                foreach (OwnServiceItem ownServiceItem in msg.Items)
                {
                    switch (msg.StatusChangeType)
                    {
                        case StatusChangeTypes.NewService:
                            if (Services != null)
                            {
                                DynamicServiceLog serviceLog;
                                //Add
                                if (!Services.TryGetValue(ownServiceItem.ServiceName, out serviceLog))
                                {
                                    InnerAddService(ownServiceItem);
                                }
                                //Update
                                else
                                {
                                    InnerUpdateItem(ownServiceItem, serviceLog);
                                }
                            }
                            break;
                        case StatusChangeTypes.UpdateService:
                            DynamicServiceLog log;
                            //Update
                            if (Services != null && Services.TryGetValue(ownServiceItem.ServiceName, out log))
                            {
                                InnerUpdateItem(ownServiceItem, log);
                            }
                            break;
                        case StatusChangeTypes.DelService:
                            if (Services != null)
                            {
                                //Del
                                Services.Remove(ownServiceItem.ServiceName);
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        ///     检查当前主机内是否包含这个服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <exception cref="ArgumentNullException">参数为空</exception>
        /// <returns>返回是否服务的结果</returns>
        internal bool ContainService(string serviceName)
        {
            if (serviceName == null)
            {
                throw new ArgumentNullException("serviceName");
            }
            if (Services.ContainsKey(serviceName))
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}