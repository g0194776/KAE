using System.Diagnostics;
using KJFramework.Messages.Analysers;

namespace KJFramework.Messages.Objects
{
    /// <summary>
    ///     可序列化字段存根
    /// </summary>
    [DebuggerDisplay("Property: {AnalyseResult.Property}, Value: {Value}")]
    internal struct SerializePropertyStub
    {
        #region Members

        /// <summary>
        ///     获取或设置字段相关值
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        ///     获取或设置字段分析结果
        /// </summary>
        public ToBytesAnalyseResult AnalyseResult { get; set; }

        #endregion
    }
}