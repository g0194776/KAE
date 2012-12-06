using System;
using KJFramework.EventArgs;
using KJFramework.Net.Channels;
using KJFramework.Statistics;
namespace KJFramework.ServiceModel.Statistics
{
    public class TcpTransportChannelStatistic : Statistic
    {
        #region 成员

        private TcpTransportChannel _channel;

        #endregion

        #region 事件

        void SendFailed(object sender, LightSingleArgEventArgs<byte[]> e)
        {

        }

        void SendSuccessfully(object sender, LightSingleArgEventArgs<byte[]> e)
        {

        }

        void Closed(object sender, System.EventArgs e)
        {

        }

        void Closing(object sender, System.EventArgs e)
        {

        }

        void Opening(object sender, System.EventArgs e)
        {

        }

        void Opened(object sender, System.EventArgs e)
        {

        }

        void Faulted(object sender, System.EventArgs e)
        {

        }

        #endregion

        #region Overrides of Statistic

        /// <summary>
        ///     初始化
        /// </summary>
        /// <param name="element">统计类型</param><typeparam name="T">统计类型</typeparam>
        public override void Initialize<T>(T element)
        {
            _channel = (TcpTransportChannel) ((Object) element);
            _channel.Opened += Opened;
            _channel.Opening += Opening;
            _channel.Closing += Closing;
            _channel.Closed += Closed;
            _channel.Faulted += Faulted;
        }

        /// <summary>
        ///     关闭统计
        /// </summary>
        public override void Close()
        {
            if (_channel != null)
            {
                _channel.Opened -= Opened;
                _channel.Opening -= Opening;
                _channel.Closing -= Closing;
                _channel.Closed -= Closed;
                _channel.Faulted -= Faulted;
                _channel = null;
            }
        }

        #endregion
    }
}