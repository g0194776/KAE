using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using KJFramework.ApplicationEngine.Attributes;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Exceptions;
using KJFramework.ApplicationEngine.Processors;
using KJFramework.ApplicationEngine.Proxies;
using KJFramework.ApplicationEngine.Resources;
using KJFramework.Messages.Contracts;
using KJFramework.Net.Configurations;
using KJFramework.Net.HostChannels;
using KJFramework.Net.Identities;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Comparers;
using KJFramework.Net.Transaction.Objects;
using KJFramework.Tracing;

namespace KJFramework.ApplicationEngine
{
    /// <summary>
    ///    KAE应用抽象父类
    /// </summary>
    public class Application : IApplication
    {
        #region Constructor.

        /// <summary>
        ///    KAE应用抽象父类
        /// </summary>
        public Application(Action<MetadataMessageTransaction, MetadataContainer> handleSucceedSituation, Action<MetadataMessageTransaction, KAEErrorCodes, string> handleErrorSituation)
        {
            _handleSucceedSituation = handleSucceedSituation;
            _handleErrorSituation = handleErrorSituation;
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
        public ApplicationStatus Status { get; internal set; }

        /// <summary>
        ///    获取应用等级
        /// </summary>
        public ApplicationLevel Level { get; private set; }

        /// <summary>
        ///    获取一个值，该值标示了当前KPP包裹是否包含了一个完整的运行环境所需要的所有依赖文件
        /// </summary>
        public bool IsCompletedEnvironment { get; private set; }

        /// <summary>
        ///    获取内部所使用的隧道连接地址
        /// </summary>
        public string TunnelAddress { get; private set; }

        /// <summary>
        ///    获取应用kpp文件的CRC
        /// </summary>
        internal long CRC { get { return _structure.GetHeadField<long>("CRC"); } }

        private string _previousCodeMD5;
        private KPPDataStructure _structure;
        private IHostTransportChannel _hostChannel;
        private IDictionary<MessageIdentity, MetadataKAEProcessor> _processors;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (Application));
        private readonly Action<MetadataMessageTransaction, KAEErrorCodes, string> _handleErrorSituation;
        private readonly Action<MetadataMessageTransaction, MetadataContainer> _handleSucceedSituation;

        #endregion

        #region Methods.
        
        /// <summary>
        ///    反向更新从CSN推送过来的KEY和VALUE配置信息
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="value">VALUE</param>
        public void UpdateConfiguration(string key, string value)
        {
            SystemWorker.ConfigurationProxy.UpdateConfiguration(key, value);
        }

        /// <summary>
        ///    更新网络缓存信息
        /// </summary>
        /// <param name="level">应用等级</param>
        /// <param name="cache">远程目标终结点信息列表</param>
        /// <param name="protocol">通信协议</param>
        /// <param name="protocolTypes">协议类型</param>
        public void UpdateCache(Protocols protocol, ProtocolTypes protocolTypes, ApplicationLevel level, List<string> cache)
        {
            SystemWorker.UpdateCache(protocol, protocolTypes, level, cache);
        }

        /// <summary>
        ///    应用初始化
        /// </summary>
        /// <param name="structure">KPP资源包的数据结构</param>
        /// <param name="settings">APP所使用的网络资源设置集</param>
        /// <param name="proxy">KAE宿主代理器</param>
        /// <param name="greyPolicyCode">灰度升级策略脚本</param>
        internal void Initialize(KPPDataStructure structure, ChannelInternalConfigSettings settings, IKAEResourceProxy proxy, string greyPolicyCode = null)
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
                _processors = CollectAbilityProcessors();
                InnerInitialize();
                Status = ApplicationStatus.Initialized;
            }
            catch (Exception ex)
            {
                _tracing.Error(ex);
                Status = ApplicationStatus.Exception;
                throw;
            }
        }

        /// <summary>
        ///     收集目标KAE应用程序集内部的所有消息处理器
        /// </summary>
        /// <returns>返回消息处理器可以处理的消息标示集合</returns>
        /// <exception cref="DuplicatedProcessorException">具有多个能处理相同MessageIdentity的KAE处理器</exception>
        /// <exception cref="NotSupportedException">不支持的Protocol Type</exception>
        protected virtual IDictionary<MessageIdentity, MetadataKAEProcessor> CollectAbilityProcessors()
        {
            IDictionary<MessageIdentity, MetadataKAEProcessor> dic = new Dictionary<MessageIdentity, MetadataKAEProcessor>(new MessageIdentityComparer());
            Type[] types = Assembly.Load(File.ReadAllBytes(_structure.GetSectionField<string>(0x00, "ApplicationMainFileName"))).GetTypes();
            foreach (Type type in types)
            {
                try
                {
                    if (type.IsAbstract) continue;
                    if (!type.IsSubclassOf(typeof (MetadataKAEProcessor))) continue;
                    KAEProcessorPropertiesAttribute[] attributes = (KAEProcessorPropertiesAttribute[])type.GetCustomAttributes(typeof (KAEProcessorPropertiesAttribute), true);
                    if (attributes.Length == 0)
                    {
                        _tracing.Warn("#Found a KAE processor, type: {0}. BUT there wasn't any KAEProcessorPropertiesAttribute can be find.", type.Name);
                        continue;
                    }
                    MessageIdentity identity = new MessageIdentity
                    {
                        ProtocolId = attributes[0].ProtocolId,
                        ServiceId = attributes[0].ServiceId,
                        DetailsId = attributes[0].DetailsId
                    };
                    if (dic.ContainsKey(identity))
                        throw new DuplicatedProcessorException("#Duplicated KAE processor which it has same ability to handle a type of message. #MessageIdentity: " + identity);
                    dic.Add(identity, (MetadataKAEProcessor)Activator.CreateInstance(type, this));
                }
                catch (Exception ex) { _tracing.Error(ex); }
            }
            return dic;
        }

        /// <summary>
        ///    初始化函数
        /// </summary>
        protected virtual void InnerInitialize()
        {
            
        }
        
        internal void HandleBusiness(Tuple<KAENetworkResource, ApplicationLevel> tag, MetadataMessageTransaction transaction, MessageIdentity reqMsgIdentity, object reqMsg, TransactionIdentity transactionIdentity)
        {
            MetadataKAEProcessor processor;
            //Targeted MessageIdentity CANNOT be support.
            if (!_processors.TryGetValue(reqMsgIdentity, out processor))
            {
                _handleErrorSituation(transaction, KAEErrorCodes.NotSupportedMessageIdentity, "#We'd not supported current business protocol yet!");
                return;
            }
            MetadataContainer rsp = processor.Process(transaction.Request);
            if (!transaction.NeedResponse) return;
            if (rsp == null) _handleErrorSituation(transaction, KAEErrorCodes.UnhandledExceptionOccured, string.Empty);
            else _handleSucceedSituation(transaction, rsp);
        }

        #endregion
    }
}