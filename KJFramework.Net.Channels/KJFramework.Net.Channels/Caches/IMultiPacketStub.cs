using System.Collections.Generic;

namespace KJFramework.Net.Channels.Caches
{
    /// <summary>
    ///     ���Ƭ���Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    /// <typeparam name="T">��Ϣ����</typeparam>
    public interface IMultiPacketStub<T>
    {
        /// <summary>
        ///     ��ȡ��ǰ������Ϣ�ı��
        /// </summary>
        int SessionId { get; }
        /// <summary>
        ///     ��ȡ�����Ƭ��Ŀ
        /// </summary>
        int MaxPacketCount { get; }
        /// <summary>
        ///     ���һ�����Ƭ
        /// </summary>
        /// <param name="message">���Ƭ��Ϣ</param>
        /// <returns>�������ֵ��Ϊfalse, ��֤���Ѿ�����һ����������Ϣ</returns>
        bool AddPacket(T message);
        /// <summary>
        ///     ��ȡ�ڲ����еķ��Ƭ
        /// </summary>
        /// <returns>���ط��Ƭ����</returns>
        IList<T> GetPackets();
    }
}