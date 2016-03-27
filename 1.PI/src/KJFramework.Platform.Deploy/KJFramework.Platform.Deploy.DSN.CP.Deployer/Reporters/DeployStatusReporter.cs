using System;
using KJFramework.Logger;
using KJFramework.Net.Channels;
using KJFramework.Platform.Deploy.DSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Reporters
{
    /// <summary>
    ///     部署状态汇报器，提供了相关的基本操作
    /// </summary>
    internal class DeployStatusReporter : IDeployStatusReporter
    {
        #region Constructor

        /// <summary>
        ///     部署状态汇报器，提供了相关的基本操作
        /// </summary>
        /// <param name="channelId">通道编号</param>
        /// <param name="requestToken">请求令牌</param>
        /// <param name="reportDetail">状态汇报标示</param>
        public DeployStatusReporter(Guid channelId, string requestToken, bool reportDetail)
        {
            if (channelId == Guid.Empty)
            {
                throw new ArgumentException("Can not create a new reporter, Illegal channel id.");
            }
            if (string.IsNullOrEmpty(requestToken))
            {
                throw new ArgumentNullException(requestToken);
            }
            _channelId = channelId;
            _requestToken = requestToken;
            _reportDetail = reportDetail;
            if (_reportDetail)
            {
                _transferChannel = Global.CommunicationNode.GetTransportChannel(channelId);
                if (_transferChannel == null)
                {
                    throw new System.Exception("Can not thought this channel id #" + channelId + " to get a transport channel.");
                }
            }
        }

        #endregion

        #region Members

        private ITransportChannel _transferChannel;
        private readonly bool _reportDetail;

        #endregion

        #region Implementation of IDeployStatusReporter

        private Guid _channelId;
        private string _requestToken;

        /// <summary>
        ///     获取与此汇报器相关联的通道编号
        /// </summary>
        public Guid ChannelId
        {
            get { return _channelId; }
        }

        /// <summary>
        ///     获取与此汇报器相关联的请求令牌
        /// </summary>
        public string RequestToken
        {
            get { return _requestToken; }
        }

        /// <summary>
        ///     将一个状态通知到远程终结点
        /// </summary>
        /// <param name="content">状态信息</param>
        /// <exception cref="System.Exception">通知失败</exception>
        public void Notify(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return;
            }
            Logs.Logger.Log("[Deploy Report]: " + content);
            if (_reportDetail)
            {
                if (!_transferChannel.IsConnected)
                {
                    throw new System.Exception("An available transfer channel has been disconnected.");
                }
                try
                {
                    DSNDeployStatusReportMessage reportMessage = new DSNDeployStatusReportMessage();
                    reportMessage.RequestToken = _requestToken;
                    reportMessage.CurrentStatus = content;
                    reportMessage.Bind();
                    _transferChannel.Send(reportMessage.Body);
                }
                catch (System.Exception ex)
                {
                    Logs.Logger.Log(ex);
                    throw;
                }
            }
        }

        /// <summary>
        ///     将一个错误信息通知到远程终结点
        /// </summary>
        /// <param name="exception">异常</param>
        /// <exception cref="System.Exception">通知失败</exception>
        public void Notify(System.Exception exception)
        {
            if (exception == null)
            {
                return;
            }
            Logs.Logger.Log(exception);
            Notify(exception.InnerException != null ? exception.InnerException.Message : exception.Message);
        }

        #endregion
    }
}