using KJFramework.Logger;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Objects
{
    /// <summary>
    ///     数据行对象，提供了相关的基本属性结构
    /// </summary>
    public class DataRow : IntellectObject
    {
        #region Members

        /// <summary>
        ///     获取或设置列值
        /// </summary>
        [IntellectProperty(0, IsRequire = false)]
        public DataColumn[] Columns { get; set; }

        #endregion

        #region Indexer

        //public string this[int id]
        //{
        //    get
        //    {
        //        try
        //        {
        //            return Columns[id].Value;
        //        }
        //        catch(System.Exception ex)
        //        {
        //            Logs.Logger.Log(ex);
        //            return null;
        //        }
        //    }
        //}

        #endregion
    }
}