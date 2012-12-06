using System;

namespace KJFramework.Attribute
{
    /// <summary>
    ///     自定义配置节标签属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CustomerSectionAttribute : System.Attribute
    {
        #region 构造函数

        /// <summary>
        ///     自定义配置节标签属性
        /// </summary>
        /// <param name="name">配置节名称</param>
        public CustomerSectionAttribute(string name)
        {
            _name = name;
        }

        #endregion

        #region 成员

        private String _name;
        private bool _remoteConfig;

        /// <summary>
        ///     获取自定义配置节名称
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        ///     获取或设置一个值，该值标示了当前的配置节是否从远程获取
        /// </summary>
        public bool RemoteConfig
        {
            get { return _remoteConfig; }
            set { _remoteConfig = value; }
        }

        #endregion
    }
}