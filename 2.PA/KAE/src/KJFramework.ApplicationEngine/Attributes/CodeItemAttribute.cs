using System;

namespace KJFramework.ApplicationEngine.Attributes
{
    /// <summary>
    ///     代码项属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CodeItemAttribute : System.Attribute
    {
        #region Members

        private readonly string _itemName;
        /// <summary>
        ///     获取相关名称
        /// </summary>
        public string ItemName
        {
            get { return _itemName; }
        }

        #endregion

        #region Constructor

        /// <summary>
        ///     代码项属性
        /// </summary>
        public CodeItemAttribute(string itemName)
        {
            if (string.IsNullOrEmpty(itemName)) throw new ArgumentNullException("itemName");
            _itemName = itemName;
        }

        #endregion
    }
}