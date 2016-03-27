using KJFramework.Exception;

namespace KJFramework.Net.Cloud.Exceptions
{
    /// <summary>
    ///   ����������ʧ���쳣��
    /// </summary>
    public class CreateServerAreaFaultException : LightException
    {
        /// <summary>
        ///   ����������ʧ���쳣��
        /// </summary>
        public CreateServerAreaFaultException(string message)
            : base(message)
        {
            
        }
    }
}