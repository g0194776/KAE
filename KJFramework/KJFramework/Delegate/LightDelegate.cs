using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace KJFramework.Delegate
{
    /// <summary>
    ///     KJFramework框架中提供的轻型委托，提供了相关的基本操作。
    ///         * 执行委托项时，将会使用线程池中的线程。
    /// </summary>
    public class LightDelegate : IDisposable
    {
        #region 成员

        protected List<ILightDelegateItem> _actions = new List<ILightDelegateItem>();

        #endregion

        #region 虚构函数

        ~LightDelegate()
        {
            Dispose();
        }

        #endregion

        #region 方法

        /// <summary>
        ///     向委托链中添加一个委托项
        /// </summary>
        /// <param name="lightDelegateItem">委托项</param>
        public void Add(ILightDelegateItem lightDelegateItem)
        {
            if (lightDelegateItem != null)
            {
                _actions.Add(lightDelegateItem);
            }
        }

        /// <summary>
        ///     从委托链中移除一个委托项
        /// </summary>
        /// <param name="lightDelegateItem">委托项</param>
        public void Remove(ILightDelegateItem lightDelegateItem)
        {
            if (lightDelegateItem != null)
            {
                _actions.Remove(lightDelegateItem);
            }
        }

        /// <summary>
        ///     获取委托链
        /// </summary>
        /// <returns>返回委托链</returns>
        public IEnumerable<ILightDelegateItem> GetDelegates()
        {
            return _actions;
        }

        /// <summary>
        ///     获取委托链
        /// </summary>
        /// <returns>返回委托链</returns>
        public IEnumerable<ILightDelegateItem> GetDelegates(Func<ILightDelegateItem, bool> predicate)
        {
            return _actions.Where(predicate);
        }

        /// <summary>
        ///     执行
        /// </summary>
        /// <param name="objs">执行参数</param>
        public void Execute(params Object[] objs)
        {
            if (_actions != null && _actions.Count > 0)
            {
                lock (_actions)
                {
                    foreach (ILightDelegateItem item in _actions)
                    {
                        ILightDelegateItem delegateItem = item;
                        ThreadPool.QueueUserWorkItem(
                                delegate
                                {
                                    delegateItem.Execute(objs);
                                });
                    }
                }
            }
        }

        #endregion

        #region 操作符

        public static LightDelegate operator +(LightDelegate selft, ILightDelegateItem item)
        {
            if (selft != null)
            {
                selft.Add(item);
            }
            return selft;
        }

        public static LightDelegate operator -(LightDelegate selft, ILightDelegateItem item)
        {
            if (selft != null)
            {
                selft.Remove(item);
            }
            return selft;
        }

        #endregion

        #region IDisposable 成员

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
           GC.SuppressFinalize(this);
        }

        #endregion
    }
}