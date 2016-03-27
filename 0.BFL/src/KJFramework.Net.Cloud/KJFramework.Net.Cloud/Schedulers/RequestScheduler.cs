using KJFramework.Enums;
using KJFramework.Net.Channels;
using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Objects;
using KJFramework.Net.Cloud.Pools;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Net.Cloud.Tasks;
using KJFramework.Net.Exception;
using KJFramework.Statistics;
using KJFramework.Tracing;
using System;
using System.Collections;
using System.Collections.Generic;

namespace KJFramework.Net.Cloud.Schedulers
{
    /// <summary>
    ///     ������������ṩ����صĻ�������
    /// </summary>
    /// <typeparam name="T">Э��ջ�и�����Ϣ���͡�</typeparam>
    public class RequestScheduler<T> : IRequestScheduler<T>
    {
        #region ���캯��

        /// <summary>
        ///     ������������ṩ����صĻ�������
        /// </summary>
        public RequestScheduler()
            : this(30000)
        {
        }

        /// <summary>
        ///     ������������ṩ����صĻ�������
        /// </summary>
        public RequestScheduler(int taskCount)
        {
            _id = Guid.NewGuid();
            Initialzie(taskCount);
        }

        #endregion

        #region Members

        private Object _lockNetworkObject = new Object();
        private Object _lockFunctionObject = new Object();
        private Object _lockProcessorObject = new Object();
        protected Dictionary<Guid, INetworkNode<T>> _networkNodes = new Dictionary<Guid, INetworkNode<T>>();
        protected Dictionary<Guid, IMessageFunctionNode<T>> _functionNodes = new Dictionary<Guid, IMessageFunctionNode<T>>();
        protected Dictionary<Type, IFunctionProcessor<T>> _cacheProcessor = new Dictionary<Type, IFunctionProcessor<T>>();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(RequestScheduler<T>));
        protected RequestTaskPool<T> _taskPool;

        #endregion

        #region Implementation of IStatisticable<IStatistic>

        private Dictionary<StatisticTypes, IStatistic> _statistics = new Dictionary<StatisticTypes, IStatistic>();
        private readonly Guid _id;

        /// <summary>
        /// ��ȡ������ͳ����
        /// </summary>
        public Dictionary<StatisticTypes, IStatistic> Statistics
        {
            get { return _statistics; }
            set { _statistics = value; }
        }

        #endregion

        #region Implementation of IRequestScheduler<T>

        /// <summary>
        ///     ��ȡΨһ��ʾ
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        ///   ���յ�ǰ�Ѿ�ע��Ĺ��ܽڵ㣬���ȴ���һ����Ϣ����
        /// </summary>
        /// <param name="networkNode">����ڵ�</param>
        /// <param name="target">���յ�����Ϣ����</param>
        public void Schedule(NetworkNode<T> networkNode, ReceivedMessageObject<T> target)
        {
            ReceivedMessageObject<T> obj = target;
            T message = obj.Message;
            Schedule(message, networkNode, obj.Channel);
        }

        /// <summary>
        ///     ���յ�ǰ�Ѿ�ע��Ĺ��ܽڵ㣬���ȴ���һ����Ϣ����
        /// </summary>
        /// <param name="message">��Ϣ</param>
        /// <param name="networkNode">����ڵ�</param>
        /// <param name="channel">����ͨ��</param>
        public void Schedule(T message, NetworkNode<T> networkNode, IMessageTransportChannel<T> channel)
        {
            //not null.
            if (Comparer.Default.Compare(message, null) != 0)
            {
                IFunctionProcessor<T> processor = GetCacheProcessor(message.GetType()) ?? SearchProcessor(channel.Key, message);
                //can not found any processor can process it.
                if (processor == null)
                {
                    _tracing.Info("can not found any processor can process it. MESSAGE TYPE = " + message.GetType());
                    return;
                }
                //rent a task to process it.
                IRequestTask<T> task = _taskPool.Rent();
                //rent successed.
                if (task == null)
                {
                    _tracing.Info("Can not rent a IRequestTask<T>, because return value is null.");
                    return;
                }
                task.Node = networkNode;
                task.Channel = channel;
                task.Message = message;
                task.Processor = processor;
                task.Execute();
            }
        }

        /// <summary>
        ///     ��ʼ��
        /// </summary>
        /// <param name="taskCount">��������</param>
        /// <exception cref="InitializeFailedException">��ʼ��ʧ��</exception>
        public void Initialzie(int taskCount)
        {
            _taskPool = new RequestTaskPool<T>(TaskExecuteSuccessful, TaskExecuteFail, TaskExecuteTimeout, taskCount);
            _taskPool.Initialzie();
        }

        /// <summary>
        ///     ע������ڵ�
        /// </summary>
        /// <param name="node">����ڵ�</param>
        public void Regist(INetworkNode<T> node)
        {
            if (node == null) return;
            lock (_lockNetworkObject)
            {
                _networkNodes.Add(node.Id, node);
            }
        }

        /// <summary>
        ///     ע�Ṧ�ܽڵ�
        /// </summary>
        /// <param name="node">���ܽڵ�</param>
        public void Regist(IMessageFunctionNode<T> node)
        {
            if (node == null) return;
            if (!node.Initialize()) throw new InitializeFailedException();
            lock (_lockFunctionObject)
            {
                _functionNodes.Add(node.Id, node);
            }
        }

        /// <summary>
        ///     ע������ڵ�
        /// </summary>
        /// <param name="id">Ψһ��ʾ</param>
        public void UnRegistNetworkNode(Guid id)
        {
            lock (_lockNetworkObject)
            {
                if (_networkNodes.ContainsKey(id))
                {
                    try
                    {
                        _networkNodes[id].Close();
                    }
                    catch (System.Exception ex)
                    {
                        _tracing.Error(ex, null);
                    }
                    finally
                    {
                        _networkNodes.Remove(id);
                    }
                }
            }
        }

