namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     消息入口点为空异常
    /// </summary>
    public class MessageEntryHasNullException : System.Exception
    {
        /// <summary>
        ///     消息入口点为空异常
        /// </summary>
        public MessageEntryHasNullException() : base("消息入口点不能为空 !")
        {
            
        }
    }
}