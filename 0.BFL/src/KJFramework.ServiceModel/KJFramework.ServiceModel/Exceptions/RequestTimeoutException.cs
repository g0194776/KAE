using KJFramework.Exception;

namespace KJFramework.ServiceModel.Exceptions
{
    /// <summary>
    ///     ����ʱ�쳣
    /// </summary>
    public class RequestTimeoutException : LightException
    {
        #region ���캯��

        /// <summary>
        ///     ����ʱ�쳣
        /// </summary>
        public RequestTimeoutException() : base("����ʱ��δ��ֻ�ܵ�ʱ�䷶Χ�ڽ��յ����Է������ķ�������������������÷�����Լ������")
        {
            
        }

        #endregion
    }
}