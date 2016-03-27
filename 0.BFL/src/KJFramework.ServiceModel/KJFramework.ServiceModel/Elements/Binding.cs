using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using KJFramework.Enums;
using KJFramework.Net.Channels;
using KJFramework.Statistics;
using Uri = KJFramework.Net.Channels.Uri.Uri;

namespace KJFramework.ServiceModel.Elements
{
    /// <summary>
    ///     �󶨻��࣬�ṩ����صĻ���������
    /// </summary>
    public abstract class Binding : IBinding
    {
        #region ���캯��

        /// <summary>
        ///     �󶨻��࣬�ṩ����صĻ���������
        /// </summary>
        /// <param name="bingdingElements">��Ԫ�ؼ���</param>
        /// <param name="localAddress">�߼���ַ</param>
        protected Binding(BindingElement<IServiceChannel> bingdingElements, Uri localAddress)
        {
            if (localAddress == null)
            {
                throw new System.Exception("�޷���ʼ���󶨣��߼���ַ����Ϊ�ա�");
            }
            _bingdingElements = bingdingElements;
            _logicalAddress = localAddress;
        }

        #endregion

        #region Implementation of IDisposable

        protected List<BindingElement<IServiceChannel>> _elements = new List<BindingElement<IServiceChannel>>();
        protected IPEndPoint _address;
        private readonly BindingElement<IServiceChannel> _bingdingElements;
        protected Uri _logicalAddress;
        protected Dictionary<StatisticTypes, IStatistic> _statistics = new Dictionary<StatisticTypes, IStatistic>();
        protected string _defaultNamespace;
        protected BindingTypes _bindingType;
        protected bool _initialized;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of ICommunicationChannelAddress

        /// <summary>
        ///     ��ȡ�����������ַ
        /// </summary>
        public IPEndPoint Address
        {
            get { return _address; }
            set { _address = value; }
        }

        /// <summary>
        ///     ��ȡ�������߼���ַ
        /// </summary>
        public Uri LogicalAddress
        {
            get { return _logicalAddress; }
            set { _logicalAddress = value; }
        }

        #endregion

        #region Implementation of IStatisticable<IStatistic>

        /// <summary>
        /// ��ȡ������ͳ����
        /// </summary>
        public Dictionary<StatisticTypes, IStatistic> Statistics
        {
            get { return _statistics; }
            set { _statistics = value; }
        }

        #endregion

        #region Implementation of IBinding

        /// <summary>
        ///     ��ȡĬ�ϵ������ռ�
        /// </summary>
        public String DefaultNamespace
        {
            get { return _defaultNamespace; }
        }

        /// <summary>
        ///     ��ȡ�󶨷�ʽ
        /// </summary>
        public BindingTypes BindingType
        {
            get { return _bindingType; }
        }

        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�Ƿ��Ѿ���ʼ���ɹ�
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
        ///     ��ȡ���а�Ԫ��
        /// </summary>
        /// <returns></returns>
        public virtual BindingElement<TChannel>[] GetBindingElements<TChannel>() where TChannel : IServiceChannel, new()
        {
            if (_elements.Count == 0)
            {
                return null;
            }
            return _elements.Cast<BindingElement<TChannel>>().ToArray();
        }

        #endregion
    }
}