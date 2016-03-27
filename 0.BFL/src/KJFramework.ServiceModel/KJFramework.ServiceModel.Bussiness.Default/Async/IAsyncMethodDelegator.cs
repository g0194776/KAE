using System;
using KJFramework.ServiceModel.Proxy;

namespace KJFramework.ServiceModel.Bussiness.Default.Async
{
    /// <summary>
    ///     �첽����������Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IAsyncMethodDelegator
    {
        /// <summary>
        ///     ��ȡ�˴������󶨵����첽��������
        /// </summary>
        String AsyncMethodName { get; }
        /// <summary>
        ///     ���һ����ʱ����
        /// </summary>
        /// <param name="callback">�ص�����</param>
        void AddDelegate(AsyncMethodCallback callback);
        /// <summary>
        ///     ���ݻỰId����ȡָ���Ļص�����
        /// </summary>
        /// <param name="sessionId">�ỰId</param>
        /// <returns>���ػص�����</returns>
        AsyncMethodCallback GetDelegate(int sessionId);
        /// <summary>
        ///     ��һ���ص������󶨵�һ��ָ���ĻỰId��
        ///     <para>* ע�⣺ʹ�ô˺���Ӧ�������AddDelegate������ͬһ���߳��ϡ�</para>
        /// </summary>
        /// <param name="sessionId">�ỰId</param>
        /// <returns>���ذ󶨵Ľ��</returns>
        bool Bind(int sessionId);
        /// <summary>
        ///     ���������ڲ��ȴ��Ļص�����
        ///     <para>* ���ô˷������ڲ�����ָ�����д洢�Ļص���������ʹ�ó�ʱ������ʧ�ܵ�ԭ��</para>
        /// </summary>
        void DiscardAllOperationForTimeout();
        /// <summary>
        ///     ��ȡ���һ�θ��µ�ʱ��
        /// </summary>
        DateTime LastUpdateTime { get; }
    }
}