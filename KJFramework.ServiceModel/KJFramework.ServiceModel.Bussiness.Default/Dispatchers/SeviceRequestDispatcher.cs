using System;
using KJFramework.ServiceModel.Bussiness.Default.Dispatchers.Cores;
using KJFramework.ServiceModel.Bussiness.Default.Messages;
using KJFramework.ServiceModel.Bussiness.Default.Services;
using KJFramework.ServiceModel.Enums;

namespace KJFramework.ServiceModel.Bussiness.Default.Dispatchers
{
    /// <summary>
    ///     ��������ַ������ṩ����صĻ���������
    /// </summary>
    internal class SeviceRequestDispatcher
    {
        #region Implementation of IServiceRequestDispatcher

        /// <summary>
        ///     �ַ�����
        ///     <para>* �˲���������ݻ�����������ͽ����ض���ʽ������ַ���</para>
        /// </summary>
        /// <param name="serviceHandle">���������</param>
        /// <param name="message">����</param>
        /// <param name="key">����ͨ��Ψһ��ʾ</param>
        public static void Dispatch(ServiceHandle serviceHandle, RequestServiceMessage message, Guid key)
        {
            switch (serviceHandle.GetService().Contract.ServiceConcurrentType)
            {
                case ServiceConcurrentTypes.Concurrent:
                    using (ICoreDispatcher dispatcher = new ConcurrentCoreDispatcher())
                    {
                        dispatcher.Dispatch(serviceHandle, message, key);
                    }
                    break;
                case ServiceConcurrentTypes.Multi:
                    using (ICoreDispatcher dispatcher = new ThreadCoreDispatcher())
                    {
                        dispatcher.Dispatch(serviceHandle, message, key);
                    }
                    break;
                case ServiceConcurrentTypes.Singleton:
                    using (ICoreDispatcher dispatcher = new SingletonCoreDispatcher())
                    {
                        dispatcher.Dispatch(serviceHandle, message, key);
                    }
                    break;
            }
        }

        #endregion
    }
}