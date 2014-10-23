using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;

using KJFramework.ApplicationEngine.KIS.Models;

namespace KJFramework.ApplicationEngine.KIS.Handlers
{
    /// <summary>
    ///     出口对象的检查
    /// </summary>
    internal class CustomerMessageHandler : MessageProcessingHandler
    {
        #region Overrides of MessageProcessingHandler

        protected override HttpRequestMessage ProcessRequest(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return request;
        }

        /// <summary>
        ///     输出报文信息
        /// </summary>
        /// <param name="response"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override HttpResponseMessage ProcessResponse(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            ObjectContent<object> objectContent = response.Content as ObjectContent<object>;
            if (objectContent != null && objectContent.Value is ErrorModel) //如果程序出错，返回状态码202
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Headers.Add("RequestUrl", HttpContext.Current.Request.Url.AbsoluteUri);
            }
            if (objectContent != null && objectContent.Value == null)
                response.Content = new StringContent("");
            return response;
        }

        #endregion
    }
}