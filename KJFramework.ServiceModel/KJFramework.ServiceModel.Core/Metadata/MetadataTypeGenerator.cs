using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using KJFramework.Helpers;
using KJFramework.Messages.Attributes;

namespace KJFramework.ServiceModel.Core.Metadata
{
    public class MetadataTypeGenerator : IMetadataTypeGenerator
    {
        #region Constructor

        static MetadataTypeGenerator()
        {
            _preTypes = new Dictionary<Type, int>();
            _preTypes.Add(typeof(char), 0);
            _preTypes.Add(typeof(int), 0);
            _preTypes.Add(typeof(long), 0);
            _preTypes.Add(typeof(short), 0);
            _preTypes.Add(typeof(uint), 0);
            _preTypes.Add(typeof(ulong), 0);
            _preTypes.Add(typeof(ushort), 0);
            _preTypes.Add(typeof(bool), 0);
            _preTypes.Add(typeof(float), 0);
            _preTypes.Add(typeof(double), 0);
            _preTypes.Add(typeof(string), 0);
            _preTypes.Add(typeof(byte), 0);
            _preTypes.Add(typeof(DateTime), 0);
            _preTypes.Add(typeof(IntPtr), 0);
            _preTypes.Add(typeof(Guid), 0);
            _preTypes.Add(typeof(Object), 0);
            _preTypes.Add(typeof(void), 0);
        }

        #endregion

        #region Members

        protected static readonly IDictionary<Type, int> _preTypes; 

        #endregion

        #region Implementation of IMetadataTypeGenerator

        /// <summary>
        ///     为一个指定类型生成元数据描述(XML)
        /// </summary>
        /// <param name="type">需要生成元数据的类型</param>
        /// <returns>返回描述信息(XML)</returns>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        public Dictionary<string, string> Generate(Type type)
        {
            if (type.IsArray) type = type.GetElementType();
            Dictionary<string, string> metadata = new Dictionary<string, string>();
            var properties =
                type.GetProperties().Where(
                    property => AttributeHelper.GetCustomerAttribute<IntellectPropertyAttribute>(property) != null);
            if (properties.Count() == 0) return metadata;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(string.Format(@"<metadata name=""{0}"" id=""{1}"">", type.Name, Math.Abs(type.FullName.GetHashCode())));
            foreach (PropertyInfo propertyInfo in properties)
            {
                Type pt = propertyInfo.PropertyType;
                IntellectPropertyAttribute attribute = AttributeHelper.GetCustomerAttribute<IntellectPropertyAttribute>(propertyInfo);
                stringBuilder.Append(string.Format(@"<member type=""{0}"" id=""{1}"" require=""{2}"" name=""{3}"" ", pt.Name, attribute.Id, attribute.IsRequire, propertyInfo.Name));
                if (IsDefaultType(pt))
                {
                    stringBuilder.AppendLine("/>");
                    continue;
                }
                foreach (KeyValuePair<string, string> pair in Generate(pt))
                    metadata.Add(pair.Key, pair.Value);
                stringBuilder.AppendLine(string.Format(@"serializable=""{0}"" reference=""{1}""/>", pt.IsSerializable,
                                                       Math.Abs(pt.IsArray
                                                                    ? pt.GetElementType().FullName.GetHashCode()
                                                                    : pt.FullName.GetHashCode())));
            }
            stringBuilder.AppendLine(@"</metadata>");
            metadata.Add(Math.Abs(type.FullName.GetHashCode()).ToString(), stringBuilder.ToString());
            return metadata;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     判断一个类型是否为系统默认数据类型
        /// </summary>
        /// <param name="type">需要判断的类型</param>
        /// <returns>返回判断后的结果</returns>
        public static bool IsDefaultType(Type type)
        {
            if (type == null) return false;
            int flowValue;
            if (!type.IsArray && _preTypes.TryGetValue(type, out flowValue)) return true;
            if (type.IsArray && _preTypes.TryGetValue(type.GetElementType(), out flowValue)) return true;
            return false;
        }

        /// <summary>
        ///     获取一个类型中的自定义客户数据类型
        /// </summary>
        /// <param name="type">需要检测的类型</param>
        /// <param name="expression">条件过滤表达式</param>
        /// <returns>返回客户端数据类型</returns>
        public static IList<Type> GetCustomerTypes(Type type, Func<PropertyInfo[], IEnumerable<PropertyInfo>> expression)
        {
            if (type == null) throw new ArgumentNullException("type");
            PropertyInfo[] properties = type.GetProperties();
            IList<Type> types = new List<Type>();
            foreach (PropertyInfo propertyInfo in expression(properties))
            {
                int flowValue;
                if(!propertyInfo.PropertyType.IsArray && !_preTypes.TryGetValue(propertyInfo.PropertyType, out flowValue))
                {
                    types.Add(propertyInfo.PropertyType);
                    continue;
                }
                if (propertyInfo.PropertyType.IsArray && !_preTypes.TryGetValue(propertyInfo.PropertyType.GetElementType(), out flowValue))
                {
                    types.Add(propertyInfo.PropertyType.GetElementType());
                    continue;
                }
            }
            return types;
        }

        #endregion
    }
}