using KJFramework.Basic.Enum;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Uri;

namespace KJFramework.ServiceModel.Elements
{
    /// <summary>
    ///     IPC绑定方式
    /// </summary>
    public class PipeBinding : Binding
    {
        #region 构造函数

        
        /// <summary>
        ///     基于TCP协议的绑定方式， 提供了相关的基本操作。
        /// </summary>
        /// <param name="localAddress">绑定地址</param>
        public PipeBinding(string localAddress)
            : this(new PipeUri(localAddress))
        {
        }

        /// <summary>
        ///     IPC绑定方式
        /// </summary>
        /// <param name="localAddress">远程地址</param>
        public PipeBinding(PipeUri localAddress)
            : base(new PipeBindingElement(), localAddress)
        {
            _bindingType = BindingTypes.Ipc;
        }

        /// <summary>
        ///     IPC绑定方式
        /// </summary>
        /// <param name="bingdingElements">绑定元素</param>
        /// <param name="localAddress">远程地址</param>
        public PipeBinding(BindingElement<IServiceChannel> bingdingElements, PipeUri localAddress)
            : base(bingdingElements, localAddress)
        {
            _bindingType = BindingTypes.Ipc;
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