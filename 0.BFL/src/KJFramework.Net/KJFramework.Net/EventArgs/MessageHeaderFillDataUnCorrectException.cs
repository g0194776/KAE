namespace KJFramework.Net.EventArgs
{
    /// <summary>
    ///     消息头数据填充非法异常
    /// </summary>
    /// <remarks>
    ///     当填充的数据不符合规范时, 触发该异常
    ///        * 填充的数据为null
    ///        * 填充的数据长度小于0
    ///        * 填充数据的偏移小于0
    ///        * 填充的数据长度大于规定的消息头长度
    /// </remarks>
    public class MessageHeaderFillDataUnCorrectException : System.Exception
    {
        /// <summary>
        ///     消息头数据填充非法异常
        /// </summary>
        /// <remarks>
        ///     当填充的数据不符合规范时, 触发该异常
        ///        * 填充的数据为null
        ///        * 填充的数据长度不大与0
        /// </remarks>
        public MessageHeaderFillDataUnCorrectException()
            : base("要填充的消息头数据非法 !")
        {
        }       
    }
}
