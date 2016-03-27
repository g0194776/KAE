using System;
using System.Reflection;
namespace KJFramework.Configurations.Objects
{
    /// <summary>
    ///     属性类型与名称临时结构体
    /// </summary>
    public class PropertyWithName : IDisposable
    {
        #region 析构函数

        ~PropertyWithName()
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
        ///     获取或设置属性类型
        /// </summary>
        public PropertyInfo PropertyInfo { get; set; }
        /// <summary>
        ///     获取或设置属性名称
        /// </summary>
        public String Name { get; set; }

        #endregion
    }
}