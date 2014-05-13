using KJFramework.Dynamic.Tables;
using KJFramework.Dynamic.Visitors;
using KJFramework.Enums;
using KJFramework.ServiceModel.Bussiness.Default.Services;
using KJFramework.ServiceModel.Elements;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;

namespace KJFramework.Dynamic.Components
{
    /// <summary>
    ///     动态程序域组件抽象父类，提供了相关的基本操作。
    /// </summary>
    public abstract class DynamicDomainComponent : MarshalByRefObject, IDynamicDomainComponent
    {
        #region Constructor

        /// <summary>
        ///     动态程序域组件抽象父类，提供了相关的基本操作。
        /// </summary>
        protected DynamicDomainComponent()
        {
            _id = Guid.NewGuid();
            _ruleTable = new DomainObjectVisitRuleTable();
            //by default.
            _name = GetType().Name;
        }

        #endregion

        #region Members

        private readonly Guid _id;
        protected bool _enable;
        protected String _name;
        protected PluginTypes _pluginType;
        protected PluginInfomation _pluginInfo;
        protected IDomainObjectVisitRuleTable _ruleTable;
        protected bool _isUseTunnel;
        protected string _tunnelAddress;
        private ServiceHost _tunnelHost;
        private IComponentTunnelVisitor _tunnelVisitor;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(DynamicDomainComponent));

        /// <summary>
        ///     获取或设置当前组件所宿主的服务
        /// </summary>
        public IDynamicDomainService OwnService { get; set; }

        #endregion

        #region Implementation of IPlugin

        /// <summary>
        ///     加载后需要做的动作
        /// </summary>
        public void OnLoading()
        {
            try { InnerOnLoading(); }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                throw;
            }
        }

        /// <summary>
        ///     获取或设置插件信息
        /// </summary>
        public PluginInfomation PluginInfo
        {
            get { return _pluginInfo; }
        }

        /// <summary>
        /// 获取或设置可用标示
        /// </summary>
        public bool Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }

        /// <summary>
        /// 获取或设置插件类型
        /// </summary>
        public PluginTypes PluginType
        {
            get { return _pluginType; }
        }

        #endregion

        #region Implementation of IDynamicDomainComponent

        /// <summary>
        ///     获取名称
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        ///     检查当前组件的健康状况
        /// </summary>
        /// <returns>返回健康状况</returns>
        public HealthStatus CheckHealth()
        {
            try { return InnerCheckHealth(); }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                return HealthStatus.Death;
            }
        }

        /// <summary>
        ///     获取唯一标示
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        ///     获取一个值，该值表示了当前是否开启了组件通讯隧道技术
        /// </summary>
        public bool IsUseTunnel
        {
            get { return _isUseTunnel; }
            internal set { _isUseTunnel = value; }
        }

        /// <summary>
        ///     获取此组件通讯隧道的地址
        ///     <para>* 仅当该组件的IsUseTunnel = true时才有意义</para>
        /// </summary>
        /// <exception cref="NotSupportedException">不支持该功能</exception>
        /// <returns>返回隧道地址</returns>
        public string GetTunnelAddress()
        {
            if (!_isUseTunnel)
                throw new NotSupportedException("#Cannot get tunnel address, beacuse this component don't use this feature. #name: " + _pluginInfo.ServiceName);
            return _tunnelAddress;
        }

        /// <summary>
        ///     获取组件访问器
        /// </summary>
        public IComponentTunnelVisitor TunnelVisitor
        {
            get { return _tunnelVisitor; }
        }

        /// <summary>
        ///     设置所有可联系组件的隧道地址
        /// </summary>
        /// <param name="addresses">隧道地址</param>
        public void SetTunnelAddresses(Dictionary<string, string> addresses)
        {
            _tunnelVisitor = new ComponentTunnelVisitor(addresses);
        }

        /// <summary>
        ///     获取指定组件的通讯隧道
        /// </summary>
        /// <param name="componentName">组件名称</param>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        /// <exception cref="System.Exception">无法找到当前组件的通讯隧道地址，或者创建隧道失败</exception>
        /// <returns>返回指定组件的通讯隧道</returns>
        public T GetTunnel<T>(string componentName)
            where T : class
        {
            if (componentName == null) throw new ArgumentNullException("componentName");
            if (_tunnelVisitor == null) throw new System.Exception("无法获取一个组件的隧道，因为还没有为当前组件创建一个组件访问器!");
            return _tunnelVisitor.GetTunnel<T>(componentName);
        }

        /// <summary>
        ///     开始执行
        /// </summary>
        public void Start()
        {
            try
            {
                InnerStart();
                _enable = true;
            }
            catch (System.Exception ex)
            {
                _enable = false;
                _tracing.Error(ex, null);
                throw;
            }
        }

        /// <summary>
        ///     停止执行
        /// </summary>
        public void Stop()
        {
            try
            {
                //关闭通讯隧道
                if (_tunnelHost != null)
                {
                    _tunnelHost.Opened -= TunnelHostOpened;
                    _tunnelHost.Closed -= TunnelHostClosed;
                    _tunnelHost.Close();
                    _tunnelHost = null;
                }
                InnerStop();
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                throw;
            }
            finally { _enable = false; }
        }

        /// <summary>
        ///     使用组件隧道技术
        ///     <para>* 调用此方法， 将会开启该组件的通讯隧道功能，使得此组件可以被其他组件访问</para>
        /// </summary>
        /// <param name="metadataExchange">
        ///     元数据开放标示
        ///     <para>* 默认为不开放元数据</para>
        /// </param>
        /// <exception cref="System.Exception">开启失败</exception>
        public void UseTunnel<T>(bool metadataExchange = false)
        {
            _isUseTunnel = true;
            try
            {
                _tunnelAddress = string.Format("PIPE://./{0}.{1}", DateTime.Now.Ticks, _pluginInfo.ServiceName);
                _tunnelHost = new ServiceHost(typeof(T), new PipeBinding(_tunnelAddress)) { IsSupportExchange = metadataExchange };
                _tunnelHost.Opened += TunnelHostOpened;
                _tunnelHost.Closed += TunnelHostClosed;
                _tunnelHost.Open();
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                throw;
            }
        }

        /// <summary>
        ///     获取程序域对象访问规则表
        /// </summary>
        internal IDomainObjectVisitRuleTable RuleTable
        {
            get { return _ruleTable; }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     开始执行
        /// </summary>
        protected abstract void InnerStart();
        /// <summary>
        ///     停止执行
        /// </summary>
        protected abstract void InnerStop();
        /// <summary>
        ///     加载后需要做的动作
        /// </summary>
        protected abstract void InnerOnLoading();
        /// <summary>
        ///     检查当前组件的健康状况
        /// </summary>
        /// <returns>返回健康状况</returns>
        protected abstract HealthStatus InnerCheckHealth();

        #endregion

        #region Events

        private void TunnelHostClosed(object sender, System.EventArgs e)
        {
            #if(DEBUG)
            Console.WriteLine("#Component tunnel closed: " + _tunnelAddress);
            #endif
        }

        private void TunnelHostOpened(object sender, System.EventArgs e)
        {
            #if(DEBUG)
            Console.WriteLine("#Component tunnel opened: " + _tunnelAddress);
            #endif
        }

        #endregion
    }
}