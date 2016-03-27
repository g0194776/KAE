using System;
using System.Net;
using System.Reflection;
using System.Text;
using KJFramework.Helpers;
using KJFramework.Messages.Attributes;
using KJFramework.ServiceModel.Bussiness.Default.Descriptions;

namespace KJFramework.ServiceModel.Bussiness.Default.Metadata.Actions
{
    /// <summary>
    ///     基于HTTP协议的服务契约元数据生成页面动作，提供了相关的相关操作。
    /// </summary>
    internal class HttpMetadataContractGeneratePageAction : HttpMetadataPageAction
    {
        #region Constructor

        /// <summary>
        ///     基于HTTP协议的服务契约元数据生成页面动作，提供了相关的相关操作。
        /// </summary>
        /// <param name="contractDescription">服务描述</param>
        public HttpMetadataContractGeneratePageAction(IContractDescription contractDescription)
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
                stringBuilder.AppendLine("<datas>");
                IDescriptionMethod[] methods = _contractDescription.GetMethods();
                if (methods != null)
                {
                    foreach (IDescriptionMethod descriptionMethod in methods)
                    {
                        foreach (IDescriptionArgument argument in descriptionMethod.GetArguments())
                        {
                            //如果不是.NET FRAMEWORK的内置类型，则启动生成机制
                            stringBuilder.AppendLine(!MetadataExchangeNode.IsFrameworkType((argument.ParameterType.IsArray ? argument.ParameterType.GetElementType() : argument.ParameterType))
                                                         ? GenerateCustomerParamter(descriptionMethod, argument, (argument.ParameterType.IsArray ? argument.ParameterType.GetElementType() : argument.ParameterType))
                                                         : GenerateFrameworkParamter(descriptionMethod, argument, argument.ParameterType));
                        }
                    }
                }
                stringBuilder.AppendLine("</datas>");
                _result = stringBuilder.ToString();
            }
            return _result;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     生成第三方参数的数据契约
        /// </summary>
        /// <param name="method">描述方法</param>
        /// <param name="argument">描述参数</param>
        /// <param name="parameterType">要生成的类型</param>
        /// <returns>返回生成后的参数契约</returns>
        private String GenerateCustomerParamter(IDescriptionMethod method, IDescriptionArgument argument, Type parameterType)
        {
            StringBuilder stringBuilder = new StringBuilder();
            SerializableAttribute serializableAttribute =  AttributeHelper.GetCustomerAttribute<SerializableAttribute>(parameterType);
            //不可序列化的
            if (serializableAttribute == null)
            {
                stringBuilder.AppendLine(String.Format(@"<argument type=""{0}"" name=""{1}"" id=""{2}"" cannull=""{3}"" token=""{4}"" flag=""{5}"">", parameterType.FullName, argument.Name, argument.Id, argument.CanNull, method.MethodToken, 1));
                foreach (PropertyInfo propertyInfo in parameterType.GetProperties())
                {
                    IntellectPropertyAttribute intellectPropertyAttribute;
                    //拥有智能属性标签
                    if ((intellectPropertyAttribute = AttributeHelper.GetCustomerAttribute<IntellectPropertyAttribute>(propertyInfo)) != null)
                    {
                        //是.NET FRAMEWORK的内置类型
                        if (MetadataExchangeNode.IsFrameworkType((propertyInfo.PropertyType.IsArray ? propertyInfo.PropertyType.GetElementType() : propertyInfo.PropertyType)))
                        {
                            stringBuilder.AppendLine(String.Format(@"<argument type=""{0}"" name=""{1}"" id=""{2}"" cannull=""{3}"" token=""{4}"" flag=""{5}""/>", propertyInfo.PropertyType.FullName, propertyInfo.Name, intellectPropertyAttribute.Id, !intellectPropertyAttribute.IsRequire, method.MethodToken, 0));
                            continue;
                        }
                        //不是.NET FRANEWORK的内置类型，开始进行迭代
                        stringBuilder.AppendLine(GenerateCustomerParamter(method, new DescriptionArgument(intellectPropertyAttribute.Id, !intellectPropertyAttribute.IsRequire, propertyInfo), (propertyInfo.PropertyType.IsArray ? propertyInfo.PropertyType.GetElementType() : propertyInfo.PropertyType)));
                    }
                }
                stringBuilder.AppendLine("</argument>");
            }
            //可序列化的
            else
            {
                stringBuilder.AppendLine(String.Format(@"<argument type=""{0}"" name=""{1}"" id=""{2}"" cannull=""{3}"" token=""{4}"" flag=""{5}"" serializable=""{6}"">", argument.FullName, argument.Name, argument.Id, argument.CanNull, method.MethodToken, 1, true));
                #region 轮询 Property

                foreach (PropertyInfo propertyInfo in parameterType.GetProperties())
                {
                    //是.NET FRAMEWORK的内置类型
                    if (MetadataExchangeNode.IsFrameworkType((propertyInfo.PropertyType.IsArray ? propertyInfo.PropertyType.GetElementType() : propertyInfo.PropertyType)))
                    {
                        stringBuilder.AppendLine(String.Format(@"<argument type=""{0}"" name=""{1}"" id=""{2}"" cannull=""{3}"" token=""{4}"" flag=""{5}"" isproperty=""True""/>", propertyInfo.PropertyType.FullName, propertyInfo.Name, -1, true, method.MethodToken, 0));
                        continue;
                    }
                    //不是.NET FRANEWORK的内置类型，开始进行迭代
                    stringBuilder.AppendLine(GenerateCustomerParamter(method, new DescriptionArgument(-1, true, propertyInfo), (propertyInfo.PropertyType.IsArray
                                     ? propertyInfo.PropertyType.GetElementType()
                                     : propertyInfo.PropertyType)));
                }

                #endregion

                #region 轮询 Field

                foreach (FieldInfo fieldInfo in parameterType.GetFields())
                {
                    //是.NET FRAMEWORK的内置类型
                    if (MetadataExchangeNode.IsFrameworkType((fieldInfo.FieldType.IsArray ? fieldInfo.FieldType.GetElementType() : fieldInfo.FieldType)))
                    {
                        stringBuilder.AppendLine(String.Format(@"<argument type=""{0}"" name=""{1}"" id=""{2}"" cannull=""{3}"" token=""{4}"" flag=""{5}"" isfield=""True""/>", fieldInfo.FieldType.FullName, fieldInfo.Name, -1, true, method.MethodToken, 0));
                        continue;
                    }
                    //不是.NET FRANEWORK的内置类型，开始进行迭代
                    stringBuilder.AppendLine(GenerateCustomerParamter(method, new DescriptionArgument(-1, true, fieldInfo), (fieldInfo.FieldType.IsArray ? fieldInfo.FieldType.GetElementType() : fieldInfo.FieldType)));
                }

                #endregion
                stringBuilder.AppendLine("</argument>");
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        ///     生成.NET类库中参数的数据契约
        /// </summary>
        /// <param name="method">描述方法</param>
        /// <param name="argument">描述参数</param>
        /// <param name="parameterType">要生成的类型</param>
        /// <returns>返回生成后的参数契约</returns>
        private String GenerateFrameworkParamter(IDescriptionMethod method, IDescriptionArgument argument, Type parameterType)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(String.Format(@"<argument type=""{0}"" name=""{1}"" id=""{2}"" cannull=""{3}"" token=""{4}"" flag=""{5}""/>", argument.FullName, argument.Name, argument.Id, argument.CanNull, method.MethodToken, 0));
            return stringBuilder.ToString();
        }

        #endregion
    }
}