using KJFramework.Exception;

namespace KJFramework.Net.Cloud.Exceptions
{
    /// <summary>
    ///   �������������쳣��
    /// </summary>
    public class ServerAreaErrorOperationException : LightException
    {
        /// <summary>
        ///   �������������쳣��
        /// </summary>
        public ServerAreaErrorOperationException(string message) : base(message)
        {
            
        }
    }
}