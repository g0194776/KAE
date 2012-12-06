using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KJFramework.Basic.Enum;
using KJFramework.Configurations.Objects;
using KJFramework.Logger;

namespace KJFramework.Helpers
{
    /// <summary>
    ///     类型帮助器，提供了相关的基本操作
    /// </summary>
    public class TypeHelper
    {
        /// <summary>
        ///     获取一个类型中所有带有指定属性标记的字段集合
        /// </summary>
        /// <typeparam name="T">标记属性类型</typeparam>
        /// <param name="type">类型</param>
        /// <returns>返回相应的字段集合</returns>
        public static List<FieldWithAttribute<T>> GetFields<T>(Type type) where T : System.Attribute
        {
            if (type == null)
            {
                return null;
            }
            var result = type.GetFields().Where(field=> AttributeHelper.GetCustomerAttribute<T>(field) != null);
            return result.Count() == 0 ? null : result.Select(field => new FieldWithAttribute<T> { FieldInfo = field, Attribute = AttributeHelper.GetCustomerAttribute<T>(field) }).ToList();
        }

        /// <summary>
        ///     获取一个类型中所有字段的相关信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>返回所有的字段集合</returns>
        public static List<FieldWithName> GetFields(Type type)
        {
            if (type == null)
            {
                return null;
            }
            return type.GetFields().Select(field => new FieldWithName {FieldInfo = field, Name = field.Name}).ToList();
        }

        /// <summary>
        ///     获取一个类型中所有字段的相关信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>返回所有的字段集合</returns>
        public static List<PropertyWithName> GetProperties(Type type)
        {
            if (type == null)
            {
                return null;
            }
            return type.GetProperties().Select(property => new PropertyWithName { PropertyInfo = property, Name = property.Name }).ToList();
        }

        /// <summary>
        ///     将一个指定的对象赋值到一个字段类型中
        /// </summary>
        /// <param name="org">被复制的原有对象</param>
        /// <param name="fieldInfo">字段类型</param>
        /// <param name="value">赋值对象</param>
        public static FieldInfo SetValue(Object org, FieldInfo fieldInfo, Object value)
        {
            if (fieldInfo == null)
            {
                return null;
            }
            if (fieldInfo.FieldType.Name.ToLower() == "int32")
            {
                fieldInfo.SetValue(org, int.Parse(value.ToString()));
            }
            else if (fieldInfo.FieldType.Name.ToLower() == "boolean")
            {
                fieldInfo.SetValue(org, bool.Parse(value.ToString()));
            }
            else
            {
                fieldInfo.SetValue(org, value);
            }
            return fieldInfo;
        }

        /// <summary>
        ///     从一个指定的类型中获取该类型的所有方法信息
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="target">要获取的对象</param>
        /// <returns>返回获取到的方法信息集合</returns>
        public static MethodInfo[] GetMethods<T>(T target)
        {
            if (target.Equals(default(T)))
            {
                return null;
            }
            return target.GetType().GetMethods();
        }

        /// <summary>
        ///     从一个指定的类型中获取该类型的指定方法信息
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="target">要获取的对象</param>
        /// <param name="name">指定方法的名称</param>
        /// <returns>返回获取到的方法信息集合</returns>
        public static MethodInfo GetMethods<T>(T target, String name)
        {
            if (target.Equals(default(T)))
            {
                return null;
            }
            try
            {
                return target.GetType().GetMethod(name);
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex, DebugGrade.Standard, Logs.Name);
                return null;
            }
        }

        /// <summary>
        ///     获取一个方法中的所有参数信息
        /// </summary>
        /// <param name="methodInfo">获取的对象</param>
        /// <returns>返回参数信息集合</returns>
        public static ParameterInfo[] GetParameteres(MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                return null;
            }
            return methodInfo.GetParameters();
        }
    }
}