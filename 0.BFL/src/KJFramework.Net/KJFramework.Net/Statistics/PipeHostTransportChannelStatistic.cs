using KJFramework.Net.HostChannels;
using KJFramework.Statistics;

namespace KJFramework.Net.Statistics
{
    internal class PipeHostTransportChannelStatistic : Statistic
    {
        #region ��Ա

        private PipeHostTransportChannel _channel;

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
            if (_channel != null)
            {
                _channel = null;
            }
        }

        #endregion
    }
}