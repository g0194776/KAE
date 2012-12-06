namespace KJFramework.Net.EventArgs
{
    /// <summary>
    ///     消息头标示不合法异常
    /// </summary>
    /// <remarks>
    ///     当消息头标示的长度不等于用户设定的消息头标示长度时，触发该异常
    /// </remarks>
    public class MessageHeaderFlagUnCorrectException : System.Exception
    {
        /// <summary>
        ///     消息头标示不合法异常
        /// </summary>
        /// <remarks>
        ///     当消息头标示的长度不等于用户设定的消息头标示长度时，触发该异常
        /// </remarks>
        public MessageHeaderFlagUnCorrectException() : base("消息头标示不合法 !")
        {
        }
    }
}
