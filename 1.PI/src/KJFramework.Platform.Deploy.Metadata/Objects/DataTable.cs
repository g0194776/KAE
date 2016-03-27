using System.Collections.Generic;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Objects
{
    /// <summary>
    ///     ���ݱ�����ṩ����صĻ������Խṹ
    /// </summary>
    public class DataTable : IntellectObject
    {
        #region Members

        /// <summary>
        ///     ��ȡ������������
        /// </summary>
        [IntellectProperty(0, IsRequire = false)]
        public DataRow[] Rows { get; set; }
        /// <summary>
        ///     ��ȡ��������������
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public string[] Columns { get; set; }

        public string GetDataRowBySearchKey(string searchKey)
        {
            int keyIndex = 1;//Key�е�����
            int valueIndex = 2;//Value�е�����
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