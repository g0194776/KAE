using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using KJFramework.ServiceModel.Bussiness.Default.Centers;
using KJFramework.ServiceModel.Bussiness.Default.Descriptions;
using KJFramework.ServiceModel.Core.Metadata;

namespace KJFramework.ServiceModel.Bussiness.Default.Metadata.Actions
{
    /// <summary>
    ///     基于HTTP协议的服务契约元数据方法页面动作，提供了相关的相关操作。
    /// </summary>
    internal class HttpMetadataContractOperationPageAction : HttpMetadataPageAction
    {
        #region Constructor

        /// <summary>
        ///     基于HTTP协议的服务契约元数据根页面动作抽象父类，提供了相关的相关操作。
        /// </summary>
        /// <param name="contractDescription">服务描述</param>
        /// <param name="methodToken">方法令牌</param>
        public HttpMetadataContractOperationPageAction(IContractDescription contractDescription, int methodToken)
            : base(contractDescription)
        {
            IDescriptionMethod method = _contractDescription.GetMethods().Where(m => m.MethodToken == methodToken).First();
            //check ret type.
            bool retCus = method.ReturnType != null && !MetadataTypeGenerator.IsDefaultType(method.ReturnType);
            IMetadataTypeGenerator generator;
            if(retCus)
            {
                generator = new MetadataTypeGenerator();
                Dictionary<string, string> metadatas = generator.Generate(method.ReturnType);
                if (metadatas != null && metadatas.Count > 0)
                {
                    //store it.
                    foreach (KeyValuePair<string, string> pair in metadatas)
                    {
                        ServiceCenter.MetadataNode.Regist(pair.Key, pair.Value);
                        ServiceCenter.MetadataNode.Regist("/" + _contractDescription.Infomation.ContractName + "/Metadata/ArgumentDescription.aspx?Id=" + pair.Key, new HttpMetadataArgumentDescriptionAction(_contractDescription, pair.Key, pair.Value));
                    }
                }
            }
            //expose argument(s).
            IDescriptionArgument[] arguments = method.GetArguments();
            if (arguments != null)
            {
                generator = new MetadataTypeGenerator();
                foreach (IDescriptionArgument descriptionArgument in arguments)
                {
                    if (MetadataTypeGenerator.IsDefaultType(descriptionArgument.ParameterType)) continue;
                    Dictionary<string, string> argMetadata = generator.Generate(descriptionArgument.ParameterType);
                    if (argMetadata == null || argMetadata.Count == 0) continue;
                    //store it.
                    foreach (KeyValuePair<string, string> pair in argMetadata)
                    {
                        ServiceCenter.MetadataNode.Regist(pair.Key, pair.Value);
                        ServiceCenter.MetadataNode.Regist("/" + _contractDescription.Infomation.ContractName + "/Metadata/ArgumentDescription.aspx?Id=" + pair.Key, new HttpMetadataArgumentDescriptionAction(_contractDescription, pair.Key, pair.Value));
                    }
                }
            }
        }

        #endregion

        #region Overrides of HttpMetadataPageAction

        /// <summary>
        ///     执行动作
        /// </summary>
        /// <param name="httpListenerRequest">HTTP输入请求</param>
        public override string Execute(HttpListenerRequest httpListenerRequest)
        {
            String name = httpListenerRequest.QueryString["Name"];
            String token = httpListenerRequest.QueryString["Token"];
            //非法请求
            if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(token)) return null;
            if (_result == null)
            {
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("KJFramework.ServiceModel.Bussiness.Default.Resources.OperationPage.htm");
                IDescriptionMethod method = _contractDescription.GetMethods().Where(m => m.MethodToken == int.Parse(token)).First();
                using (StreamReader reader = new StreamReader(stream))
                {
                    String content = reader.ReadToEnd().Replace("{ContractName}", _contractDescription.Infomation.ContractName);
                    content = content.Replace("{HtmlTitle}", String.Format("{0} Operation Page", method.Name));
                    content = content.Replace("{OperationName}", name);
                    content = content.Replace("{MethodTemplate}", (method.HasReturnValue ? method.ReturnType.FullName.Substring(method.ReturnType.FullName.LastIndexOf('.') + 1) + " " : "void ") + name + GenArguments(method));
                    content = content.Replace("{ArgumentTemplate}", GetArgumentDescription(method));
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
                return "()";
            }
            for (int i = 0; i < arguments.Length; i++)
            {
                if (i + 1 == arguments.Length)
                {
                    content += String.Format("{0} {1}", arguments[i].FullName.Substring(arguments[i].FullName.LastIndexOf('.') + 1), arguments[i].Name);
                    continue;
                }
                content += String.Format("{0} {1}, ", arguments[i].FullName.Substring(arguments[i].FullName.LastIndexOf('.') + 1), arguments[i].Name);
            }
            content += ")";
            return content;
        }

        /// <summary>
        ///     获取参数描述
        /// </summary>
        /// <returns>返回参数描述</returns>
        public String GetArgumentDescription(IDescriptionMethod method)
        {
            IDescriptionArgument[] arguments = method.GetArguments();
            if (arguments == null || arguments.Length == 0)
            {
                return "None.";
            }
            StringBuilder sb = new StringBuilder();
            foreach (IDescriptionArgument descriptionArgument in arguments)
            {
                sb.AppendLine(descriptionArgument.Name + " : ");
                sb.AppendLine("    Type : " + descriptionArgument.FullName);
                sb.AppendLine("    Id : " + descriptionArgument.Id);
                sb.AppendLine("    Can Null : " + descriptionArgument.CanNull);
            }
            return sb.ToString();
        }

        #endregion
    }
}