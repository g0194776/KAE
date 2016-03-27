using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KJFramework.Helpers;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Exceptions;

namespace KJFramework.Messages.Analysers
{
    /// <summary>
    ///     可转换为元数据的智能类型分析器，提供了相关的基本操作。
    /// </summary>
    internal class ToBytesIntellectTypeAnalyser : IntellectTypeAnalyser<ToBytesAnalyseResult[], IIntellectObject>
    {
        #region Overrides of IntellectTypeAnalyser<GetObjectAnalyseResult>

        /// <summary>
        ///     分析一个类型中的所有智能属性
        /// </summary>
        /// <param name="obj">要分析的类型</param>
        /// <returns>返回分析的结果</returns>
        public override ToBytesAnalyseResult[] Analyse(IIntellectObject obj)
        {
            if (obj == null) return null;
            Type t = obj.GetType();
            ToBytesAnalyseResult[] result = GetObject(t.FullName);
            if (result != null) return result;
            #region Analyse process.

            IList<ToBytesAnalyseResult> temp = new List<ToBytesAnalyseResult>();
            PropertyInfo[] propertyInfos = t.GetProperties();
            IntellectPropertyAttribute attribute;
            bool nullable;
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                attribute = AttributeHelper.GetCustomerAttribute<IntellectPropertyAttribute>(propertyInfo);
                if (attribute == null) continue;
                nullable = Nullable.GetUnderlyingType(propertyInfo.PropertyType) != null;
                if (attribute.AllowDefaultNull && (!propertyInfo.PropertyType.IsValueType || nullable))
                    throw new DefineNoMeaningException(string.Format(ExceptionMessage.EX_NO_MEANING_VALUE, attribute.Id, propertyInfo.Name, propertyInfo.PropertyType));
                temp.Add(new ToBytesAnalyseResult
                    {
                        VTStruct = GetVT(propertyInfo.PropertyType),
                        Property = propertyInfo,
                        Attribute = attribute,
                        TargetType = t,
                        Nullable = nullable
                    }.Initialize());
            }
            if (temp.Count == 0) return null;
            result = temp.OrderBy(p => p.Attribute.Id).ToArray();
            RegistAnalyseResult(t.FullName, result);
            return result;

            #endregion
        }

        #endregion
    }
}