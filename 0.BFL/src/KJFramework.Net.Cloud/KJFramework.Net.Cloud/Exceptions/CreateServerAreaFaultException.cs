using KJFramework.Exception;

namespace KJFramework.Net.Cloud.Exceptions
{
    /// <summary>
    ///   创建服务域失败异常。
    /// </summary>
    public class CreateServerAreaFaultException : LightException
    {
        /// <summary>
        ///   创建服务域失败异常。
        /// </summary>
        public CreateServerAreaFaultException(string message)
            : base(message)
        {
            
        }
    }
}