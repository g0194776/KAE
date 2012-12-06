using KJFramework.Basic.Enum;
using KJFramework.Dynamic.Components;
using KJFramework.Net.Exception;

namespace KJFramework.Net.Cloud.Nodes
{
    /// <summary>
    ///     独立的功能节点，是一种独立运行的功能模块
    ///     <para>* 如果你要执行互不依赖的功能，可以选择使用该功能模块。</para>
    /// </summary>
    /// <typeparam name="T">协议栈中父类消息类型。</typeparam>
    public abstract class IndependentFunctionNode<T> : DynamicDomainComponent, IIndependentFunctionNode<T>
    {
        #region Overrides of DynamicDomainComponent

        private object _tag;

        /// <summary>
        ///     开始执行时被调用
        /// </summary>
        protected abstract override void InnerStart();
        /// <summary>
        ///     停止执行时被调用
        /// </summary>
        protected abstract override void InnerStop();
        /// <summary>
        ///     加载时被调用
        /// </summary>
        protected abstract override void InnerOnLoading();
        /// <summary>
        ///     内部健康检查时被调用
        /// </summary>
        /// <returns></returns>
        protected abstract override HealthStatus InnerCheckHealth();

        #endregion

        #region Implementation of IFunctionNode<T>

        /// <summary>
        ///    获取或设置附属属性
        /// </summary>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        /// <summary>
        ///     初始化
        /// </summary>
        /// <returns>返回初始化后的状</returns>
        /// <exception cref="InitializeFailedException">初始化失败</exception>
        public abstract bool Initialize();

        #endregion
    }
}