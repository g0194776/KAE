using System;
using KJFramework.EventArgs;
using KJFramework.Statistics;

namespace KJFramework.Net.Channels.Statistics
{
    internal class PipeTransportChannelStatistic : Statistic
    {
        #region ��Ա

        private PipeTransportChannel _channel;

        #endregion

        #region �¼�

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
        ///     ��ʼ��
        /// </summary>
        /// <param name="element">ͳ������</param><typeparam name="T">ͳ������</typeparam>
        public override void Initialize<T>(T element)
        {
            _channel = (PipeTransportChannel)((Object)element);
            _channel.Opened += Opened;
            _channel.Opening += Opening;
            _channel.Closing += Closing;
            _channel.Closed += Closed;
            _channel.Faulted += Faulted;
        }

        /// <summary>
        ///     �ر�ͳ��
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