using System;
using System.IO;
using System.Net;
using System.Reflection;
using KJFramework.ServiceModel.Bussiness.Default.Descriptions;

namespace KJFramework.ServiceModel.Bussiness.Default.Metadata.Actions
{
    /// <summary>
    ///     基于HTTP协议的服务契约元数据根页面动作，提供了相关的相关操作。
    /// </summary>
    internal class HttpMetadataContractRootPageAction : HttpMetadataPageAction
    {
        #region Constructor

        /// <summary>
        ///     基于HTTP协议的服务契约元数据根页面动作抽象父类，提供了相关的相关操作。
        /// </summary>
        /// <param name="contractDescription">服务描述</param>
        public HttpMetadataContractRootPageAction(IContractDescription contractDescription)
            : base(contractDescription)
        {
        }

        #endregion

        #region Overrides of HttpMetadataPageAction

        /// <summary>
        ///     执行动作
        /// </summary>
        /// <param name="httpListenerRequest">HTTP输入请求</param>
        public override string Execute(HttpListenerRequest httpListenerRequest)
        {
            if (_result == null)
            {
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("KJFramework.ServiceModel.Bussiness.Default.Resources.RootPage.html");
                using (StreamReader reader = new StreamReader(stream))
                {
                    String content = reader.ReadToEnd().Replace("{Title}", _contractDescription.Infomation.ContractName);
                    String methods = "";
                    foreach (IDescriptionMethod descriptionMethod in _contractDescription.GetMethods())
                    {
                        String tempMethodName = descriptionMethod.Name.Substring(descriptionMethod.Name.LastIndexOf('.') + 1);
                        methods += String.Format(@"<li><a href=""{1}"">{0}</a></li><p>",
                                                (descriptionMethod.HasReturnValue ? descriptionMethod.ReturnType.FullName.Substring(descriptionMethod.ReturnType.FullName.LastIndexOf('.') + 1) + " " : "void ") +
                                                 tempMethodName + GenArguments(descriptionMethod),
                                                "/" + _contractDescription.Infomation.ContractName + "/Operations.aspx?Name=" + tempMethodName + "&Token=" + descriptionMethod.MethodToken);
                    }
                    content = content.Replace("{HtmlTitle}", String.Format("{0} Contract Page", _contractDescription.Infomation.ContractName));
                    content = content.Replace("{List}", @"<ul>" + methods + "</ul>");
                    _result = content;
                }
            }
            return _result;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     生成方法参数的文本格式
        /// </summary>
        /// <param name="method">可描述的方法</param>
        /// <returns>返回方法参数文本格式</returns>
        private String GenArguments(IDescriptionMethod method)
        {
            String content = "(";
            IDescriptionArgument[] arguments = method.GetArguments();
            if (arguments == null || arguments.Length == 0)
            {
                return "";
            }
            for (int i = 0; i < arguments.Length; i++)
            {
                if (i + 1 == arguments.Length)
                {
                    content += String.Format("{0} {1}", arguments[i].FullName, arguments[i].Name);
                    continue;
                }
                content += String.Format("{0} {1}, ", arguments[i].FullName, arguments[i].Name);
            }
            content += ")";
            return content;
        }

        #endregion
    }
}