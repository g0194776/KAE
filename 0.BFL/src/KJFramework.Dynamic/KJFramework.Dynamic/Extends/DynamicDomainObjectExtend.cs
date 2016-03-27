using KJFramework.Dynamic.Components;
using KJFramework.Dynamic.Structs;
using KJFramework.Tracing;

namespace KJFramework.Dynamic.Extends
{
    internal static class DynamicDomainObjectExtend
    {
        #region Members

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(DynamicDomainObjectExtend));

        #endregion

        #region ����

        /// <summary>
        ///     ��һ�������������ڵ���Ϣ��װ��һ����̬���������
        /// </summary>
        /// <param name="info">�����������ڵ���Ϣ</param>
        /// <returns>���ض�̬���������</returns>
        public static DynamicDomainObject Wrap(this DomainComponentEntryInfo info)
        {
            try { return new DynamicDomainObject(info); }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                return null;
            }
        }

        #endregion
    }
}