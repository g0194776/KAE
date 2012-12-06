using System;
using KJFramework.Basic.Enum;
using KJFramework.Platform.Deploy.Metadata.Enums;
using KJFramework.Platform.Deploy.Metadata.Objects;
using KJFramework.Platform.Deploy.Metadata.Performances;
using KJFramework.Platform.Deploy.SMC.Common.Performances;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Objects
{
    /// <summary>
    ///     动态服务记录项
    /// </summary>
    public class DynamicServiceItem : IDynamicServiceItem
    {
        #region Implementation of IDynamicServiceItem

        private Guid _channelId;
        private string _serviceName;
        private string _version;
        private string _description;
        private string _name;
        private int _appDomainCount;
        private int _componentCount;
        private DateTime _lastHeartBeatTime;
        private string _lastError;
        private ServiceLiveStatus _liveStatus;
        private bool _supportDomainPerformance;
        private DateTime _lastUpdateTime;
        private ServicePerformanceItem[] _servicePerformanceItems;
        private DomainPerformanceItem[] _domainPerformanceItems;
        private ComponentHealthItem[] _componentHealthItems;
        private string _processName;
        private ComponentDetailItem[] _components;
        private string _shellVersion;

        /// <summary>
        ///     获取通道编号
        /// </summary>
        public Guid ChannelId
        {
            get { return _channelId; }
            set { _channelId = value; }
        }

        /// <summary>
        ///     获取服务名称
        /// </summary>
        public string ServiceName
        {
            get { return _serviceName; }
            set { _serviceName = value; }
        }

        /// <summary>
        ///     获取服务版本号
        /// </summary>
        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        /// <summary>
        ///     获取服务描述
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        ///     获取服务别名
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        ///     获取或设置外壳版本号
        /// </summary>
        public string ShellVersion
        {
            get { return _shellVersion; }
            set { _shellVersion = value; }
        }

        /// <summary>
        ///     获取或设置一个值，该值表示了当前动态服务是否支持程序与的性能捕获
        /// </summary>
        public bool SupportDomainPerformance
        {
            get { return _supportDomainPerformance; }
            set { _supportDomainPerformance = value; }
        }

        /// <summary>
        ///     获取或设置服务存活状态
        /// </summary>
        public ServiceLiveStatus LiveStatus
        {
            get { return _liveStatus; }
            set { _liveStatus = value; }
        }

        /// <summary>
        ///     获取或设置服务的应用程序域个数
        /// </summary>
        public int AppDomainCount
        {
            get { return _appDomainCount; }
            set { _appDomainCount = value; }
        }

        /// <summary>
        ///     获取或设置服务的组件个数
        /// </summary>
        public int ComponentCount
        {
            get { return _componentCount; }
            set { _componentCount = value; }
        }

        /// <summary>
        ///     获取或设置最后心跳时间
        /// </summary>
        public DateTime LastHeartBeatTime
        {
            get { return _lastHeartBeatTime; }
            set { _lastHeartBeatTime = value; }
        }

        /// <summary>
        ///     获取或设置最后更新时间
        /// </summary>
        public DateTime LastUpdateTime
        {
            get { return _lastUpdateTime; }
            set { _lastUpdateTime = value; }
        }

        /// <summary>
        ///     获取或设置最后错误信息
        /// </summary>
        public string LastError
        {
            get { return _lastError; }
            set { _lastError = value; }
        }

        /// <summary>
        ///     获取或设置进程名称
        /// </summary>
        public string ProcessName
        {
            get { return _processName; }
            set { _processName = value; }
        }

        /// <summary>
        ///     获取或设置组件的详细信息
        /// </summary>
        public ComponentDetailItem[] Components
        {
            get { return _components; }
            set { _components = value; }
        }

        /// <summary>
        ///     更新性能项
        /// </summary>
        /// <param name="items">性能项</param>
        public void Update(ServicePerformanceItem[] items)
        {
            _servicePerformanceItems = items;
        }

        /// <summary>
        ///     更新性能项
        /// </summary>
        /// <param name="items">性能项</param>
        public void Update(DomainPerformanceItem[] items)
        {
            _domainPerformanceItems = items;
        }

        /// <summary>
        ///     更新性能项
        /// </summary>
        /// <param name="items">性能项</param>
        public void Update(ComponentHealthItem[] items)
        {
            _componentHealthItems = items;
        }

        /// <summary>
        ///     设置组件更新结果项
        /// </summary>
        /// <param name="items">更新结果项</param>
        public void Update(ComponentUpdateResultItem[] items)
        {
            if (_components != null)
            {
                foreach (var updateItem in items)
                {
                    foreach (ComponentDetailItem detailItem in _components)
                    {
                        //compare component name.
                        if (updateItem.ComponentName == detailItem.ServiceName)
                        {
                            detailItem.LastUpdateTime = updateItem.LastUpdateTime;
                            //update failed.
                            if (!updateItem.Result)
                            {
                                detailItem.Status = HealthStatus.Death;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     获取服务相关性能项
        /// </summary>
        /// <returns>返回服务相关性能项</returns>
        public ServicePerformanceItem[] GetPerformances()
        {
            return _servicePerformanceItems;
        }

        /// <summary>
        ///     获取服务应用程序与相关性能项
        /// </summary>
        /// <returns>返回服务相关性能项</returns>
        public DomainPerformanceItem[] GetDomainPerformances()
        {
            return _domainPerformanceItems;
        }

        #endregion
    }
}