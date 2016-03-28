using System;
using System.Collections.Generic;
using System.Linq;
using KJFramework.Helpers;
using KJFramework.Messages.Attributes;

namespace KJFramework.Messages.Analysers
{
    /// <summary>
    ///     ��ת��Ϊ������������ͷ��������ṩ����صĻ���������
    /// </summary>
    internal class GetObjectIntellectTypeAnalyser : IntellectTypeAnalyser<Dictionary<int, GetObjectAnalyseResult>, Type>
    {
        #region Overrides of IntellectTypeAnalyser<GetObjectAnalyseResult>

        /// <summary>
        ///     ����һ�������е�������������
        /// </summary>
        /// <param name="type">Ҫ����������</param>
        /// <returns>���ط����Ľ��</returns>
        public override Dictionary<int, GetObjectAnalyseResult> Analyse(Type type)
        {
            if (type == null) return null;
            Dictionary<int, GetObjectAnalyseResult> result = GetObject(type.FullName);
            if (result != null) return result;
            var targetProperties =
               type.GetProperties().AsParallel().Where(
                   property => AttributeHelper.GetCustomerAttribute<IntellectPropertyAttribute>(property) != null);
            if (!targetProperties.Any()) return null;
            result = targetProperties.Select(property => new GetObjectAnalyseResult
                                                        {
                                                            VTStruct = GetVT(property.PropertyType),
                                                            Property = property,
                                                            TargetType = type,
                                                            Nullable = Nullable.GetUnderlyingType(property.PropertyType) != null,
                                                            Attribute = AttributeHelper.GetCustomerAttribute<IntellectPropertyAttribute>(property)
                                                        }.Initialize()).DefaultIfEmpty().OrderBy(property => property.Attribute.Id).ToDictionary(property => property.Attribute.Id);
            RegistAnalyseResult(type.FullName, result);
            return result;
        }

        #endregion
    }
}