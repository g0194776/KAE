using System;
using KJFramework.Net.Cloud.Accessors.Rules;

namespace KJFramework.Net.Cloud.Virtuals.Accessors.Rules
{
    /// <summary>
    ///     傀儡授权访问规则
    /// </summary>
    public class PuppetAccessRule : IAccessRule
    {
        #region Constructor

        /// <summary>
        ///     傀儡授权访问规则
        /// </summary>
        /// <param name="accessed">访问标示</param>
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
        ///     获取一个值，该值标示了当前访问是否被允许
        /// </summary>
        public bool Accessed
        {
            get { return _accessed; }
        }

        #endregion
    }
}