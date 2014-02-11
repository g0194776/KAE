using System.Diagnostics;
using KJFramework.Helpers;
using KJFramework.Messages.Contracts;
using KJFramework.Net.Exception;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.Attribute;
using KJFramework.Net.Transaction.Identities;
using KJFramework.Net.Transaction.Objects;
using KJFramework.Net.Transaction.Processors;
using KJFramework.PerformanceProvider;
using KJFramework.Tracing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace KJFramework.Net.Transaction.Schedulers
{
    /// <summary>
    ///     请求分发器，提供了相关的基本操作
    /// </summary>
    public class MetadataMessageRequestScheduler : INewRequestScheduler<MetadataContainer>
    {
        #region Constructor

        /// <summary>
        ///     请求分发器，提供了相关的基本操作
        /// </summary>
        public MetadataMessageRequestScheduler()
        {
            _perfCategroy = string.Format("DynamicPerf::{0}", Process.GetCurrentProcess().ProcessName);
        }

        #endregion

        #region Members

        private object _lockObj = new object();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(MetadataMessageRequestScheduler));
        private readonly IList<IConnectionAgent> _agents = new List<IConnectionAgent>();
        private readonly ConcurrentDictionary<Protocols, NewProcessorObject> _processors = new ConcurrentDictionary<Protocols, NewProcessorObject>();
        private readonly string _perfCategroy;

        #endregion

        #region Implementation of IRequestScheduler

        /// <summary>
        ///     注册一个连接代理器
        /// </summary>
        /// <param name="agent">连接代理器</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public void Regist(IServerConnectionAgent<MetadataContainer> agent)
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
        public INewRequestScheduler<MetadataContainer> Regist(Protocols protocol, IMessageTransactionProcessor<MetadataMessageTransaction, MetadataContainer> processor)
        {
            if (processor == null) throw new ArgumentNullException("processor");
            NewProcessorObject p;
            if (_processors.TryGetValue(protocol, out p)) return this;
            p = new NewProcessorObject { Processor = processor };
            if (!_processors.TryAdd(protocol, p)) throw new System.Exception("Cannot add a message processor.");
            return this;
        }

        /// <summary>
        ///     注册一个消息处理器
        /// </summary>
        /// <param name="processor">处理器</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="ArgumentException">Attribute标签为空</exception>
        public INewRequestScheduler<MetadataContainer> Regist(IMessageTransactionProcessor<MetadataMessageTransaction, MetadataContainer> processor)
        {     
            ProcessorMessageIdentityAttribute[] attribute = (ProcessorMessageIdentityAttribute[]) GetType().GetCustomAttributes(typeof(ProcessorMessageIdentityAttribute), true);
            if (attribute.Length == 0) throw new ArgumentException("#Current attribute is not found");
            NewProcessorObject p;
            Protocols protocol = new Protocols{ProtocolId = attribute[0].ProtocolId, ServiceId = attribute[0].ServiceId, DetailsId = attribute[0].DetailsId};
            if (_processors.TryGetValue(protocol, out p)) return this;
            p = new NewProcessorObject { Processor = processor };
            if (!_processors.TryAdd(protocol, p)) throw new System.Exception("Cannot add a message processor.");
            return this;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     动态创建已注册处理器的性能计数器
        /// </summary>
        public void CreateDynamicCounters()
        {
            if (_processors.Count == 0) return;
            AppDomain.CurrentDomain.ProcessExit += delegate
            {
                PerformanceCounterCategory.Delete(_perfCategroy);
            };
            CounterCreationDataCollection dataCollection = new CounterCreationDataCollection();
            #region Ensure performance counter category.

            try
            {
                bool exists = PerformanceCounterCategory.Exists(_perfCategroy);
                if (exists) PerformanceCounterCategory.Delete(_perfCategroy);
                foreach (NewProcessorObject obj in _processors.Values)
                {
                    string defaultCounterName = string.Format("#Frequency Calls:{0}", obj.Processor.GetType().Name);
                    PerfCounterAttribute[] attributes;
                    if ((attributes = AttributeHelper.GetCustomerAttributes<PerfCounterAttribute>(obj.Processor.GetType())) != null && attributes.Length > 0)
                    {
                        foreach (PerfCounterAttribute attribute in attributes)
                            dataCollection.Add(new CounterCreationData(attribute.Name, attribute.Help, attribute.Type));
                    }
                    //add default performance counter for each processor.
                    dataCollection.Add(
                        new CounterCreationData(defaultCounterName,
                                                "This was automic created by KJFramework. It'll be used for the infomation collections.",
                                                PerformanceCounterType.RateOfCountsPerSecond32));
                }
                if (dataCollection.Count > 0)
                    PerformanceCounterCategory.Create(_perfCategroy, string.Format("#This was automic created by KJFramework for process: {0}, Pls *DO NOT* remove it by manual.", Process.GetCurrentProcess().ProcessName), PerformanceCounterCategoryType.MultiInstance, dataCollection);

                #region Dynamic create performance for each processor.

                foreach (NewProcessorObject obj in _processors.Values)
                {
                    obj.Counters = new List<PerfCounter>();
                    string defaultCounterName = string.Format("#Frequency Calls:{0}", obj.Processor.GetType().Name);
                    PerfCounterAttribute[] attributes;
                    if ((attributes = AttributeHelper.GetCustomerAttributes<PerfCounterAttribute>(obj.Processor.GetType())) != null && attributes.Length > 0)
                    {
                        foreach (PerfCounterAttribute attribute in attributes)
                            obj.Counters.Add(new PerfCounter(_perfCategroy, Process.GetCurrentProcess().ProcessName, attribute));
                    }
                    //add default performance counter for each processor.
                    obj.Counters.Add(new PerfCounter(_perfCategroy, Process.GetCurrentProcess().ProcessName, new PerfCounterAttribute(defaultCounterName, PerformanceCounterType.RateOfCountsPerSecond32)));
                }

                #endregion
            }
            catch (System.Exception ex) { _tracing.Error(ex, null); }

            #endregion
        }

        /// <summary>
        ///      销毁程序内部创建的动态性能计数器
        /// </summary>
        public void DistoryDynamicCounters()
        {
            try { PerformanceCounterCategory.Delete(_perfCategroy); }
            catch (System.Exception ex) { _tracing.Error(ex, null); }
        }

        #endregion

        #region Events

        void NewTransaction(object sender, KJFramework.EventArgs.LightSingleArgEventArgs<IMessageTransaction<MetadataContainer>> e)
        {
            NewProcessorObject obj;
            MetadataMessageTransaction transaction = (MetadataMessageTransaction)e.Target;
            if(!transaction.Request.IsAttibuteExsits(0x00)) throw new KeyHasNullException();
            MessageIdentity identity = transaction.Request.GetAttribute(0x00).GetValue<MessageIdentity>();
            if (!_processors.TryGetValue(new Protocols { ProtocolId = identity.ProtocolId, ServiceId = identity.ServiceId, DetailsId = identity.DetailsId }, out obj))
            {
                _tracing.Error("#Schedule message failed, because cannot find processor. #P:{0}, S{1}, D{2}", identity.ProtocolId, identity.ServiceId, identity.DetailsId);
                return;
            }
            try
            {
                #region Use Performance Counters.

                if (obj.Counters != null)
                {
                    if (obj.Counters.Count == 1) obj.Counters[0].Increment();
                    else for (int i = 0; i < obj.Counters.Count; i++) obj.Counters[i].Increment();
                }

                #endregion
                obj.Processor.Process(transaction);
            }
            catch (System.Exception ex) { _tracing.Error(ex, null); }
        }

        void Disconnected(object sender, System.EventArgs e)
        {
            IServerConnectionAgent<MetadataContainer> agent = (IServerConnectionAgent<MetadataContainer>)sender;
            agent.Disconnected -= Disconnected;
            agent.NewTransaction -= NewTransaction;
            lock (_lockObj) _agents.Remove(agent);
        }

        #endregion
    }
}