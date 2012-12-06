using KJFramework.Messages.Analysers;

namespace KJFramework.Messages.Objects
{
    /// <summary>
    ///     中转用的解析结构体
    /// </summary>
    internal struct TempParseStruct
    {
        /// <summary>
        ///     获取或设置二进制数据
        /// </summary>
        public byte[] Data { get; set; }
        /// <summary>
        ///     获取或设置分析结果
        /// </summary>
        public GetObjectAnalyseResult AnalyzeResult { get; set; }
    }
}