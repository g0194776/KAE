using System;

namespace KJFramework.Messages.Attributes
{
    /// <summary>
    ///     智能属性标签，提供了自动编译属性的基础能力支持。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IntellectPropertyAttribute : System.Attribute, IIntellectProperty
    {
        #region 构造函数

        /// <summary>
        ///     智能属性标签，提供了自动编译属性的基础能力支持。
        /// </summary>
        /// <param name="id">顺序编号</param>
        public IntellectPropertyAttribute(int id) : this(id, false)
        {
        }

        /// <summary>
        ///     智能属性标签，提供了自动编译属性的基础能力支持。
        /// </summary>
        /// <param name="id">顺序编号</param>
        /// <param name="isRequire">标示了当前属性是否必须拥有值</param>
        public IntellectPropertyAttribute(int id, bool isRequire)
        {
            _id = id;
            _isRequire = isRequire;
        }

        #endregion

        #region 成员

        private int _id;
        /// <summary>
        ///     获取或设置属性顺序编号
        ///     <para>* 此编号不能重复。</para>
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private bool _isRequire;
        /// <summary>
        ///     获取或设置一个值，该值标示了当前属性是否必须拥有值。
        /// </summary>
        public bool IsRequire
        {
            get { return _isRequire; }
            set { _isRequire = value; }
        }

        private bool _allowDefaultNull;
        /// <summary>
        ///     获取或设置一个值，该值标示了当前字段是否支持"默认值不参与传输"特性
        ///     <para>* 此特性仅对值类型字段才有意义</para>
        ///     <para>* 如果您将该属性设置为true, 并且被标记字段当前包含的值等于我们框架内部所设置的默认值，则此字段将会不参与序列化过程</para>
        /// </summary>
        public bool AllowDefaultNull
        {
            get { return _allowDefaultNull; }
            set { _allowDefaultNull = value; }
        }

        #region Customer Message Region

        private bool _needExtendAction;
        /// <summary>
        ///     获取或设置一个值，该值标示了当前属性是否需要进行扩展构造动作。
        ///     <para>* 此属影响范围：第三方消息结构定义器。</para>
        /// </summary>
        public bool NeedExtendAction
        {
            get { return _needExtendAction; }
            set { _needExtendAction = value; }
        }

        private string _tag;
        /// <summary>
        ///     获取或设置附属名称
        ///     <para>* 此属影响范围：第三方消息结构定义器。</para>
        /// </summary>
        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        #endregion

        #endregion
    }
}