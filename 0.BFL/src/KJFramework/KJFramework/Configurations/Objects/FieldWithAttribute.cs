using System;
using System.Reflection;
namespace KJFramework.Configurations.Objects
{
    /// <summary>
    ///     获取字段的标记属性时的临时数据结构
    /// </summary>
    public class FieldWithAttribute<T> : IDisposable
        where T : System.Attribute
    {
        #region 析构函数

        ~FieldWithAttribute()
        {
            Dispose();
        }

        #endregion

        #region IDisposable 成员

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region 成员

        /// <summary>
        ///     获取或设置字段信息
        /// </summary>
        public FieldInfo FieldInfo { get; set; }
        /// <summary>
        ///     获取或设置字段标记属性
        /// </summary>
        public T Attribute { get; set; }

        #endregion
    }
}