using System;
using KJFramework.Net.Channels;

namespace KJFramework.Net.Cloud.Objects
{
    /// <summary>
    ///   ������Ϣ�����𴫵ݽ�����Ϣ��������Ĳ���
    /// </summary>
    public struct ReceivedMessageObject<T>
    {
        #region Members

        /// <summary>
        ///     ��ȡ�����ô����ŵ�
        /// </summary>
        public IMessageTransportChannel<T> Channel { get; set; }
        /// <summary>
        ///     ��ȡ����������ڵ���
        /// </summary>
        public Guid NodeId { get; set; }
        /// <summary>
        ///     ��ȡ�����ý��յ�����Ϣʵ��
        /// </summary>
        public T Message { get; set; }

        #endregion
    }
}