using System;
using System.Collections.Generic;

namespace KJFramework.Logger.LogObject
{
    /// <summary>
    ///     可序列化的错误记录集合，用于序列化后保存成记录文件。
    /// </summary>
    [Serializable]
    public class SerializableDebugLogList
    {
        /// <summary>
        ///     错误记录集合
        /// </summary>
        public List<IDebugLog> Logs = new List<IDebugLog>(); 
    }
}
