using System;
using KJFramework.Net.Channels.Enums;
using KJFramework.Statistics;

namespace KJFramework.Net.Channels
{
    /// <summary>
    ///     ͨѶ����Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface ICommunicationObject : IStatisticable<IStatistic>, IDisposable
    {
        /// <summary>
        ///     ֹͣ
        /// </summary>
        void Abort();
        /// <summary>
        ///     ��
        /// </summary>
        void Open();
        /// <summary>
        ///     �ر�
        /// </summary>
        void Close();
        /// <summary>
        ///     �첽��
        /// </summary>
        /// <param name="callback">�ص�����</param>
        /// <param name="state">״̬</param>
        /// <returns>�����첽���</returns>
        IAsyncResult BeginOpen(AsyncCallback callback, Object state);
        /// <summary>
        ///     �첽�ر�
        /// </summary>
        /// <param name="callback">�ص�����</param>
        /// <param name="state">״̬</param>
        /// <returns>�����첽���</returns>
        IAsyncResult BeginClose(AsyncCallback callback, Object state);
        /// <summary>
        ///     �첽��
        /// </summary>
        void EndOpen(IAsyncResult result);
        /// <summary>
        ///     �첽�ر�
        /// </summary>
        void EndClose(IAsyncResult result);
        /// <summary>
        ///     ��ȡ�����õ�ǰ����״̬
        /// </summary>
        bool Enable { get; set; }
        /// <summary>
        ///     ��ȡ��ǰͨѶ״̬
        /// </summary>
        CommunicationStates CommunicationState { get; }
        /// <summary>
        ///     �ѹر��¼�
        /// </summary>
        event EventHandler Closed;
        /// <summary>
        ///     ���ڹر��¼�
        /// </summary>
        event EventHandler Closing;
        /// <summary>
        ///     �Ѵ����¼�
        /// </summary>
        event EventHandler Faulted;
        /// <summary>
        ///     �ѿ����¼�
        /// </summary>
        event EventHandler Opened;
        /// <summary>
        ///     ���ڿ����¼�
        /// </summary>
        event EventHandler Opening;
    }
}