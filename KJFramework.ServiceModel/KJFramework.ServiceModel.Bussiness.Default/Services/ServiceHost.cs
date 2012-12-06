using System;
using KJFramework.Logger;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Enums;
using KJFramework.Net.Helper;
using KJFramework.ServiceModel.Bussiness.Default.Counters;
using KJFramework.ServiceModel.Bussiness.Default.Descriptions;
using KJFramework.ServiceModel.Elements;

namespace KJFramework.ServiceModel.Bussiness.Default.Services
{
    /// <summary>
    ///     服务宿主，提供了对于开放服务的基础支持。
    /// </summary>
    public class ServiceHost : CommunicationObject, IMetadataExchange
    {
        #region 成员

        private IServiceHandle _serviceHandle;
        private bool _hasPerformanceCounters;
        private bool _needPerformanceCounter;
        
        #endregion

        #region 构造函数

        /// <summary>
        ///     服务宿主，提供了对于开放服务的基础支持。
        /// </summary>
        /// <param name="binding">绑定类型</param>
        /// <param name="service">内部服务</param>
        public ServiceHost(Type service, params Binding[] binding)
        {
            ServiceModelPerformanceCounter.Instance.Initialize();
            if (binding == null) throw new ArgumentNullException("binding");
            if (service == null) throw new ArgumentNullException("service");
            NetHelper.Initialize();
            GlobalMemory.Initialize();
            ServiceModel.Initialize();
            _serviceHandle = new ServiceHandle(service, binding);
            _serviceHandle.Opened += ServiceHandleOpened;
            _serviceHandle.Closed += ServiceHandleClosed;
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        ///     停止
        /// </summary>
        public override void Abort()
        {
            Close();
        }

        /// <summary>
        ///     打开
        /// </summary>
        public override void Open()
        {
            try
            {
                _communicationState = CommunicationStates.Opening;
                _serviceHandle.Open();
                _communicationState = CommunicationStates.Opened;
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                _communicationState = CommunicationStates.Faulte;
                throw;
            }
        }

        /// <summary>
        ///     关闭
        /// </summary>
        public override void Close()
        {
            try
            {
                _communicationState = CommunicationStates.Closing;
                _serviceHandle.Close();
                CloseHandle();
                _communicationState = CommunicationStates.Closed;
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                _communicationState = CommunicationStates.Faulte;
                throw;
            }
        }

        #endregion

        #region Implementation of IMetadataExchange

        /// <summary>
        ///     创建一个关于当前服务节点的契约描述
        /// </summary>
        /// <returns>返回契约描述</returns>
        public IContractDescription CreateDescription()
        {
            return _serviceHandle.CreateDescription();
        }

        /// <summary>
        ///      获取一个值，该值表示了当前服务节点是否支持契约元数据交换
        /// </summary>
        public bool IsSupportExchange
        {
            get { return _serviceHandle.IsSupportExchange; }
            set { _serviceHandle.IsSupportExchange = value; }
        }

        /// <summary>
        ///     获取或设置一个值，该值标示了当前的CONNECT.服务是否需要性能计数器支持
        /// </summary>
        public bool NeedPerformanceCounter
        {
            get { return _needPerformanceCounter; }
            set 
            { 
                _needPerformanceCounter = value;
                if (_needPerformanceCounter && !_hasPerformanceCounters)
                {
                    _hasPerformanceCounters = true;
                }
            }
        }

        #endregion

        #region 方法

        /// <summary>
        ///     解除服务句柄事件
        /// </summary>
        protected void CloseHandle()
        {
            if (_serviceHandle == null) return;
            try
            {
                _serviceHandle.Opened -= ServiceHandleOpened;
                _serviceHandle.Closed -= ServiceHandleClosed;
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
            }
        }

        /// <summary>
        ///     关闭服务，并关闭当前服务所开放的所有系统资源
        /// </summary>
        public void Shutdown()
        {
            _serviceHandle.Shutdown();
        }

        #endregion

        #region 事件

        void ServiceHandleClosed(object sender, System.EventArgs e)
        {
            ClosedHandler(null);
        }

        void ServiceHandleOpened(object sender, System.EventArgs e)
        {
            OpenedHandler(null);
        }

        #endregion
    }
}