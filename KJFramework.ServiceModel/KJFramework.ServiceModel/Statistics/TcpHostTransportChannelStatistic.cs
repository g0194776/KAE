using KJFramework.Net.Channels.HostChannels;
using KJFramework.Statistics;

namespace KJFramework.ServiceModel.Statistics
{
    public class TcpHostTransportChannelStatistic : Statistic
    {
        #region ��Ա

        private TcpHostTransportChannel _channel;

        #endregion

        #region Overrides of Statistic

        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="element">ͳ������</param><typeparam name="T">ͳ������</typeparam>
        public override void Initialize<T>(T element)
        {
        }

        /// <summary>
        /// �ر�ͳ��
        /// </summary>
        public override void Close()
        {
            _channel = null;
        }

        #endregion
    }
}