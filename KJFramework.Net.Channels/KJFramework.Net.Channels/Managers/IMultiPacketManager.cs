using System;
using KJFramework.EventArgs;
using KJFramework.Net.Channels.Caches;

namespace KJFramework.Net.Channels.Managers
{
    /// <summary>
    ///     ���Ƭ������Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    /// <typeparam name="T">��Ϣ����</typeparam>
    public interface IMultiPacketManager<T> : IDisposable
    {
        /// <summary>
        ///     ���һ�����Ƭ
        /// </summary>
        /// <param name="key">Ψһ��ϢId</param>
        /// <param name="message">���Ƭ</param>
        /// <param name="maxPacketCount">
        ///     �����Ƭ��
        ///     <para>* ��һ�ε���ʱ���ô�ֵ���Ժ�Ĭ�ϴ�-1���ɡ�</para>
        /// </param>
        /// <returns>�������ֵ��Ϊnull, ��֤���Ѿ�ƴ��Ϊһ����������Ϣ</returns>
        T Add(int key, T message, int maxPacketCount = -1);
        /// <summary>
        ///     ���һ�����Ƭ
        /// </summary>
        /// <param name="key">Ψһ��ϢId</param>
        /// <param name="message">���Ƭ</param>
        /// <param name="timeSpan">����ʱ��</param>
        /// <param name="maxPacketCount">
        ///     �����Ƭ��
        ///     <para>* ��һ�ε���ʱ���ô�ֵ���Ժ�Ĭ�ϴ�-1���ɡ�</para>
        /// </param>
        /// <returns>�������ֵ��Ϊnull, ��֤���Ѿ�ƴ��Ϊһ����������Ϣ</returns>
        T Add(int key, T message, TimeSpan timeSpan, int maxPacketCount = -1);
        /// <summary>
        ///     �����Ϣ�����¼�
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<IMultiPacketStub<T>>> Expired;
    }
}