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
    ///     ���Ͱ��������ṩ����صĻ�������
    /// </summary>
    public class TypeHelper
    {
        /// <summary>
        ///     ��ȡһ�����������д���ָ�����Ա�ǵ��ֶμ���
        /// </summary>
        /// <typeparam name="T">�����������</typeparam>
        /// <param name="type">����</param>
        /// <returns>������Ӧ���ֶμ���</returns>
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
        ///     ��ȡһ�������������ֶε������Ϣ
        /// </summary>
        /// <param name="type">����</param>
        /// <returns>�������е��ֶμ���</returns>
        public static List<FieldWithName> GetFields(Type type)
        {
            if (type == null)
            {
                return null;
            }
            return type.GetFields().Select(field => new FieldWithName {FieldInfo = field, Name = field.Name}).ToList();
        }

        /// <summary>
        ///     ��ȡһ�������������ֶε������Ϣ
        /// </summary>
        /// <param name="type">����</param>
        /// <returns>�������е��ֶμ���</returns>
        public static List<PropertyWithName> GetProperties(Type type)
        {
            if (type == null)
            {
                return null;
            }
            return type.GetProperties().Select(property => new PropertyWithName { PropertyInfo = property, Name = property.Name }).ToList();
        }

        /// <summary>
        ///     ��һ��ָ���Ķ���ֵ��һ���ֶ�������
        /// </summary>
        /// <param name="org">�����Ƶ�ԭ�ж���</param>
        /// <param name="fieldInfo">�ֶ�����</param>
        /// <param name="value">��ֵ����</param>
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
        ///     ��һ��ָ���������л�ȡ�����͵����з�����Ϣ
        /// </summary>
        /// <typeparam name="T">ָ������</typeparam>
        /// <param name="target">Ҫ��ȡ�Ķ���</param>
        /// <returns>���ػ�ȡ���ķ�����Ϣ����</returns>
        public static MethodInfo[] GetMethods<T>(T target)
        {
            if (target.Equals(default(T)))
            {
                return null;
            }
            return target.GetType().GetMethods();
        }

        /// <summary>
        ///     ��һ��ָ���������л�ȡ�����͵�ָ��������Ϣ
        /// </summary>
        /// <typeparam name="T">ָ������</typeparam>
        /// <param name="target">Ҫ��ȡ�Ķ���</param>
        /// <param name="name">ָ������������</param>
        /// <returns>���ػ�ȡ���ķ�����Ϣ����</returns>
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
        ///     ��ȡһ�������е����в�����Ϣ
        /// </summary>
        /// <param name="methodInfo">��ȡ�Ķ���</param>
        /// <returns>���ز�����Ϣ����</returns>
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