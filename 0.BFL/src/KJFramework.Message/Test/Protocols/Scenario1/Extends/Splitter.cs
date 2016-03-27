using System.Collections.Generic;
using KJFramework.IO.Helper;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Extends.Splitters;

namespace Test.Protocols.Scenario1.Extends
{
    public class Splitter : IMetadataFieldSplitter
    {
        #region Implementation of IMetadataFieldSplitter

        /// <summary>
        ///     分割字段
        /// </summary>
        /// <param name="target">目标对象类型</param>
        /// <param name="data">消息元数据</param>
        /// <param name="head">消息头标示部分</param>
        /// <param name="end">消息尾标示部分</param>
        /// <returns>返回分割后的字段集合</returns>
        /// <exception cref="System.Exception">分割失败</exception>
        public Dictionary<int, byte[]> Split(IntellectObject target, byte[] data, out Dictionary<int, byte[]> head, out Dictionary<int, byte[]> end)
        {
            Dictionary<int, byte[]> result = target.Split(ByteArrayHelper.GetNextData(data, 4, data.Length - 4));
            head = new Dictionary<int, byte[]>();
            end = null;
            return result;
        }

        #endregion
    }
}