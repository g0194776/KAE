using System;
using System.Collections.Generic;
using KJFramework.Messages.Contracts;

namespace KJFramework.Messages.Extends.Splitters
{
    /// <summary>
    ///     消息元数据字段分割器，提供了相关的基本操作。
    /// </summary>
    public interface IMetadataFieldSplitter
    {
        /// <summary>
        ///     分割字段
        /// </summary>
        /// <param name="target">智能对象</param>
        /// <param name="data">消息元数据</param>
        /// <param name="head">消息头标示部分</param>
        /// <param name="end">消息尾标示部分</param>
        /// <returns>返回分割后的字段集合</returns>
        /// <exception cref="System.Exception">分割失败</exception>
        Dictionary<int, byte[]> Split(IntellectObject target, byte[] data, out Dictionary<int, byte[]> head, out Dictionary<int, byte[]> end);
    }
}