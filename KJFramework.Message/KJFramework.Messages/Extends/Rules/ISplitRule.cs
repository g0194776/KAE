using System;

namespace KJFramework.Messages.Extends.Rules
{
    /// <summary>
    ///     分割规则元接口，提供了相关的基本操作。
    /// </summary>
    public interface ISplitRule : IDisposable
    {
        /// <summary>
        ///     获取或设置支持的类型
        /// </summary>
        Type SupportType { get; set; }
        /// <summary>
        ///     获取或设置分割长度
        /// </summary>
        int SplitLength { get; set; }

        /// <summary>
        ///     取值检查
        ///     <para>* 此方法通常用于检测可选字段是否拥有值时的判断。</para>
        /// </summary>
        /// <param name="offset">当前元数据的偏移量</param>
        /// <param name="data">元数据</param>
        /// <param name="tagOffset">
        ///     返回的附属偏移量
        ///     <para>* 如果返回的结果是false的话，就需要填写附属偏移量。</para>
        /// </param>
        /// <param name="targetContentLength">当前要截取的内容长度</param>
        /// <returns>返回是否决定取值的结果</returns>
        bool Check(int offset, byte[] data, ref int tagOffset, ref int targetContentLength);
    }
}