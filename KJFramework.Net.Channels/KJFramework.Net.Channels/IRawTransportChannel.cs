using System;
using KJFramework.EventArgs;
using KJFramework.Net.Channels.Events;

namespace KJFramework.Net.Channels
{
    /// <summary>
    ///     �����������ŵ�Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    public interface IRawTransportChannel : ITransportChannel
    {
        /// <summary>
        ///     ��ȡ�����õ�ǰԪ�����ŵ��Ƿ�֧����Ƭ�εķ�ʽ��������������
        /// </summary>
        bool SupportSegment { get; set; }
        /// <summary>
        ///     ���յ������¼�
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<byte[]>> ReceivedData;
        /// <summary>
        ///     ���յ�����Ƭ���¼�
        /// </summary>
        event EventHandler<SegmentReceiveEventArgs> ReceivedDataSegment;
    }
}