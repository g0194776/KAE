using System;
using KJFramework.ServiceModel.Proxy;

namespace KJFramework.ServiceModel.Bussiness.Default.Async
{
    /// <summary>
    ///     �첽�������������Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IAsyncMethodDelegatorController
    {
        /// <summary>
        ///     ��ȡһ���������첽�������������ǰ���������������е��б��У������Զ���ӡ�
        /// </summary>
        /// <param name="asyncMethodName">�첽����</param>
        /// <returns>����һ���첽����������</returns>
        IAsyncMethodDelegator this[String asyncMethodName] { get; }
        /// <summary>
        ///     ��ȡһ���������첽�������������ǰ���������������е��б��У������Զ���ӡ�
        /// </summary>
        /// <param name="asyncMethodName">�첽����</param>
        /// <returns>����һ���첽����������</returns>
        IAsyncMethodDelegator GetDelegator(String asyncMethodName);
        /// <summary>
        ///     ���һ������
        /// </summary>
        /// <param name="asyncMethodName">�첽��������</param>
        /// <param name="callback">�ص�����</param>
        void AddDelegate(String asyncMethodName, AsyncMethodCallback callback);
    }
}