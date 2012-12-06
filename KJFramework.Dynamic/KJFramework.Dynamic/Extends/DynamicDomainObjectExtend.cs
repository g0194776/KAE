using KJFramework.Dynamic.Components;
using KJFramework.Dynamic.Structs;
using KJFramework.Logger;

namespace KJFramework.Dynamic.Extends
{
    internal static class DynamicDomainObjectExtend
    {
        #region 方法

        /// <summary>
        ///     将一个程序域组件入口点信息包装成一个动态程序域对象
        /// </summary>
        /// <param name="info">程序域组件入口点信息</param>
        /// <returns>返回动态程序域对象</returns>
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