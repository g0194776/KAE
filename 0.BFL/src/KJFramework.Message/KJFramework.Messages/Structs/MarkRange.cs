using System.Runtime.InteropServices;

namespace KJFramework.Messages.Structs
{
    /// <summary>
    ///     存储序列化byte数组的标识
    /// </summary>
    [StructLayout(LayoutKind.Explicit,CharSet = CharSet.Ansi,Pack = 1, Size = 5)]
    public struct MarkRange
    {
        /// <summary>
        ///     存储对应Value值得标识
        /// </summary>
        [FieldOffset(0)]
        public byte Id;
        /// <summary>
        ///     存储对应Value值得标识
        ///     *高位1 byte为Type Id号，及二进制流对应的存储类型 低位3bytes用于存放相应字段的offset*
        /// </summary>
        [FieldOffset(1)]
        public uint Flag;
    }
}

