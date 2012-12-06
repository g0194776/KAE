using System;
using System.Collections.Generic;
using KJFramework.Basic.Enum;
using KJFramework.Cache.Cores;
using KJFramework.Logger;
using KJFramework.Messages.Helpers;
using KJFramework.ServiceModel.Bussiness.Default.Counters;
using KJFramework.ServiceModel.Bussiness.Default.Messages;
using KJFramework.ServiceModel.Bussiness.Default.Objects;
using KJFramework.ServiceModel.Bussiness.Default.Services;
using KJFramework.ServiceModel.Bussiness.Default.Transactions;
using KJFramework.ServiceModel.Core.Objects;
using KJFramework.ServiceModel.Enums;
using KJFramework.ServiceModel.Metadata;
using KJFramework.Statistics;

namespace KJFramework.ServiceModel.Bussiness.Default.Dispatchers.Cores
{
    /// <summary>
    ///     ���ķַ��������࣬�ṩ����صĻ���������
    /// </summary>
    internal abstract class CoreDispatcher : ICoreDispatcher
    {
        #region Implementation of IStatisticable<IStatistic>

        protected Dictionary<StatisticTypes, IStatistic> _statistics = new Dictionary<StatisticTypes, IStatistic>();
        /// <summary>
        /// ��ȡ������ͳ����
        /// </summary>
        public Dictionary<StatisticTypes, IStatistic> Statistics
        {
            get { return _statistics; }
            set { _statistics = value; }
        }

        #endregion

        #region Implementation of ICoreDispatcher

        /// <summary>
        ///     �ַ�����
        /// </summary>
        /// <param name="serviceHandle">������</param>
        /// <param name="message">������Ϣ</param>
        /// <param name="channel">�ײ㴫��ͨ��</param>
        public void Dispatch(IServiceHandle serviceHandle, RPCTransaction rpcTrans)
        {
            ServiceReturnValue serviceReturnValue;
            HostService hostService = serviceHandle.GetService();
            ServiceMethodPickupObject methodPickupObjects;
            IFixedCacheStub<ServiceReturnValue> fixedCacheStub = null;
            object instance = null;
            RequestMethodObject requestObj = ((RequestServiceMessage)rpcTrans.Request).RequestObject;
            try
            {
                //get callee method.
                methodPickupObjects = hostService.GetMethod(requestObj.MethodToken);
                if (methodPickupObjects == null)
                    throw new System.Exception("Illegal service request, current method non-existed. #token: " + requestObj.MethodToken.ToString());
                IBinaryArgContext[] binaryArgContexts = null;
                if (requestObj.Context != null && requestObj.Context.Length > 0) binaryArgContexts = requestObj.GetArgs();
                InitServiceMethodPickupObject(methodPickupObjects,hostService);
                fixedCacheStub = ServiceModel.FixedReturnValues.Rent();
                serviceReturnValue = fixedCacheStub.Cache;
                serviceReturnValue.HasReturnValue = methodPickupObjects.Method.HasReturnValue;
                Object retnValue;
                try
                {
                    Object[] args = null;
                    if (binaryArgContexts != null)
                    {
                        args = new Object[binaryArgContexts.Length];
                        for (int i = 0; i < binaryArgContexts.Length; i++)
                        {
                            args[i] = binaryArgContexts[i].Data == null ? null : DataHelper.GetObject(methodPickupObjects[i], binaryArgContexts[i].Data);
                        }
                    }
                    retnValue = methodPickupObjects.Method.Invoke(args);
                }
                catch (System.Exception ex)
                {
                    #region Inner call failed.

                    Logs.Logger.Log(ex);
                    ServiceModelPerformanceCounter.Instance.RateOfExecuteFailed.Increment();
                    if (!rpcTrans.Request.TransactionIdentity.IsOneway)
                    {
                        serviceReturnValue.HasReturnValue = false;
                        serviceReturnValue.ExceptionType = String.Format("{0}, {1}",
                                                                         ex.GetType().FullName,
                                                                         ex.GetType().Assembly.FullName);
                        serviceReturnValue.ExceptionMessage = ex.Message;
                        serviceReturnValue.ProcessResult = ServiceProcessResult.Exception;
                        RequestCallback(serviceReturnValue, rpcTrans);
                    }
                    return;

                    #endregion
                }
                //has return value.
                if (serviceReturnValue.HasReturnValue) serviceReturnValue.SetReturnValue(methodPickupObjects.Method.ReturnType, retnValue);
                ServiceModelPerformanceCounter.Instance.RateOfExecuteSuccessed.Increment();
                if (!rpcTrans.Request.TransactionIdentity.IsOneway)
                    RequestCallback(serviceReturnValue, rpcTrans);
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                ServiceModelPerformanceCounter.Instance.RateOfExecuteFailed.Increment();
            }
            finally
            {
                //�黹��Լ����ʵ��
                Giveback(hostService, instance, fixedCacheStub);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     ����ص�
        /// </summary>
        /// <param name="returnValue">����ֵ</param>
        /// <param name="channel">����ͨ��</param>
        /// <param name="sessionId">������Ϣ��Ψһ��ʾ</param>
        protected void RequestCallback(ServiceReturnValue returnValue, RPCTransaction rpcTrans)
        {
            /*
             * ע�⣬����ķ�����Ϣ���ݵ���һ���������󷵻�ֵ��������Ҫ������Ϣͷ�е�  Protocol Id��������
             * Ŀǰͨ����ע Protocol Id = 1 ����ʾ�Ƿ���ֵ��
             */
            IFixedCacheStub<ResponseServiceMessage> stub = null;
            try
            {
                stub = ServiceModel.FixedResponseMessage.Rent();
                ResponseServiceMessage serviceMessage = stub.Cache;
                serviceMessage.ServiceReturnValue = returnValue;
                rpcTrans.SendResponse(serviceMessage);
            }
            catch (System.Exception ex) { Logs.Logger.Log(ex); }
            finally
            {
                ServiceModelPerformanceCounter.Instance.RateOfCallback.Increment();
                if (stub != null) ServiceModel.FixedResponseMessage.Giveback(stub);
            }
        }

        /// <summary>
        ///     ��ɶ�����Դ�Ĺ黹
        /// </summary>
        /// <param name="hostService">��������</param>
        /// <param name="instance">�����ڲ�ʵ��</param>
        /// <param name="stub">������</param>
        protected void Giveback(HostService hostService, object instance, IFixedCacheStub<ServiceReturnValue> stub)
        {
            if (stub != null) ServiceModel.FixedReturnValues.Giveback(stub);
            if (hostService.Contract.ServiceConcurrentType != ServiceConcurrentTypes.Singleton)
                hostService.Giveback(instance);
        }

        protected virtual void InitServiceMethodPickupObject(ServiceMethodPickupObject instance, IHostService hostService)
        {
            instance.Method.Instance = hostService.NewServiceInstance();
        }

        #endregion
    }
}