        /// <summary>
        ///     ע�Ṧ�ܽڵ�
        /// </summary>
        /// <param name="id">Ψһ��ʾ</param>
        public void UnRegistFunctionNode(Guid id)
        {
            lock (_lockFunctionObject)
            {
                if (_functionNodes.ContainsKey(id))
                {
                    _functionNodes.Remove(id);
                }
            }
        }

        /// <summary>
        ///     ��ʼ����
        /// </summary>
        public void Start()
        {
            if (_networkNodes.Count > 0)
            {
                foreach (KeyValuePair<Guid, INetworkNode<T>> networkNode in _networkNodes)
                {
                    networkNode.Value.NewMessageReceived += NetworkNodeNewMessageReceived;
                    networkNode.Value.Open();
                }
            }
        }

        /// <summary>
        ///     ֹͣ����
        /// </summary>
        public void Stop()
        {
            if (_networkNodes.Count > 0)
            {
                foreach (KeyValuePair<Guid, INetworkNode<T>> networkNode in _networkNodes)
                {
                    try
                    {
                        networkNode.Value.NewMessageReceived -= NetworkNodeNewMessageReceived;
                        networkNode.Value.Close();
                    }
                    catch (System.Exception ex)
                    {
                        _tracing.Error(ex, null);
                    }
                }
            }
            _cacheProcessor.Clear();
        }

        #endregion

        #region �¼�

        void NetworkNodeNewMessageReceived(object sender, KJFramework.EventArgs.LightSingleArgEventArgs<ReceivedMessageObject<T>> e)
        {
            NetworkNode<T> networkNode = (NetworkNode<T>) sender;
            Schedule(networkNode, e.Target);
        }

        //execute timeout.
        void TaskExecuteTimeout(object sender, KJFramework.EventArgs.LightSingleArgEventArgs<T> e)
        {
            IRequestTask<T> task = (IRequestTask<T>)sender;
            _tracing.Info(String.Format("Request task {0} had timeout ! when it process message {1}.", task.TaskId, task.Message.GetType()));
            Giveback(task);
        }
        //execute successed.
        void TaskExecuteSuccessful(object sender, System.EventArgs e)
        {
            IRequestTask<T> task = (IRequestTask<T>) sender;
            T message = task.ResultMessage;
            //get network node.
            try
            {
                //not null.
                if (Comparer.Default.Compare(message, null) != 0)
                {
                    IMessageTransportChannel<T> channel = task.Channel;
                    if (channel == null)
                    {
                        _tracing.Info("This result message lost the org transport channel.");
                        return;
                    }
                    //disconencted.
                    if (!channel.IsConnected)
                    {
                        _tracing.Info("Remote connection has been disconnected. #key: " + channel.Key);
                        return;
                    }
                    channel.Send(message);
                    return;
                }
            }
            catch (System.Exception ex) { _tracing.Error(ex, null); }
            finally { Giveback(task); }
        }

        //execute failed.
        void TaskExecuteFail(object sender, System.EventArgs e)
        {
            IRequestTask<T> task = (IRequestTask<T>)sender;
            _tracing.Info(String.Format("Request task {0} has failed ! when it process message {1}.", task.TaskId, task.Message.GetType()));
            Giveback(task);
        }

        #endregion

        #region Functions

        /// <summary>
        ///     ��ȡ����ָ����Ϣ���͵Ļ��湦�ܴ�����
        /// </summary>
        /// <param name="messageType">��Ϣ����</param>
        /// <returns>���ػ���Ĺ��ܴ�����</returns>
        protected IFunctionProcessor<T> GetCacheProcessor(Type messageType)
        {
            IFunctionProcessor<T> processor;
            if (_cacheProcessor.TryGetValue(messageType, out processor))
            {
                return processor;
            }
            return null;
        }

        /// <summary>
        ///     ��ѯ����ָ����Ϣ�Ĺ��ܴ�������������ҵ����ͻ�������
        /// </summary>
        /// <param name="id">����ͨ�����</param>
        /// <param name="message">Ҫ�������Ϣ</param>
        /// <returns>���ؽ��</returns>
        protected IFunctionProcessor<T> SearchProcessor(Guid id, T message)
        {
            IFunctionProcessor<T> processor;
            foreach (var functionNode in _functionNodes.Values)
            {
                if ((processor = functionNode.CanProcess(id, message)) != null)
                {
                    //cache it.
                    lock (_lockProcessorObject)
                    {
                        _cacheProcessor.Add(message.GetType(), processor);
                    }
                    return processor;
                }
            }
            return null;
            #region ԭ���Ĳ�ѯ�㷨

            ////use linq to searh what processor can process this message.
            //var result =
            //    _functionNodes.Select(
            //        functionNode =>
            //        new {Node = functionNode.Value, ProcessorId = functionNode.Value.CanProcess(Guid.Empty, message)}).
            //        Where(ids => ids.ProcessorId != null);
            ////get result
            //if (result != null)
            //{
            //    foreach (var function in result)
            //    {
            //        //pickup target processor
            //        IFunctionProcessor<T> processor = function.Node.GetProcessor((Guid) function.ProcessorId);
            //        if (processor != null)
            //        {

            //        }
            //        return processor;
            //    }
            //}
            //return null;

            #endregion
        }

        /// <summary>
        ///     �黹��������
        /// </summary>
        /// <param name="task">��������</param>
        protected virtual void Giveback(IRequestTask<T> task)
        {
            if (task == null) return;
            _taskPool.Giveback(task);
        }

        #endregion
    }
}