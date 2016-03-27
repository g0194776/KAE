using KJFramework.Exception;

namespace KJFramework.Net.Cloud.Exceptions
{
    /// <summary>
    ///   服务域取消绑定失败异常。
    /// </summary>
    public class ServerAreaUnBindFailedException : LightException
    {
        public ServerAreaUnBindFailedException(string message) : base(message)
        {
            
        }
    }
}