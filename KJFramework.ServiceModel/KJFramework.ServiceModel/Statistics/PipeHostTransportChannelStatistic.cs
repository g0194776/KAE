using KJFramework.EventArgs;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.HostChannels;
using KJFramework.Statistics;

namespace KJFramework.ServiceModel.Statistics
{
    public class PipeHostTransportChannelStatistic : Statistic
    {
        #region 成员

        private PipeHostTransportChannel _channel;

        #endregion

        #region 事件

        void ChannelCreated(object sender, LightSingleArgEventArgs<ITransportChannel> e)
        {

        }

        void Opened(object sender, System.EventArgs e)
        {
        }

        void Opening(object sender, System.EventArgs e)
        {

        }

        void Closed(object sender, System.EventArgs e)
        {

        }

        void Closing(object sender, System.EventArgs e)
        {

        }

        void Faulted(object sender, System.EventArgs e)
        {

        }

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