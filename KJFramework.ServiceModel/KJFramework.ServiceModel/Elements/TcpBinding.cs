using KJFramework.Enums;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Uri;

namespace KJFramework.ServiceModel.Elements
{
    /// <summary>
    ///     基于TCP协议的绑定方式， 提供了相关的基本操作。
    /// </summary>
    public class TcpBinding : Binding
    {
        #region 构造函数

        /// <summary>
        ///     基于TCP协议的绑定方式， 提供了相关的基本操作。
        /// </summary>
        /// <param name="localAddress">绑定地址</param>
        public TcpBinding(string localAddress)
            : this(new TcpUri(localAddress))
        {
        }

        /// <summary>
        ///     基于TCP协议的绑定方式， 提供了相关的基本操作。
        /// </summary>
        /// <param name="localAddress">绑定地址</param>
        public TcpBinding(TcpUri localAddress)
            : base(new TcpBindingElement(), localAddress)
        {
            _bindingType = BindingTypes.Tcp;
        }

        /// <summary>
        ///     基于TCP协议的绑定方式， 提供了相关的基本操作。
        /// </summary>
        /// <param name="bingdingElements">绑定元素</param>
        /// <param name="localAddress">绑定地址</param>
        public TcpBinding(BindingElement<IServiceChannel> bingdingElements, TcpUri localAddress)
            : base(bingdingElements, localAddress)
        {
            _bindingType = BindingTypes.Tcp;
        }

        #endregion

        #region Overrides of Binding

        /// <summary>
        ///     初始化
        /// </summary>
        public override void Initialize()
        {
        }

        #endregion
    }
}