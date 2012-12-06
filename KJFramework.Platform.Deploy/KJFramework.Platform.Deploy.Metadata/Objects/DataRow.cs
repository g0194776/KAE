using KJFramework.Logger;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Objects
{
    /// <summary>
    ///     �����ж����ṩ����صĻ������Խṹ
    /// </summary>
    public class DataRow : IntellectObject
    {
        #region Members

        /// <summary>
        ///     ��ȡ��������ֵ
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