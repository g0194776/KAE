using KJFramework.EventArgs;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.HostChannels;
using KJFramework.Statistics;

namespace KJFramework.ServiceModel.Statistics
{
    public class PipeHostTransportChannelStatistic : Statistic
    {
        #region ��Ա

        private PipeHostTransportChannel _channel;

        #endregion

        #region �¼�

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