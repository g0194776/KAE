using System.Threading;
using KJFramework.ApplicationEngine.Attributes;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Exceptions;
using KJFramework.ApplicationEngine.Helpers;
using KJFramework.ApplicationEngine.Processors;
using KJFramework.ApplicationEngine.Resources;
using KJFramework.ApplicationEngine.Rings;
using KJFramework.Dynamic.Components;
using KJFramework.Enums;
using KJFramework.EventArgs;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Types;
using KJFramework.Messages.ValueStored;
using KJFramework.Messages.ValueStored.DataProcessor.Mapping;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.HostChannels;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Channels.Uri;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.Comparers;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.ProtocolStack;
using KJFramework.Net.Transaction.ValueStored;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KJFramework.ApplicationEngine
{
    /// <summary>
    ///    KAE应用抽象父类
    /// </summary>
    public abstract class Application : DynamicDomainComponent, IApplication
    {
        #region Constructor.

        /// <summary>
        ///    KAE应用抽象父类
        /// </summary>
        protected Application()
        {
            Status = ApplicationStatus.Unknown;
        }

        #endregion

        #region Members.

        /// <summary>
        ///    获取应用版本
        /// </summary>
        public string Version { get; private set; }

        /// <summary>
        ///    获取应用描述
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        ///    获取应用包名
        /// </summary>
        public string PackageName { get; private set; }

        /// <summary>
        ///    获取应用的全局唯一编号
        /// </summary>
        public Guid GlobalUniqueId { get; private set; }

        /// <summary>
        ///    获取应用当前的状态
        /// </summary>
        public ApplicationStatus Status { get; private set; }

        /// <summary>
        ///    获取应用等级
        /// </summary>
        public ApplicationLevel Level { get; private set; }

        /// <summary>
        ///    获取一个值，该值标示了当前KPP包裹是否包含了一个完整的运行环境所需要的所有依赖文件
        /// </summary>
        public bool IsCompletedEnvironment { get; private set; }

        /// <summary>
        ///    获取应用kpp文件的CRC
        /// </summary>
        internal long CRC { get { return _structure.GetHeadField<long>("CRC"); } }

        private KPPDataStructure _structure;
        private IHostTransportChannel _hostChannel;
        private IDictionary<ProtocolTypes, Dictionary<MessageIdentity, object>> _processors;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (Application));
        private static readonly MetadataProtocolStack _protocolStack = new MetadataProtocolStack();
        private static readonly MetadataTransactionManager _transactionManager = new MetadataTransactionManager(new TransactionIdentityComparer());
        private IDictionary<MessageIdentity, IDictionary<ApplicationLevel, KetamaRing>> _rings = new Dictionary<MessageIdentity, IDictionary<ApplicationLevel, KetamaRing>>();

        #endregion

        #region Methods.

        /// <summary>
        ///    获取应用内部所有已经支持的网络通讯协议
        /// </summary>
        /// <returns>返回支持的网络通信协议列表</returns>
        public IDictionary<ProtocolTypes, IList<MessageIdentity>> AcquireSupportedProtocols()
        {
            IDictionary<ProtocolTypes, IList<MessageIdentity>> dic = new Dictionary<ProtocolTypes, IList<MessageIdentity>>();
            foreach (KeyValuePair<ProtocolTypes, Dictionary<MessageIdentity, object>> pair in _processors)
                dic.Add(pair.Key, pair.Value.Keys.ToList());
            return dic;
        }

        /// <summary>
        ///    更新网络缓存信息
        /// </summary>
        /// <param name="cache">网络信息</param>
        public void UpdateNetworkCache(Dictionary<string, List<string>> cache)
        {
            IDictionary<MessageIdentity, IDictionary<ApplicationLevel, KetamaRing>> rings = new Dictionary<MessageIdentity, IDictionary<ApplicationLevel, KetamaRing>>();
            foreach (KeyValuePair<string, List<string>> pair in cache)
            {
                string[] contents = pair.Key.Split(new[] {"_"}, StringSplitOptions.RemoveEmptyEntries);
                string[] ids = contents[0].Replace("(", "").Replace(")", "").Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                ApplicationLevel level;
                if (!Enum.TryParse(contents[2], out level))
                {
                    _tracing.Error("#Couldn't parse targeted value to the Application Level. Value: {0}." + contents[2]);
                    continue;
                }
                MessageIdentity identity = new MessageIdentity
                {
                    ProtocolId = byte.Parse(ids[0]),
                    ServiceId = byte.Parse(ids[1]),
                    DetailsId = byte.Parse(ids[2])
                };
                //prepares kathma ring.
                KetamaRing ring = new KetamaRing(pair.Value.Select(v => new KAEHostNode(v)).ToList());
                IDictionary<ApplicationLevel, KetamaRing> tempDic;
                if (!rings.TryGetValue(identity, out tempDic)) rings.Add(identity, (tempDic = new Dictionary<ApplicationLevel, KetamaRing>()));
                tempDic[level] = ring;
            }
            Interlocked.Exchange(ref _rings, rings);
        }

        /// <summary>
        ///    应用初始化
        /// </summary>
        /// <param name="structure">KPP资源包的数据结构</param>
        internal void Initialize(KPPDataStructure structure)
        {
            _structure = structure;
            Version = _structure.GetSectionField<string>(0x00, "Version");
            PackageName = _structure.GetSectionField<string>(0x00, "PackName");
            Description = _structure.GetSectionField<string>(0x00, "PackDescription");
            GlobalUniqueId = _structure.GetSectionField<Guid>(0x00, "GlobalUniqueIdentity");
            Level = (ApplicationLevel) _structure.GetSectionField<byte>(0x00, "ApplicationLevel");
            IsCompletedEnvironment = _structure.GetSectionField<bool>(0x00, "IsCompletedEnvironment");
            Status = ApplicationStatus.Initializing;
            try
            {
                InnerInitialize();
                _processors = CollectAbilityProcessors();
                Status = ApplicationStatus.Initialized;
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex);
                Status = ApplicationStatus.Exception;
                throw;
            }
        }

        protected override void InnerStart()
        {
            Status = ApplicationStatus.Running;
        }

        protected override void InnerStop()
        {
            if (Status != ApplicationStatus.Running) throw new IllegalApplicationStatusException("#Illegal application status that it couldn't start! #Status: " + Status);
            Status = ApplicationStatus.Stoping;
            try
            {
                if (_hostChannel != null)
                {
                    _hostChannel.ChannelCreated -= ChannelCreated;
                    _hostChannel.UnRegist();
                    _hostChannel = null;
                }
                _tunnelAddress = null;
            }
            catch (System.Exception ex) { _tracing.Error(ex); }
            finally { Status = ApplicationStatus.Stopped; }
        }

        protected override void InnerOnLoading()
        {
            //initializes something before actual business starting.
            ExtensionTypeMapping.Regist(typeof(MessageIdentityValueStored));
            ExtensionTypeMapping.Regist(typeof(TransactionIdentityValueStored));
            if (Status != ApplicationStatus.Initialized && Status != ApplicationStatus.Stopped) throw new IllegalApplicationStatusException("#Illegal application status that it couldn't start! #Status: " + Status);
            //URL FORMAT: pipe://./{APP-Name}_ulong(MD5(DateTime.Now),4 ,8)
            string url = string.Format("pipe://./{0}_{1}", PackageName, DateTime.Now.Ticks);
            _hostChannel = new PipeHostTransportChannel(new PipeUri(url), 254);
            if (!_hostChannel.Regist()) throw new AllocResourceFailedException("#Sadly, We couldn't alloc current network resource. #Resource: " + url);
            _hostChannel.ChannelCreated += ChannelCreated;
            _isUseTunnel = true;
            _tunnelAddress = url;
            Status = ApplicationStatus.Loaded; 
        }

        protected override HealthStatus InnerCheckHealth()
        {
            return HealthStatus.Good;
        }

        /// <summary>
        ///     收集目标KAE应用程序集内部的所有消息处理器
        /// </summary>
        /// <returns>返回消息处理器可以处理的消息标示集合</returns>
        /// <exception cref="DuplicatedProcessorException">具有多个能处理相同MessageIdentity的KAE处理器</exception>
        /// <exception cref="NotSupportedException">不支持的Protocol Type</exception>
        protected virtual IDictionary<ProtocolTypes, Dictionary<MessageIdentity, object>> CollectAbilityProcessors()
        {
            IDictionary<ProtocolTypes, Dictionary<MessageIdentity, object>> dic =
                new Dictionary<ProtocolTypes, Dictionary<MessageIdentity, object>>();
            Type[] types = GetType().Assembly.GetTypes();
            foreach (Type type in types)
            {
                try
                {
                    if (type.IsAbstract) continue;
                    if (!type.IsSubclassOf(typeof (IntellegenceKAEProcessor)) &&
                        !type.IsSubclassOf(typeof (JsonKAEProcessor)) &&
                        !type.IsSubclassOf(typeof (MetadataKAEProcessor))) continue;
                    KAEProcessorPropertiesAttribute[] attributes =
                        (KAEProcessorPropertiesAttribute[])
                            type.GetCustomAttributes(typeof (KAEProcessorPropertiesAttribute), true);
                    if (attributes.Length == 0)
                    {
                        _tracing.Warn(
                            "#Found a KAE processor, type: {0}. BUT there wasn't any KAEProcessorPropertiesAttribute can be find.",
                            type.Name);
                        continue;
                    }
                    Dictionary<MessageIdentity, object> subDic;
                    ProtocolTypes targetProtocolType;
                    MessageIdentity identity = new MessageIdentity
                    {
                        ProtocolId = attributes[0].ProtocolId,
                        ServiceId = attributes[0].ServiceId,
                        DetailsId = attributes[0].DetailsId
                    };
                    if (type.IsSubclassOf(typeof (IntellegenceKAEProcessor)))
                        targetProtocolType = ProtocolTypes.Intellegence;
                    else if (type.IsSubclassOf(typeof (JsonKAEProcessor))) targetProtocolType = ProtocolTypes.Json;
                    else if (type.IsSubclassOf(typeof (MetadataKAEProcessor)))
                        targetProtocolType = ProtocolTypes.Metadata;
                    else throw new NotSupportedException();
                    if (!dic.TryGetValue(targetProtocolType, out subDic))
                        dic.Add(targetProtocolType, (subDic = new Dictionary<MessageIdentity, object>()));
                    if (subDic.ContainsKey(identity))
                        throw new DuplicatedProcessorException(
                            "#Duplicated KAE processor which it has same ability to handle a type of message. #MessageIdentity: " +
                            identity);
                    subDic.Add(identity, Activator.CreateInstance(type, this));
                }
                catch (System.Exception ex)
                {
                    _tracing.Error(ex);
                }
            }
            return dic;
        }

        /// <summary>
        ///    初始化函数
        /// </summary>
        protected abstract void InnerInitialize();

        private object AssembleNewTransparencyTransaction(IMessageTransaction<MetadataContainer> preTransaction)
        {
            ProtocolTypes protocol = (ProtocolTypes) preTransaction.Request.GetAttributeAsType<byte>(0x0A);
            switch (protocol)
            {
                case ProtocolTypes.Metadata:
                    MetadataMessageTransaction transaction = new MetadataMessageTransaction(preTransaction.GetChannel());
                    transaction.Identity = preTransaction.Identity;
                    transaction.Request = preTransaction.Request.GetAttributeAsType<ResourceBlock>(0x0B).AsMetadataContainer();
                    transaction.NeedResponse = !transaction.Identity.IsOneway;
                    return transaction;
            }
            return null;
        }

        #endregion

        #region Events.

        //Interval piped name channel connected event.
        private void ChannelCreated(object sender, LightSingleArgEventArgs<ITransportChannel> e)
        {
            MetadataConnectionAgent agent = new MetadataConnectionAgent(new MessageTransportChannel<MetadataContainer>((IRawTransportChannel) e.Target, _protocolStack), _transactionManager);
            agent.Disconnected += AgentDisconnected;
            agent.NewTransaction += AgentNewTransaction;
        }

        private void AgentDisconnected(object sender, System.EventArgs e)
        {
            MetadataConnectionAgent agent = (MetadataConnectionAgent) sender;
            agent.Disconnected -= AgentDisconnected;
            agent.NewTransaction -= AgentNewTransaction;
        }

        /*
         * Application's tunnel MSG construction.
         * REQ Message:
         *      0x00 - MessageIdentity
         *      0x01 - TransactionIdentity
         *      0x0A - Protocol Type
         *      0x0B - Specified Value
         * RSP Message:
         *      0x00 - MessageIdentity
         *      0x01 - TransactionIdentity
         *      0x0A - Error Id
         *      0x0B - Error Reason
         *      0x0C - Specified Value
         */
        private void AgentNewTransaction(object sender, LightSingleArgEventArgs<IMessageTransaction<MetadataContainer>> e)
        {
            MetadataContainer reqMsg = e.Target.Request;
            ProtocolTypes protocol = (ProtocolTypes) reqMsg.GetAttributeAsType<byte>(0x0A);
            MessageIdentity messageIdentity = reqMsg.GetAttributeAsType<ResourceBlock>(0x0B).GetAttributeAsType<MessageIdentity>(0x00);
            object transaction = AssembleNewTransparencyTransaction(e.Target);
            object pObject;
            Dictionary<MessageIdentity, object> dic;
            if (!_processors.TryGetValue(protocol, out dic))
            {
                HandleErrorSituation(e.Target, KAEErrorCodes.NotSupportedNetworkType);
                return;
            }
            if (!dic.TryGetValue(messageIdentity, out pObject))
            {
                HandleErrorSituation(e.Target, KAEErrorCodes.NotSupportedMessageIdentity);
                return;
            }
            //handles concrete message by respective protocol's processor.
            switch (protocol)
            {
                case ProtocolTypes.Metadata:
                    MetadataKAEProcessor p1 = (MetadataKAEProcessor) pObject;
                    p1.Process((IMessageTransaction<MetadataContainer>) transaction);
                    break;
                case ProtocolTypes.Intellegence:
                    IntellegenceKAEProcessor p2 = (IntellegenceKAEProcessor)pObject;
                    p2.Process((IMessageTransaction<IntellectObject>) transaction);
                    break;
                case ProtocolTypes.Json:
                    JsonKAEProcessor p3 = (JsonKAEProcessor)pObject;
                    p3.Process((IMessageTransaction<string>) transaction);
                    break;
            }
        }

        private void HandleErrorSituation(IMessageTransaction<MetadataContainer> transaction, KAEErrorCodes errorCode)
        {
            MessageIdentity msgIdentity = transaction.Request.GetAttributeAsType<MessageIdentity>(0x00);
            msgIdentity.DetailsId += 1;
            MetadataContainer rspMsg = new MetadataContainer();
            rspMsg.SetAttribute(0x00, new MessageIdentityValueStored(msgIdentity));
            rspMsg.SetAttribute(0x0A, new ByteValueStored((byte)errorCode));
            rspMsg.SetAttribute(0x0B, new StringValueStored(string.Empty));
            transaction.SendResponse(rspMsg);
        }

    #endregion
    }
}