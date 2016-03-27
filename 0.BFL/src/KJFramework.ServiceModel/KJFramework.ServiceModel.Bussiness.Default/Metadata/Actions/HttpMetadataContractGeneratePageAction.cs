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
    ///     ����HTTPЭ��ķ�����ԼԪ��������ҳ�涯�����ṩ����ص���ز�����
    /// </summary>
    internal class HttpMetadataContractGeneratePageAction : HttpMetadataPageAction
    {
        #region Constructor

        /// <summary>
        ///     ����HTTPЭ��ķ�����ԼԪ��������ҳ�涯�����ṩ����ص���ز�����
        /// </summary>
        /// <param name="contractDescription">��������</param>
        public HttpMetadataContractGeneratePageAction(IContractDescription contractDescription)
            : base(contractDescription)
        {
        }

        #endregion

        #region Overrides of HttpMetadataPageAction

        /// <summary>
        ///     ִ�ж���
        /// </summary>
        /// <param name="httpListenerRequest">HTTP��������</param>
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
                            //�������.NET FRAMEWORK���������ͣ����������ɻ���
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
        ///     ���ɵ�����������������Լ
        /// </summary>
        /// <param name="method">��������</param>
        /// <param name="argument">��������</param>
        /// <param name="parameterType">Ҫ���ɵ�����</param>
        /// <returns>�������ɺ�Ĳ�����Լ</returns>
        private String GenerateCustomerParamter(IDescriptionMethod method, IDescriptionArgument argument, Type parameterType)
        {
            StringBuilder stringBuilder = new StringBuilder();
            SerializableAttribute serializableAttribute =  AttributeHelper.GetCustomerAttribute<SerializableAttribute>(parameterType);
            //�������л���
            if (serializableAttribute == null)
            {
                stringBuilder.AppendLine(String.Format(@"<argument type=""{0}"" name=""{1}"" id=""{2}"" cannull=""{3}"" token=""{4}"" flag=""{5}"">", parameterType.FullName, argument.Name, argument.Id, argument.CanNull, method.MethodToken, 1));
                foreach (PropertyInfo propertyInfo in parameterType.GetProperties())
                {
                    IntellectPropertyAttribute intellectPropertyAttribute;
                    //ӵ���������Ա�ǩ
                    if ((intellectPropertyAttribute = AttributeHelper.GetCustomerAttribute<IntellectPropertyAttribute>(propertyInfo)) != null)
                    {
                        //��.NET FRAMEWORK����������
                        if (MetadataExchangeNode.IsFrameworkType((propertyInfo.PropertyType.IsArray ? propertyInfo.PropertyType.GetElementType() : propertyInfo.PropertyType)))
                        {
                            stringBuilder.AppendLine(String.Format(@"<argument type=""{0}"" name=""{1}"" id=""{2}"" cannull=""{3}"" token=""{4}"" flag=""{5}""/>", propertyInfo.PropertyType.FullName, propertyInfo.Name, intellectPropertyAttribute.Id, !intellectPropertyAttribute.IsRequire, method.MethodToken, 0));
                            continue;
                        }
                        //����.NET FRANEWORK���������ͣ���ʼ���е���
                        stringBuilder.AppendLine(GenerateCustomerParamter(method, new DescriptionArgument(intellectPropertyAttribute.Id, !intellectPropertyAttribute.IsRequire, propertyInfo), (propertyInfo.PropertyType.IsArray ? propertyInfo.PropertyType.GetElementType() : propertyInfo.PropertyType)));
                    }
                }
                stringBuilder.AppendLine("</argument>");
            }
            //�����л���
            else
            {
                stringBuilder.AppendLine(String.Format(@"<argument type=""{0}"" name=""{1}"" id=""{2}"" cannull=""{3}"" token=""{4}"" flag=""{5}"" serializable=""{6}"">", argument.FullName, argument.Name, argument.Id, argument.CanNull, method.MethodToken, 1, true));
                #region ��ѯ Property

                foreach (PropertyInfo propertyInfo in parameterType.GetProperties())
                {
                    //��.NET FRAMEWORK����������
                    if (MetadataExchangeNode.IsFrameworkType((propertyInfo.PropertyType.IsArray ? propertyInfo.PropertyType.GetElementType() : propertyInfo.PropertyType)))
                    {
                        stringBuilder.AppendLine(String.Format(@"<argument type=""{0}"" name=""{1}"" id=""{2}"" cannull=""{3}"" token=""{4}"" flag=""{5}"" isproperty=""True""/>", propertyInfo.PropertyType.FullName, propertyInfo.Name, -1, true, method.MethodToken, 0));
                        continue;
                    }
                    //����.NET FRANEWORK���������ͣ���ʼ���е���
                    stringBuilder.AppendLine(GenerateCustomerParamter(method, new DescriptionArgument(-1, true, propertyInfo), (propertyInfo.PropertyType.IsArray
                                     ? propertyInfo.PropertyType.GetElementType()
                                     : propertyInfo.PropertyType)));
                }

                #endregion

                #region ��ѯ Field

                foreach (FieldInfo fieldInfo in parameterType.GetFields())
                {
                    //��.NET FRAMEWORK����������
                    if (MetadataExchangeNode.IsFrameworkType((fieldInfo.FieldType.IsArray ? fieldInfo.FieldType.GetElementType() : fieldInfo.FieldType)))
                    {
                        stringBuilder.AppendLine(String.Format(@"<argument type=""{0}"" name=""{1}"" id=""{2}"" cannull=""{3}"" token=""{4}"" flag=""{5}"" isfield=""True""/>", fieldInfo.FieldType.FullName, fieldInfo.Name, -1, true, method.MethodToken, 0));
                        continue;
                    }
                    //����.NET FRANEWORK���������ͣ���ʼ���е���
                    stringBuilder.AppendLine(GenerateCustomerParamter(method, new DescriptionArgument(-1, true, fieldInfo), (fieldInfo.FieldType.IsArray ? fieldInfo.FieldType.GetElementType() : fieldInfo.FieldType)));
                }

                #endregion
                stringBuilder.AppendLine("</argument>");
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        ///     ����.NET����в�����������Լ
        /// </summary>
        /// <param name="method">��������</param>
        /// <param name="argument">��������</param>
        /// <param name="parameterType">Ҫ���ɵ�����</param>
        /// <returns>�������ɺ�Ĳ�����Լ</returns>
        private String GenerateFrameworkParamter(IDescriptionMethod method, IDescriptionArgument argument, Type parameterType)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(String.Format(@"<argument type=""{0}"" name=""{1}"" id=""{2}"" cannull=""{3}"" token=""{4}"" flag=""{5}""/>", argument.FullName, argument.Name, argument.Id, argument.CanNull, method.MethodToken, 0));
            return stringBuilder.ToString();
        }

        #endregion
    }
}