using KJFramework.Exception;

namespace KJFramework.Net.Cloud.Exceptions
{
    /// <summary>
    ///   ������ȡ����ʧ���쳣��
    /// </summary>
    public class ServerAreaUnBindFailedException : LightException
    {
        public ServerAreaUnBindFailedException(string message) : base(message)
        {
            
        }
    }
}