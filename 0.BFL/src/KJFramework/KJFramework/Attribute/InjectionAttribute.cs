using System;

namespace KJFramework.Attribute
{
    /// <summary>
    ///     允许注入标签, 为今后的自动加载提供了必要的属性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class, AllowMultiple = false)]
    public class InjectionAttribute : System.Attribute
    {
        #region 构造函数

        /// <summary>
        ///     允许注入标签, 为今后的自动加载提供了必要的属性。
        /// </summary>
        /// <param name="name">匹配名称</param>
        public InjectionAttribute(String name)
        {
            _name = name;
        }

        #endregion

        #region 成员

        private String _name;
        /// <summary>
        ///     获取或设置注入的匹配名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion
    }
}
