using System.Collections.Generic;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Objects
{
    /// <summary>
    ///     数据表对象，提供了相关的基本属性结构
    /// </summary>
    public class DataTable : IntellectObject
    {
        #region Members

        /// <summary>
        ///     获取或设置数据行
        /// </summary>
        [IntellectProperty(0, IsRequire = false)]
        public DataRow[] Rows { get; set; }
        /// <summary>
        ///     获取或设置列名集合
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public string[] Columns { get; set; }

        public string GetDataRowBySearchKey(string searchKey)
        {
            int keyIndex = 1;//Key列的索引
            int valueIndex = 2;//Value列的索引
            searchKey = searchKey.ToLower();
            for (int i = 0; i < Rows.Length; i++)
            {
                if (Rows[i].Columns[keyIndex].Value == searchKey)
                {
                    return Rows[i].Columns[valueIndex].Value;
                }
            }
            return null;
        }

        public string[] GetValuesBySearchKey(string[] searchKeys)
        {
            List<string> list = new List<string>();
            string value;
            foreach (var key in searchKeys)
            {
                value = GetDataRowBySearchKey(key);
                if (!string.IsNullOrEmpty(value))
                {
                    list.Add(value);
                }
            }
            return list.ToArray();
        }

        #endregion
    }
}