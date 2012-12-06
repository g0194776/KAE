using KJFramework.Net.Channels.HostChannels;
using KJFramework.Statistics;

namespace KJFramework.ServiceModel.Statistics
{
    public class TcpHostTransportChannelStatistic : Statistic
    {
        #region 成员

        private TcpHostTransportChannel _channel;

        #endregion

        #region Overrides of Statistic

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="element">统计类型</param><typeparam name="T">统计类型</typeparam>
        public override void Initialize<T>(T element)
        {
        }

        /// <summary>
        /// 关闭统计
        /// </summary>
        public override void Close()
        {
            _channel = null;
        }

        #endregion
    }
}