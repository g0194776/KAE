using KJFramework.Dynamic.Tables;
using KJFramework.Enums;
using KJFramework.Tracing;
using System;

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
        ///     获取指定组件的通讯隧道
        /// </summary>
        /// <param name="componentName">组件名称</param>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        /// <exception cref="System.Exception">无法找到当前组件的通讯隧道地址，或者创建隧道失败</exception>
        /// <returns>返回指定组件的通讯隧道</returns>
        [Obsolete("KJFramework.Dynamic does not support it anymore.", true)]
        public T GetTunnel<T>(string componentName)
            where T : class
        {
            throw new NotImplementedException();
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
    }
}