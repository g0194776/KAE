using System;
using System.Reflection;
namespace KJFramework.Configurations.Objects
{
    /// <summary>
    ///     字段类型与名称临时结构体
    /// </summary>
    public class FieldWithName : IDisposable
    {
        #region 析构函数

        ~FieldWithName()
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
        ///     获取或设置字段类型
        /// </summary>
        public FieldInfo FieldInfo { get; set; }
        /// <summary>
        ///     获取或设置字段名称
        /// </summary>
        public String Name { get; set; }

        #endregion
    }
}