using System;
using KJFramework.EventArgs;
using KJFramework.Net.Cloud.Schedulers;
using KJFramework.Net.Cloud.Virtuals.Processors;

namespace KJFramework.Net.Cloud.Virtuals
{
    /// <summary>
    ///     ������ΪԪ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    public interface IPuppetBehavior<T>
    {
        /// <summary>
        ///     ����һ�����ܹ��ܴ�����
        /// </summary>
        /// <param name="processor">���ܹ��ܴ�����</param>
        /// <returns>��������</returns>
        PuppetNetworkNode<T> Attach(IPuppetFunctionProcessor processor);
        /// <summary>
        ///     ����һ���������������
        /// </summary>
        /// <param name="taskCount">��ʼ������������</param>
        /// <returns>���ش�����Ŀ������������</returns>
        IRequestScheduler<T> CreateScheduler(int taskCount = 100);
        /// <summary>
        ///     ���ӳɹ��¼�
        /// </summary>
        event EventHandler AttachSuccessed;
        /// <summary>
        ///     ����ʧ���¼�
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<string>> AttachFailed;
    }
}