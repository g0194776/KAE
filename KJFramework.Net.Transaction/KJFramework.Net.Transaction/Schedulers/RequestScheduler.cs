using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.Identities;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Net.Transaction.Objects;
using KJFramework.Net.Transaction.Processors;
using KJFramework.Tracing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace KJFramework.Net.Transaction.Schedulers
{
    /// <summary>
    ///     请求分发器，提供了相关的基本操作
    /// </summary>
    public class RequestScheduler : IRequestScheduler
    {
        #region Members

        private object _lockObj = new object();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (RequestScheduler));
        private readonly IList<IConnectionAgent> _agents = new List<IConnectionAgent>();
        private readonly ConcurrentDictionary<Protocols, IMessageProcessor> _processors = new ConcurrentDictionary<Protocols, IMessageProcessor>();

        #endregion

        #region Implementation of IRequestScheduler

        /// <summary>
        ///     注册一个连接代理器
        /// </summary>
        /// <param name="agent">连接代理器</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public void Regist(IServerConnectionAgent agent)
        {
            if (agent == null) throw new ArgumentNullException("agent");
            lock (_lockObj) _agents.Add(agent);
            agent.Disconnected += Disconnected;
            agent.NewTransaction += NewTransaction;
        }

        /// <summary>
        ///     注册一个消息处理器
        /// </summary>
        /// <param name="protocol">消息处理协议</param>
        /// <param name="processor">处理器</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public IRequestScheduler Regist(Protocols protocol, IMessageProcessor processor)
        {
            if (processor == null) throw new ArgumentNullException("processor");
            IMessageProcessor p;
            if (_processors.TryGetValue(protocol, out p))
            {
                _processors.TryUpdate(protocol, processor, p); 
                return this;
            }
            if(!_processors.TryAdd(protocol, processor))throw new System.Exception("Cannot add a message processor.");
            return this;
        }

        #endregion

        #region Events

        void NewTransaction(object sender, KJFramework.EventArgs.LightSingleArgEventArgs<IMessageTransaction<BaseMessage>> e)
        {
            BusinessMessageTransaction transaction = (BusinessMessageTransaction)e.Target;
            IMessageProcessor processor;
            MessageIdentity identity = transaction.Request.MessageIdentity;
            if (!_processors.TryGetValue(new Protocols { ProtocolId = identity.ProtocolId, ServiceId = identity.ServiceId, DetailsId = identity.DetailsId }, out processor))
            {
                _tracing.Error("#Schedule message failed, because cannot find processor. #P:{0}, S{1}, D{2}", identity.ProtocolId, identity.ServiceId, identity.DetailsId);
                return;
            }
            try { processor.Process(transaction); }
            catch (System.Exception ex) { _tracing.Error(ex, null);  }
        }

        void Disconnected(object sender, System.EventArgs e)
        {
            IServerConnectionAgent agent = (IServerConnectionAgent)sender;
            agent.Disconnected -= Disconnected;
            agent.NewTransaction -= NewTransaction;
            lock (_lockObj) _agents.Remove(agent);
        }

        #endregion
    }
}