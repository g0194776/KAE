using KJFramework.Dynamic.Components;
using KJFramework.Dynamic.Structs;
using KJFramework.Logger;

namespace KJFramework.Dynamic.Extends
{
    internal static class DynamicDomainObjectExtend
    {
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
                Logs.Logger.Log(ex);
                return null;
            }
        }

        #endregion
    }
}