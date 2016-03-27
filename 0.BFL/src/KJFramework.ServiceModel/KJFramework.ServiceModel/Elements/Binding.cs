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
    ///     绑定基类，提供了相关的基本操作。
    /// </summary>
    public abstract class Binding : IBinding
    {
        #region 构造函数

        /// <summary>
        ///     绑定基类，提供了相关的基本操作。
        /// </summary>
        /// <param name="bingdingElements">绑定元素集合</param>
        /// <param name="localAddress">逻辑地址</param>
        protected Binding(BindingElement<IServiceChannel> bingdingElements, Uri localAddress)
        {
            if (localAddress == null)
            {
                throw new System.Exception("无法初始化绑定，逻辑地址不能为空。");
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
        ///     获取或设置物理地址
        /// </summary>
        public IPEndPoint Address
        {
            get { return _address; }
            set { _address = value; }
        }

        /// <summary>
        ///     获取或设置逻辑地址
        /// </summary>
        public Uri LogicalAddress
        {
            get { return _logicalAddress; }
            set { _logicalAddress = value; }
        }

        #endregion

        #region Implementation of IStatisticable<IStatistic>

        /// <summary>
        /// 获取或设置统计器
        /// </summary>
        public Dictionary<StatisticTypes, IStatistic> Statistics
        {
            get { return _statistics; }
            set { _statistics = value; }
        }

        #endregion

        #region Implementation of IBinding

        /// <summary>
        ///     获取默认的命名空间
        /// </summary>
        public String DefaultNamespace
        {
            get { return _defaultNamespace; }
        }

        /// <summary>
        ///     获取绑定方式
        /// </summary>
        public BindingTypes BindingType
        {
            get { return _bindingType; }
        }

        /// <summary>
        ///     获取一个值，该值标示了当前是否已经初始化成功
        /// </summary>
        public bool Initialized
        {
            get { return _initialized; }
        }

        /// <summary>
        ///     初始化
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        ///     获取所有绑定元素
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