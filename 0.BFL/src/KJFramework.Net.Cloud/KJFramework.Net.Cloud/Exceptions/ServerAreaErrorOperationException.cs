using KJFramework.Exception;

namespace KJFramework.Net.Cloud.Exceptions
{
    /// <summary>
    ///   服务域错误操作异常。
    /// </summary>
    public class ServerAreaErrorOperationException : LightException
    {
        /// <summary>
        ///   服务域错误操作异常。
        /// </summary>
        public ServerAreaErrorOperationException(string message) : base(message)
        {
            
        }
    }
}