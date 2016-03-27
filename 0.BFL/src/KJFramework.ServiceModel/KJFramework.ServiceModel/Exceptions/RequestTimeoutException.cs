using KJFramework.Exception;

namespace KJFramework.ServiceModel.Exceptions
{
    /// <summary>
    ///     请求超时异常
    /// </summary>
    public class RequestTimeoutException : LightException
    {
        #region 构造函数

        /// <summary>
        ///     请求超时异常
        /// </summary>
        public RequestTimeoutException() : base("请求超时，未在只能的时间范围内接收到来自服务器的反馈，请重试重新请求该服务契约操作。")
        {
            
        }

        #endregion
    }
}