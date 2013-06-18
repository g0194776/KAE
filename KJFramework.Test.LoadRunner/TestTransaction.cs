using System;
using KJFramework.Configurations;
using KJFramework.Configurations.Items;
using KJFramework.Dynamic.Configurations;
using KJFramework.Net.Channels.Configurations;
using KJFramework.Net.Configurations;
using KJFramework.Tracing;
using LoadRunner;

namespace KJFramework.Test.LoadRunner
{
    /// <summary>
    ///     测试事务
    /// </summary>
    public class TestTransaction<TContext> : ITestTransaction<TContext>
        where TContext : ITestContext
    {
        #region Constructor

        /// <summary>
        ///     测试事务
        /// </summary>
        /// <param name="info">测试信息</param>
        /// <param name="api">LoadRunner API</param>
        public TestTransaction(ITestInfo info, LrApi api)
        {
            FillConfiguration();
            if (info == null) throw new ArgumentNullException("info");
            _api = api ?? new LrApi();
            _info = info;
            _tracing = TracingManager.GetTracing(typeof(TestTransaction<TContext>));
        }

        #endregion

        #region Implementation of ITestTransaction

        protected TContext _context;
        protected readonly ITestInfo _info;
        protected ITestUnit<TContext> _beginUnit;
        protected readonly LrApi _api;
        protected readonly ITracing _tracing;

        /// <summary>
        ///     获取或设置测试上下文
        /// </summary>
        public TContext Context
        {
            get { return _context; }
            set { _context = value; }
        }

        /// <summary>
        ///     获取测试信息
        /// </summary>
        public ITestInfo Info
        {
            get { return _info; }
        }

        /// <summary>
        ///     设置测试单元，整个测试事务将从这个测试单元开始执行
        /// </summary>
        /// <param name="unit">测试单元</param>
        public void SetTestUnit(ITestUnit<TContext> unit)
        {
            if (unit == null) throw new ArgumentNullException("unit");
            _beginUnit = unit;
        }

        /// <summary>
        ///     开始执行
        /// </summary>
        public void Start()
        {
            if (_beginUnit == null) throw new System.Exception("You *MUST* set a TestUnit first!!!");
            if (_context == null) throw new System.Exception("Context cannot be null!!!");
            _api.start_transaction(_info.Name);
            try
            {
                ITestUnit<TContext> currentUnit = _beginUnit;
                do
                {
                    _api.start_sub_transaction(currentUnit.Info.Name, _info.Name);
                    if (!currentUnit.Execute(_context))
                    {
                        _api.error_message("#TestUnit: " + currentUnit + " has failed to perform!");
                        _api.end_sub_transaction(currentUnit.Info.Name, _api.FAIL);
                        _api.end_transaction(_info.Name, _api.FAIL);
                        return;
                    }
                    _api.end_sub_transaction(currentUnit.Info.Name, _api.PASS);
                }
                while ((currentUnit = currentUnit.Next) != null);
                _api.end_transaction(_info.Name, _api.PASS);
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                _api.error_message(ex.Message);
                _api.end_transaction(_info.Name, _api.FAIL);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     填充默认的KJFramework配置值
        /// </summary>
        protected virtual void FillConfiguration()
        {
            SystemConfigurationSection.Current = new SystemConfigurationSection
                {
                    ConfigurationSettingsItem =new ConfigurationSettingsItem
                        {
                            LogPath = string.Format("C:\\LoadRunner-UnitTest-Test-Base-{0}\\", DateTime.Now.Ticks)
                        }
                };
            LocalConfiguration.Current = new LocalConfiguration
                {
                    NetworkLayer = new NetworkLayerConfiguration
                        {
                            BufferSize = 10240,
                            BufferPoolSize = 4096,
                            MessageHeaderLength = 80,
                            MessageHeaderFlag = "#KJMS",
                            MessageHeaderFlagLength = 5,
                            MessageHeaderEndFlag = "�",
                            MessageHeaderEndFlagLength = 1,
                            MessageDealerFolder = "C:\\Dealers\\",
                            MessageHookFolder = "C:\\Hooks\\",
                            SpyFolder = "C:\\Spys\\",
                            BasicSessionStringTemplate = "BASE-KEY:{USERID:{0}}-TIME:{1}",
                            UserHreatCheckTimeSpan = 10000,
                            UserHreatTimeout = 15000,
                            UserHreatAlertCount = 3,
                            FSHreatCheckTimeSpan = 10000,
                            FSHreatTimeout = 15000,
                            FSHreatAlertCount = 3,
                            SessionExpireCheckTimeSpan = 5000,
                            DefaultConnectionPoolConnectCount = 1024,
                            DefaultChannelGroupLayer = 3,
                            DefaultDecleardSize = 1024,
                            PredominantCpuUsage = 10,
                            PredominantMemoryUsage = 150
                        }
                };
            ChannelModelSettingConfigSection.Current = new ChannelModelSettingConfigSection
                {
                    Settings = new SettingConfiguration
                        {
                            RecvBufferSize = 20480,
                            BuffStubPoolSize = 10000,
                            NoBuffStubPoolSize = 10000,
                            MaxMessageDataLength = 19456,
                            SegmentSize = 5120,
                            SegmentBuffer = 1024000
                        }
                };
            ServiceDescriptionConfigSection.Current = new ServiceDescriptionConfigSection
                {
                    Details =
                        new InfoConfiguration
                            {
                                Name = "LoadTest Service",
                                Description = "",
                                ServiceName = "KJFramework.Test.Framework.LoadTestUnit",
                                Version = "1.0.0.0"
                            }
                };
        }

        #endregion
    }
}