using System;
using KJFramework.Net.Cloud.Accessors;
using KJFramework.Net.Cloud.Accessors.Rules;
using KJFramework.Net.Cloud.Virtuals.Accessors.Rules;

namespace KJFramework.Net.Cloud.Virtuals.Accessors
{
    /// <summary>
    ///     傀儡访问器, 提供了相关的基本操作
    ///     <para>* 此傀儡访问器，总是允许对任何规则的访问</para>
    /// </summary>
    public class PuppetAccessor : IAccessor
    {
        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     获取一个对象访问规则
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="taget">对象</param>
        /// <returns>返回访问规则</returns>
        public IAccessRule GetAccessRule<T>(T taget)
        {
            return new PuppetAccessRule(true);
        }

        #endregion
    }
}