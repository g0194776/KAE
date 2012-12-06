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
    ///     �����������ṩ�˶��ڿ��ŷ���Ļ���֧�֡�
    /// </summary>
    public class ServiceHost : CommunicationObject, IMetadataExchange
    {
        #region ��Ա

        private IServiceHandle _serviceHandle;
        private bool _hasPerformanceCounters;
        private bool _needPerformanceCounter;
        
        #endregion

        #region ���캯��

        /// <summary>
        ///     �����������ṩ�˶��ڿ��ŷ���Ļ���֧�֡�
        /// </summary>
        /// <param name="binding">������</param>
        /// <param name="service">�ڲ�����</param>
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
        ///     ֹͣ
        /// </summary>
        public override void Abort()
        {
            Close();
        }

        /// <summary>
        ///     ��
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
        ///     �ر�
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
        ///     ����һ�����ڵ�ǰ����ڵ����Լ����
        /// </summary>
        /// <returns>������Լ����</returns>
        public IContractDescription CreateDescription()
        {
            return _serviceHandle.CreateDescription();
        }

        /// <summary>
        ///      ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ����ڵ��Ƿ�֧����ԼԪ���ݽ���
        /// </summary>
        public bool IsSupportExchange
        {
            get { return _serviceHandle.IsSupportExchange; }
            set { _serviceHandle.IsSupportExchange = value; }
        }

        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ��CONNECT.�����Ƿ���Ҫ���ܼ�����֧��
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

        #region ����

        /// <summary>
        ///     ����������¼�
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
        ///     �رշ��񣬲��رյ�ǰ���������ŵ�����ϵͳ��Դ
        /// </summary>
        public void Shutdown()
        {
            _serviceHandle.Shutdown();
        }

        #endregion

        #region �¼�

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