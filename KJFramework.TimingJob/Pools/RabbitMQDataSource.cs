using System;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Topology;
using KJFramework.TimingJob.EventArgs;
using KJFramework.TimingJob.Formatters;
using Newtonsoft.Json.Linq;

namespace KJFramework.TimingJob.Pools
{
    /// <summary>
    ///    基于RabbitMQ构建的远程消息队列数据源
    /// </summary>
    public sealed class RabbitMQDataSource : IRemoteDataSource
    {
        #region Constructor.

        /// <summary>
        ///    基于RabbitMQ构建的远程消息队列数据源
        /// </summary>
        /// <param name="connectionStr">远程RabbitMQ连接串</param>
        /// <param name="exchange">远程RabbitMQ Exchange名称</param>
        /// <param name="exchangeType">远程RabbitMQ Exchange类型</param>
        /// <param name="queueName">远程RabbitMQ 队列名称</param>
        /// <param name="routingKey">远程RabbitMQ路由KEY</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public RabbitMQDataSource(string connectionStr, string exchange, string exchangeType, string queueName, string routingKey)
        {
            if (string.IsNullOrEmpty(connectionStr)) throw new ArgumentNullException(nameof(connectionStr));
            if (string.IsNullOrEmpty(exchange)) throw new ArgumentNullException(nameof(exchange));
            if (string.IsNullOrEmpty(exchangeType)) throw new ArgumentNullException(nameof(exchangeType));
            if (string.IsNullOrEmpty(queueName)) throw new ArgumentNullException(nameof(queueName));
            if (string.IsNullOrEmpty(routingKey)) throw new ArgumentNullException(nameof(routingKey));
            _bus = RabbitHutch.CreateBus(connectionStr, x => x.Register<IMessageSerializationStrategy>(_ => new GeneralSerializationStrategy()).Register(_ => RabbitMQLogger.Instance));
            _exchange = _bus.Advanced.ExchangeDeclare(exchange, exchangeType);
            _queue = _bus.Advanced.QueueDeclare(queueName, true);
            _bus.Advanced.Bind(_exchange, new Queue(queueName, false), routingKey);
        }

        #endregion

        #region Members.

        private readonly IBus _bus;
        private readonly IQueue _queue;
        private IExchange _exchange;
        private IDisposable _consumptionHandler;

        #endregion

        #region Methods.

        /// <summary>
        ///    开始数据接收
        /// </summary>
        public void Open()
        {
            //_consumptionHandler = _bus.Advanced.Consume(_queue, (IMessage<JObject> message, MessageReceivedInfo info) =>
            //    {
            //        Task.Factory.StartNew(() => OnDataReceived(new DataRecvEventArgs(message.Body)));
            //    });
            _consumptionHandler = _bus.Advanced.Consume(_queue, (IMessage<JObject> message, MessageReceivedInfo info) => OnDataReceived(new DataRecvEventArgs(message.Body)));
        }

        /// <summary>
        ///    暂停数据接收
        /// </summary>
        public void Pause()
        {
            if (_consumptionHandler != null)
            {
                _consumptionHandler.Dispose();
                _consumptionHandler = null;
            }
        }

        /// <summary>
        ///    停止数据接收，并回收内部所有资源
        /// </summary>
        public void Close()
        {
            Pause();
            _bus.Dispose();
        }

        /// <summary>
        ///    将指定数据发送到远程数据源上
        /// </summary>
        /// <param name="data">需要发送的数据</param>
        /// <param name="args">
        ///     数据参数
        ///     <para>* Index 0: Routing Key</para>
        /// </param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="ArgumentException">参数格式错误</exception>
        public Task Send(byte[] data, params object[] args)
        {
            string routingKey = args[0].ToString();
            bool mandatory = (bool)((args.Length != 0 && args.Length >= 2) ? args[1] : false);
            bool immediate = (bool)((args.Length != 0 && args.Length >= 3) ? args[2] : false);
            return _bus.Advanced.PublishAsync(_exchange, routingKey, mandatory, immediate, new MessageProperties(), data);
        }

        #endregion

        #region Events.

        /// <summary>
        ///    接收到数据后的事件通知
        /// </summary>
        public event EventHandler<DataRecvEventArgs> DataReceived;

        private void OnDataReceived(DataRecvEventArgs e)
        {
            EventHandler<DataRecvEventArgs> handler = DataReceived;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}