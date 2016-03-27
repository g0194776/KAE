using System;
using KJFramework.Net.Cloud.Accessors;
using KJFramework.Net.Cloud.Accessors.Rules;
using KJFramework.Net.Cloud.Virtuals.Accessors.Rules;

namespace KJFramework.Net.Cloud.Virtuals.Accessors
{
    /// <summary>
    ///     ���ܷ�����, �ṩ����صĻ�������
    ///     <para>* �˿��ܷ�����������������κι���ķ���</para>
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
        ///     ��ȡһ��������ʹ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="taget">����</param>
        /// <returns>���ط��ʹ���</returns>
        public IAccessRule GetAccessRule<T>(T taget)
        {
            return new PuppetAccessRule(true);
        }

        #endregion
    }
}