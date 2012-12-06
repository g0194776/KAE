using System;

namespace KJFramework.Attribute
{
    /// <summary>
    ///     自定义配置节属性字段标签属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class CustomerFieldAttribute : System.Attribute
    {
        #region 构造函数

        /// <summary>
        ///     自定义配置节标签属性
        /// </summary>
        /// <param name="name">配置节名称</param>
        public CustomerFieldAttribute(string name)
        {
            _name = name;
        }

        /// <summary>
        ///     自定义配置节标签属性
        /// </summary>
        /// <param name="name">配置节名称</param>
        /// <param name="defaultValue">默认值</param>
        public CustomerFieldAttribute(string name, object defaultValue)
        {
            _name = name;
            _defaultValue = defaultValue;
        }

        #endregion

        #region 成员

        private String _name;

        /// <summary>
        ///     获取自定义配置节名称
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        private Object _defaultValue;
        /// <summary>
        ///     获取或设置默认值
        /// </summary>
        public object DefaultValue
        {
            get { return _defaultValue; }
        }

        private bool _isList;
        /// <summary>
        ///     获取或设置当前字段是否为一个集合的标示
        /// </summary>
        public bool IsList
        {
            get { return _isList; }
            set { _isList = value; }
        }

        private Type _elementType;
        /// <summary>
        ///     获取或设置当前内部元素的类型
        ///            *　仅当IsList == true的时候有效。
        /// </summary>
        public Type ElementType
        {
            get { return _elementType; }
            set { _elementType = value; }
        }

        private String _elementName;
        /// <summary>
        ///     获取或设置当前内部元素的配置项名称
        ///            *　仅当IsList == true的时候有效。
        /// </summary>
        public string ElementName
        {
            get { return _elementName; }
            set { _elementName = value; }
        }



        #endregion
    }
}