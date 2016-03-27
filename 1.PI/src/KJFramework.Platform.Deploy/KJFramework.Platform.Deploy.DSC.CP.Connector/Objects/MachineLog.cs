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
        ///     ��������Ľ���״̬
        /// </summary>
        /// <param name="responseMessage">��ȡ�������״̬������Ϣ��</param>
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
        ///     ����ָ�����������ڵķ�����Ϣ
        /// </summary>
        /// <param name="responseMessage">������ϸ��Ϣ��</param>
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
        ///     �ڲ�����һ������ķ���
        /// </summary>
        /// <param name="ownServiceItem">����������</param>
        /// <param name="log">ԭ�з�����</param>
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
        ///     �ڲ����һ���·���ķ���
        /// </summary>
        /// <param name="ownServiceItem">����������</param>
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
        ///     ״̬���֪ͨ
        /// </summary>
        /// <param name="msg">״̬���֪ͨ��Ϣ</param>
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
        ///     ��鵱ǰ�������Ƿ�����������
        /// </summary>
        /// <param name="serviceName">��������</param>
        /// <exception cref="ArgumentNullException">����Ϊ��</exception>
        /// <returns>�����Ƿ����Ľ��</returns>
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