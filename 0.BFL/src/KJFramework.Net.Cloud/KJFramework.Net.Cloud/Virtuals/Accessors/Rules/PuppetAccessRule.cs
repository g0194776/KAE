using System;
using KJFramework.Net.Cloud.Accessors.Rules;

namespace KJFramework.Net.Cloud.Virtuals.Accessors.Rules
{
    /// <summary>
    ///     ������Ȩ���ʹ���
    /// </summary>
    public class PuppetAccessRule : IAccessRule
    {
        #region Constructor

        /// <summary>
        ///     ������Ȩ���ʹ���
        /// </summary>
        /// <param name="accessed">���ʱ�ʾ</param>
        public PuppetAccessRule(bool accessed)
        {
            _accessed = accessed;
        }

        #endregion

        #region Implementation of IDisposable

        private bool _accessed;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IAccessRule

        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�����Ƿ�����
        /// </summary>
        public bool Accessed
        {
            get { return _accessed; }
        }

        #endregion
    }
}