using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace KJFramework.Delegate
{
    /// <summary>
    ///     KJFramework������ṩ������ί�У��ṩ����صĻ���������
    ///         * ִ��ί����ʱ������ʹ���̳߳��е��̡߳�
    /// </summary>
    public class LightDelegate : IDisposable
    {
        #region ��Ա

        protected List<ILightDelegateItem> _actions = new List<ILightDelegateItem>();

        #endregion

        #region �鹹����

        ~LightDelegate()
        {
            Dispose();
        }

        #endregion

        #region ����

        /// <summary>
        ///     ��ί���������һ��ί����
        /// </summary>
        /// <param name="lightDelegateItem">ί����</param>
        public void Add(ILightDelegateItem lightDelegateItem)
        {
            if (lightDelegateItem != null)
            {
                _actions.Add(lightDelegateItem);
            }
        }

        /// <summary>
        ///     ��ί�������Ƴ�һ��ί����
        /// </summary>
        /// <param name="lightDelegateItem">ί����</param>
        public void Remove(ILightDelegateItem lightDelegateItem)
        {
            if (lightDelegateItem != null)
            {
                _actions.Remove(lightDelegateItem);
            }
        }

        /// <summary>
        ///     ��ȡί����
        /// </summary>
        /// <returns>����ί����</returns>
        public IEnumerable<ILightDelegateItem> GetDelegates()
        {
            return _actions;
        }

        /// <summary>
        ///     ��ȡί����
        /// </summary>
        /// <returns>����ί����</returns>
        public IEnumerable<ILightDelegateItem> GetDelegates(Func<ILightDelegateItem, bool> predicate)
        {
            return _actions.Where(predicate);
        }

        /// <summary>
        ///     ִ��
        /// </summary>
        /// <param name="objs">ִ�в���</param>
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

        #region ������

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

        #region IDisposable ��Ա

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