using System.Reflection.Emit;

namespace KJFramework.Reflection
{
    /// <summary>
    ///     Microsoft MSIL Lanauage 结构元接口
    ///    <para>* 此结构保存每一条MSIL指令的相关信息。</para>
    /// </summary>
    public interface IMsilInstruction
    {
        /// <summary>
        ///     获取或设置指令
        /// </summary>
        OpCode Code { get; set; }
        /// <summary>
        ///     获取或设置操作
        /// </summary>
        object Operand { get; set; }
        /// <summary>
        ///     相关操作元数据
        /// </summary>
        byte[] OperandData { get; set; }
        /// <summary>
        ///     当前指令的偏移地址
        /// </summary>
        int Offset { get; set; }
        /// <summary>
        ///     输出关于此条指令的可视化语句
        /// </summary>
        /// <returns>返回可视化语句</returns>
        string GetCode();
    }
}