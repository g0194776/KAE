using System;
using System.Collections.Generic;
using KJFramework.Enums;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Enums;
using KJFramework.Statistics;

namespace KJFramework.ServiceModel.Elements
{
    /// <summary>
    ///     ��Ԫ�س����࣬�ṩ����صĻ���������
    /// </summary>
    /// <typeparam name="TChannel">ͨ������</typeparam>
    public abstract class BindingElement<TChannel> : CommunicationObject, IBindingElement<TChannel>
        where TChannel : IServiceChannel
    {
        #region Implementation of IStatisticable<IStatistic>

        protected Dictionary<StatisticTypes, IStatistic> _statistics = new Dictionary<StatisticTypes, IStatistic>();
        protected bool _enable;
        protected CommunicationStates _communicationState;
        protected Guid _id;
        protected bool _isActive;
        protected double _collectInterval;
        protected Type _collectType;
        protected DateTime _createTime;
        protected object _tag;
        protected InfomationCollectorTypes _infomationCollectorType;
        protected string _name;
        protected bool _canBind;
        protected bool _initialized;

        /// <summary>
        /// ��ȡ������ͳ����
        /// </summary>
        public Dictionary<StatisticTypes, IStatistic> Statistics
        {
            get { return _statistics; }
            set { _statistics = value; }
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of ICollector

        /// <summary>
        /// ��ȡΨһ��ʾ
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        /// ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�ռ����Ƿ����
        /// </summary>
        public bool IsActive
        {
            get { return _isActive; }
        }

        public event EventHandler BeginWork;
        protected void BeginWorkHandler(System.EventArgs e)
        {
            EventHandler work = BeginWork;
            if (work != null) work(this, e);
        }

        public event EventHandler EndWork;
        protected void EndWorkHandler(System.EventArgs e)
        {
            EventHandler work = EndWork;
            if (work != null) work(this, e);
        }

        #endregion

        #region Implementation of IBindingElement<TChannel>

        /// <summary>
        ///     ��ȡ��Ԫ������
        /// </summary>
        public string Name
        {
            get { return _name; }
        }
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ��Ԫ���Ƿ���԰�
        /// </summary>
        public bool CanBind
        {
            get { return _canBind; }
        }
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰͨ���������Ƿ��Ѿ���ʼ���ɹ�
        /// </summary>
        public bool Initialized
        {
            get { return _initialized; }
        }
        /// <summary>
        ///     ��ʼ��
        /// </summary>
        public abstract void Initialize();
        /// <summary>
        ///     ����ͨ��
        /// </summary>
        /// <returns>���ش������ͨ��</returns>
        public abstract TChannel CreateChannel();

        #endregion
    }
}