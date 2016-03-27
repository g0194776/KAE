using System;

namespace KJFramework.Delegate
{
    /// <summary>
    ///     轻型委托项，提供了相关的基本操作。
    /// </summary>
    public class LightDelegateItem : ILightDelegateItem
    {
        #region 成员

        protected Action<Object[]> _doAction;

        #endregion

        #region 虚构函数

        ~LightDelegateItem()
        {
            Dispose();
        }

        #endregion

        #region 构造函数

        /// <summary>
        ///     轻型委托项，提供了相关的基本操作。
        /// </summary>
        public LightDelegateItem(Action<Object[]> doAction)
        {
            _doAction = doAction;
        }

        #endregion

        #region Implementation of IDisposable

        private object _tag;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of ILightDelegateItem

        /// <summary>
        ///     获取或设置附属属性
        /// </summary>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        /// <summary>
        ///     运行
        /// </summary>
        /// <param name="objs">运行参数</param>
        public virtual void Execute(params Object[] objs)
        {
            if (_doAction != null)
            {
                _doAction(objs);
            }
        }

        #endregion
    }
}