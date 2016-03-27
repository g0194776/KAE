using System;
using System.Collections.Generic;
using System.Threading;
using KJFramework.Basic.Enum;
using KJFramework.EventArgs;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Enums;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Statistics;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Subscribers;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Contexts;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Steps;
using KJFramework.Platform.Deploy.CSN.ProtocolStack.Enums;
using KJFramework.Statistics;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters
{
    /// <summary>
    ///     配置传输器，提供了相关的基本操作。
    /// </summary>
    public class ConfigTransmitter : IConfigTransmitter
    {
        #region Constructor

        static ConfigTransmitter()
        {
            //regist transmitte step.
            _steps.Add(TransmitterSteps.InitializePolicy, new InitializePolicyTransmitterStep());
            _steps.Add(TransmitterSteps.BeginTransfer, new BeginTransferDataTransmitterStep());
            _steps.Add(TransmitterSteps.EndTransfer, new InitializePolicyTransmitterStep());
            _steps.Add(TransmitterSteps.TransferData, new TransferDataTransmitterStep());
            _steps.Add(TransmitterSteps.TransferDataWithoutMultiPackage, new TransferDataWithoutMultiPackageTransmitterStep());
            _steps.Add(TransmitterSteps.Notify, new NotifyMultiPackageTransmitterStep());
        }

        /// <summary>
        ///     配置传输器，提供了相关的基本操作。
        /// </summary>
        /// <param name="configSubscriber">配置订阅人</param>
        /// <param name="context">上下文</param>
        public ConfigTransmitter(IConfigSubscriber configSubscriber, ITransmitterContext context)
        {
            ConfigTransmitterStatistic statistic = new ConfigTransmitterStatistic();
            statistic.Initialize(this);
            _statistics.Add(StatisticTypes.Other, statistic);
            if (configSubscriber == null || context == null)
            {
                throw new ArgumentException("Invailed parameters.");
            }
            _context = context;
            _subscriber = configSubscriber;
            if (Interlocked.Read(ref _curTaskId) == int.MaxValue)
            {
                Interlocked.Exchange(ref _curTaskId, 0);
            }
            _taskId = (int) Interlocked.Increment(ref _curTaskId);
        }

        #endregion

        #region Members

        private static long _curTaskId;
        private static Dictionary<TransmitterSteps, ITransmitteStep> _steps = new Dictionary<TransmitterSteps, ITransmitteStep>();
        protected ITransmitterContext _context;

        #endregion

        #region Implementation of IStatisticable<IStatistic>

        private Dictionary<StatisticTypes, IStatistic> _statistics = new Dictionary<StatisticTypes, IStatistic>();
        private int _taskId;
        protected IConfigSubscriber _subscriber;
        protected ConfigTypes _configType;
        protected DateTime _lastActionTime;
        protected TransmitterSteps _nextStep;

        /// <summary>
        /// 获取或设置统计器
        /// </summary>
        public Dictionary<StatisticTypes, IStatistic> Statistics
        {
            get { return _statistics; }
            set { _statistics = value; }
        }

        #endregion

        #region Implementation of IConfigTransmitter

        /// <summary>
        ///     获取一个关联此传输器的唯一任务编号
        /// </summary>
        public int TaskId
        {
            get { return _taskId; }
        }

        /// <summary>
        ///     获取或设置传输器上下文
        /// </summary>
        public ITransmitterContext Context
        {
            get { return _context; }
            set { _context = value; }
        }

        /// <summary>
        ///     获取配置订阅者
        /// </summary>
        public IConfigSubscriber Subscriber
        {
            get { return _subscriber; }
        }

        /// <summary>
        ///     获取或设置下一个传输步骤的类型
        /// </summary>
        public TransmitterSteps NextStep
        {
            get { return _nextStep; }
            set { _nextStep = value; }
        }

        /// <summary>
        ///     获取配置类型
        /// </summary>
        public ConfigTypes ConfigType
        {
            get { return _configType; }
        }

        /// <summary>
        ///     获取或设置上一次产生动作的时间
        /// </summary>
        public DateTime LastActionTime
        {
            get { return _lastActionTime; }
            set { _lastActionTime = value; }
        }

        /// <summary>
        ///     注册一个传输器步骤
        /// </summary>
        /// <param name="transmitterStep">传输器步骤枚举</param>
        /// <param name="step">传输器步骤</param>
        public void Regist(TransmitterSteps transmitterStep, ITransmitteStep step)
        {
            ITransmitteStep t;
            if (!_steps.TryGetValue(transmitterStep, out t))
            {
                _steps.Add(transmitterStep, step);
            }
        }

        /// <summary>
        ///     执行下一步传输任务
        /// </summary>
        public virtual void Next(params object[] args)
        {
            ITransmitteStep transmitteStep;
            if (_steps.TryGetValue(_nextStep, out transmitteStep))
            {
                _nextStep = transmitteStep.Do(_subscriber, _context, args);
                ProcessingHandler(new LightSingleArgEventArgs<string>(string.Format("#Processing configuration transmitter: {0}, result: {1}", transmitteStep, _nextStep)));
                if (_nextStep == TransmitterSteps.Exception)
                {
                    ProcessingHandler(new LightSingleArgEventArgs<string>(string.Format("#Processing configuration transmitter: {0} occur exception!", transmitteStep)));
                    return;
                }
                if (_nextStep == TransmitterSteps.Finish)
                {
                    ProcessingHandler(new LightSingleArgEventArgs<string>("#Processing configuration transmitter finished."));
                    return;
                }
            }
            else
            {
                ProcessingHandler(new LightSingleArgEventArgs<string>(string.Format("#Can not find configuration transmitter: {0}.", _nextStep)));
            }
        }

        /// <summary>
        ///     当前进度的发布事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<string>> Processing;
        protected void ProcessingHandler(LightSingleArgEventArgs<string> e)
        {
            EventHandler<LightSingleArgEventArgs<string>> handler = Processing;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}