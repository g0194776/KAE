using KJFramework.Dynamic.Tables;
using KJFramework.Enums;
using KJFramework.Tracing;
using System;

namespace KJFramework.Dynamic.Components
{
    /// <summary>
    ///     ��̬��������������࣬�ṩ����صĻ���������
    /// </summary>
    public abstract class DynamicDomainComponent : MarshalByRefObject, IDynamicDomainComponent
    {
        #region Constructor

        /// <summary>
        ///     ��̬��������������࣬�ṩ����صĻ���������
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
        ///     ��ȡ�����õ�ǰ����������ķ���
        /// </summary>
        public IDynamicDomainService OwnService { get; set; }

        #endregion

        #region Implementation of IPlugin

        /// <summary>
        ///     ���غ���Ҫ���Ķ���
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
        ///     ��ȡ�����ò����Ϣ
        /// </summary>
        public PluginInfomation PluginInfo
        {
            get { return _pluginInfo; }
        }

        /// <summary>
        /// ��ȡ�����ÿ��ñ�ʾ
        /// </summary>
        public bool Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }

        /// <summary>
        /// ��ȡ�����ò������
        /// </summary>
        public PluginTypes PluginType
        {
            get { return _pluginType; }
        }

        #endregion

        #region Implementation of IDynamicDomainComponent

        /// <summary>
        ///     ��ȡ����
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        ///     ��鵱ǰ����Ľ���״��
        /// </summary>
        /// <returns>���ؽ���״��</returns>
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
        ///     ��ȡΨһ��ʾ
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        ///     ��ȡָ�������ͨѶ���
        /// </summary>
        /// <param name="componentName">�������</param>
        /// <exception cref="ArgumentNullException">��������</exception>
        /// <exception cref="System.Exception">�޷��ҵ���ǰ�����ͨѶ�����ַ�����ߴ������ʧ��</exception>
        /// <returns>����ָ�������ͨѶ���</returns>
        [Obsolete("KJFramework.Dynamic does not support it anymore.", true)]
        public T GetTunnel<T>(string componentName)
            where T : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     ��ʼִ��
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
        ///     ִֹͣ��
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
        ///     ��ȡ�����������ʹ����
        /// </summary>
        internal IDomainObjectVisitRuleTable RuleTable
        {
            get { return _ruleTable; }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     ��ʼִ��
        /// </summary>
        protected abstract void InnerStart();
        /// <summary>
        ///     ִֹͣ��
        /// </summary>
        protected abstract void InnerStop();
        /// <summary>
        ///     ���غ���Ҫ���Ķ���
        /// </summary>
        protected abstract void InnerOnLoading();
        /// <summary>
        ///     ��鵱ǰ����Ľ���״��
        /// </summary>
        /// <returns>���ؽ���״��</returns>
        protected abstract HealthStatus InnerCheckHealth();

        #endregion
    }
}