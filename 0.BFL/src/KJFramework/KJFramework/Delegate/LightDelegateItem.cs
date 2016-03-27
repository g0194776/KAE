using System;

namespace KJFramework.Delegate
{
    /// <summary>
    ///     ����ί����ṩ����صĻ���������
    /// </summary>
    public class LightDelegateItem : ILightDelegateItem
    {
        #region ��Ա

        protected Action<Object[]> _doAction;

        #endregion

        #region �鹹����

        ~LightDelegateItem()
        {
            Dispose();
        }

        #endregion

        #region ���캯��

        /// <summary>
        ///     ����ί����ṩ����صĻ���������
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
        ///     ��ȡ�����ø�������
        /// </summary>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="objs">���в���</param>
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