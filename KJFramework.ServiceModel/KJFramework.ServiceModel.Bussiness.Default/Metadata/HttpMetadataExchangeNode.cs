using System;
using System.Diagnostics;
using System.Text;
using KJFramework.Helpers;
using KJFramework.Net;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.HostChannels;
using KJFramework.ServiceModel.Bussiness.Default.Metadata.Actions;

namespace KJFramework.ServiceModel.Bussiness.Default.Metadata
{
    /// <summary>
    ///     ����HTTPЭ���Ԫ���ݽ�������ڵ㣬�ṩ����صĻ���������
    /// </summary>
    public class HttpMetadataExchangeNode : MetadataExchangeNode
    {
        #region Members

        private IHostTransportChannel _hostChannel;

        #endregion

        #region Overrides of MetadataExchangeNode

        /// <summary>
        ///     ����Ԫ���ݽ���
        /// </summary>
        public override void Start()
        {
            InitializeMetadataNode();
        }

        /// <summary>
        ///     ֹͣԪ���ݽ���
        /// </summary>
        public override void Stop()
        {
            if (!_isEnable) return;
            _hostChannel.ChannelCreated -= ChannelCreated;
            _hostChannel.UnRegist();
            _hostChannel = null;
            _isEnable = false;
        }

        /// <summary>
        ///     ��ʼ������Ԫ���ݽڵ�
        /// </summary>
        protected override void InitializeMetadataNode()
        {
            if (_isEnable) return;
            if (_hostChannel == null)
            {
                //direct a available port.
#if !MONO
                int port = NetworkDirecter.DirectPortEx(65300, 65535, NetworkDirecter.Protocol.TCP);
#else
                int port = 65300;
#endif
                if (port == -1)
                {
                    throw new System.Exception("#Can not found a tcp port for http metadata exchange node!");
                }
                HttpHostTransportChannel hostChannel = new HttpHostTransportChannel();
                hostChannel.Prefixes.Add(string.Format("http://+:{0}/", port));
                //show details at debug mode.
                if (Debugger.IsAttached)
                {
                    ConsoleHelper.PrintLine(string.Format("#TCP port: {0} used for http metadata exchange!", port), ConsoleColor.DarkGray);
                }
                _hostChannel = hostChannel;
                _hostChannel.ChannelCreated += ChannelCreated;
                _hostChannel.Regist();
            }
            _isEnable = true;
        }

        #endregion

        #region Events

        //channel create event.
        void ChannelCreated(object sender, EventArgs.LightSingleArgEventArgs<ITransportChannel> e)
        {
            IHttpTransportChannel transportChannel = (IHttpTransportChannel)e.Target;
            IHttpMetadataPageAction action;
            if (_actions.TryGetValue(transportChannel.RawUrl, out action))
            {
                string result = action.Execute(transportChannel.GetRequest());
                if (!string.IsNullOrEmpty(result))
                {
                    byte[] data = Encoding.UTF8.GetBytes(result);
                    transportChannel.Send(data);
                    return;
                }
            }
            //not found.
            transportChannel.StatusCode = 404;
            transportChannel.Send();
        }

        #endregion
    }
}