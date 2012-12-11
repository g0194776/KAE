using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Objects
{
    /// <summary>
    ///     数据列对象，提供了相关的基本属性结构
    /// </summary>
    public class DataColumn : IntellectObject
    {
        #region Members

        /// <summary>
        ///     获取或设置数据列的值
        /// </summary>
        [IntellectProperty(0, IsRequire = false)]
        public string Value { get; set; }
        ///// <summary>
        /////     获取或设置列编号
        /////     <para>* 此编号将会按照数据库中的字段顺序排列对应。</para>
        ///// </summary>
        //[IntellectProperty(1, IsRequire = true)]
        //public short Id { get; set; }
       
        #endregion
    }
}