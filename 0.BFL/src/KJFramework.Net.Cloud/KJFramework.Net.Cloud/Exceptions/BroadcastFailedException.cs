using KJFramework.Exception;

namespace KJFramework.Net.Cloud.Exceptions
{
    /// <summary>
    ///   �㲥ʧ���쳣��
    /// </summary>
    public class BroadcastFailedException : LightException
    {
        public BroadcastFailedException(string message) : base(message)
        {

        }
    }
}