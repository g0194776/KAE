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
        protected bool _isUseTunnel;
        protected string _tunnelAddress;
        private ServiceHost _tunnelHost;
        private IComponentTunnelVisitor _tunnelVisitor;
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
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�Ƿ��������ͨѶ�������
        /// </summary>
        public bool IsUseTunnel
        {
            get { return _isUseTunnel; }
            internal set { _isUseTunnel = value; }
        }

        /// <summary>
        ///     ��ȡ�����ͨѶ����ĵ�ַ
        ///     <para>* �����������IsUseTunnel = trueʱ��������</para>
        /// </summary>
        /// <exception cref="NotSupportedException">��֧�ָù���</exception>
        /// <returns>���������ַ</returns>
        public string GetTunnelAddress()
        {
            if (!_isUseTunnel)
                throw new NotSupportedException("#Cannot get tunnel address, beacuse this component don't use this feature. #name: " + _pluginInfo.ServiceName);
            return _tunnelAddress;
        }

        /// <summary>
        ///     ��ȡ���������
        /// </summary>
        public IComponentTunnelVisitor TunnelVisitor
        {
            get { return _tunnelVisitor; }
        }

        /// <summary>
        ///     �������п���ϵ����������ַ
        /// </summary>
        /// <param name="addresses">�����ַ</param>
        public void SetTunnelAddresses(Dictionary<string, string> addresses)
        {
            _tunnelVisitor = new ComponentTunnelVisitor(addresses);
        }

        /// <summary>
        ///     ��ȡָ�������ͨѶ���
        /// </summary>
        /// <param name="componentName">�������</param>
        /// <exception cref="ArgumentNullException">��������</exception>
        /// <exception cref="System.Exception">�޷��ҵ���ǰ�����ͨѶ�����ַ�����ߴ������ʧ��</exception>
        /// <returns>����ָ�������ͨѶ���</returns>
        public T GetTunnel<T>(string componentName)
            where T : class
        {
            if (componentName == null) throw new ArgumentNullException("componentName");
            if (_tunnelVisitor == null) throw new System.Exception("�޷���ȡһ��������������Ϊ��û��Ϊ��ǰ�������һ�����������!");
            return _tunnelVisitor.GetTunnel<T>(componentName);
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
                //�ر�ͨѶ���
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
        ///     ʹ������������
        ///     <para>* ���ô˷����� ���Ὺ���������ͨѶ������ܣ�ʹ�ô�������Ա������������</para>
        /// </summary>
        /// <param name="metadataExchange">
        ///     Ԫ���ݿ��ű�ʾ
        ///     <para>* Ĭ��Ϊ������Ԫ����</para>
        /// </param>
        /// <exception cref="System.Exception">����ʧ��</exception>
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