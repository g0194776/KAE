using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using KJFramework.ServiceModel.Bussiness.Default.Centers;
using KJFramework.ServiceModel.Bussiness.Default.Descriptions;
using KJFramework.ServiceModel.Core.Metadata;

namespace KJFramework.ServiceModel.Bussiness.Default.Metadata.Actions
{
    /// <summary>
    ///     基于HTTP协议的服务契约元数据预览页面动作，提供了相关的相关操作。
    /// </summary>
    internal class HttpMetadataContractPreviewPageAction : HttpMetadataPageAction
    {
        #region Constructor

        /// <summary>
        ///     基于HTTP协议的服务契约元数据预览页面动作，提供了相关的相关操作。
        /// </summary>
        /// <param name="contractDescription">服务描述</param>
        public HttpMetadataContractPreviewPageAction(IContractDescription contractDescription)
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
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(@"<?xml version=""1.0"" encoding=""utf-8""?>");
                stringBuilder.AppendLine("<contract>");
                stringBuilder.AppendLine(
                    String.Format(
                        @"<infomations name=""{0}"" thirdty-name=""{1}"" full-name=""{2}"" description=""{3}"" version=""{4}"" />",
                        _contractDescription.Infomation.Name, _contractDescription.Infomation.ContractName,
                        _contractDescription.Infomation.FullName, _contractDescription.Infomation.Description,
                        _contractDescription.Infomation.Version));
                stringBuilder.AppendLine("<operations>");
                IDescriptionMethod[] methods = _contractDescription.GetMethods();
                if (methods != null)
                {
                    foreach (IDescriptionMethod descriptionMethod in methods)
                    {
                        string name = descriptionMethod.Name.Substring(descriptionMethod.Name.LastIndexOf('.') + 1);
                        bool retDefault = MetadataTypeGenerator.IsDefaultType(descriptionMethod.ReturnType);
                        stringBuilder.Append(
                            string.Format(
                                @"<operation name=""{0}"" fullsign=""{1}"" token=""{2}"" isasync=""{3}"" isoneway=""{4}"" ret=""{5}"" ",
                                name,
                                ((descriptionMethod.HasReturnValue
                                      ? descriptionMethod.ReturnType.FullName.Substring(
                                          descriptionMethod.ReturnType.FullName.LastIndexOf(
                                              '.') + 1) + " "
                                      : "void ") + name + GenArguments(descriptionMethod)),
                                descriptionMethod.MethodToken,
                                descriptionMethod.Attribute.IsAsync,
                                descriptionMethod.Attribute.IsOneWay,
                                (descriptionMethod.ReturnType == null
                                     ? "void"
                                     : (!retDefault
                                           ? descriptionMethod.ReturnType.Name
                                           : descriptionMethod.ReturnType.FullName))));
                        if (retDefault || descriptionMethod.ReturnType == null) stringBuilder.AppendLine(@"/>");
                        else
                        {
                            IMetadataTypeGenerator generator = new MetadataTypeGenerator();
                            Dictionary<string, string> metadatas = generator.Generate(descriptionMethod.ReturnType);
                            if (metadatas == null || metadatas.Count == 0)
                            {
                                stringBuilder.AppendLine(@"/>");
                                continue;
                            }
                            stringBuilder.AppendLine(string.Format(@"referenceId=""{0}""/>",
                                                                   descriptionMethod.ReturnType.IsArray
                                                                       ? descriptionMethod.ReturnType.GetElementType().FullName.GetHashCode()
                                                                       : descriptionMethod.ReturnType.FullName.GetHashCode()));
                        }
                    }
                }
                stringBuilder.AppendLine("</operations>");
                stringBuilder.AppendLine("</contract>");
                _result = stringBuilder.ToString();
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