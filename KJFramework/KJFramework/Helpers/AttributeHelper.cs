using System;
using System.Reflection;

namespace KJFramework.Helpers
{
    /// <summary>
    ///     ������԰��������ṩ����صĻ���������
    /// </summary>
    public class AttributeHelper
    {
        /// <summary>
        ///     ��ȡһ�����͵��е�ָ���������
        /// </summary>
        /// <typeparam name="T">�����������</typeparam>
        /// <param name="type">��ȡ�Ķ���</param>
        /// <returns>����ָ���ı������</returns>
        public static T GetCustomerAttribute<T>(Type type) where T : System.Attribute
        {
            if (type == null)
            {
                return null;
            }
            Object[] objects = type.GetCustomAttributes(typeof (T), true);
            return objects != null && objects.Length > 0 ? (T) objects[0] : default(T);
        }

        /// <summary>
        ///     ��ȡһ�����͵��е�ָ���������
        /// </summary>
        /// <typeparam name="T">�����������</typeparam>
        /// <param name="type">��ȡ�Ķ���</param>
        /// <returns>����ָ���ı������</returns>
        public static T[] GetCustomerAttributes<T>(Type type) where T : System.Attribute
        {
            if (type == null)
            {
                return null;
            }
            Object[] objects = type.GetCustomAttributes(typeof(T), true);
            return objects == null ? null : (T[]) objects;
        }

        /// <summary>
        ///     ��ȡһ���ֶε��е�ָ���������
        /// </summary>
        /// <typeparam name="T">�����������</typeparam>
        /// <param name="field">��ȡ�Ķ���</param>
        /// <returns>����ָ���ı������</returns>
        public static T GetCustomerAttribute<T>(FieldInfo field) where T : System.Attribute
        {
            if (field == null)
            {
                return null;
            }
            Object[] objects = field.GetCustomAttributes(typeof(T), true);
            return objects != null && objects.Length > 0 ? (T)objects[0] : default(T);
        }

        /// <summary>
        ///     ��ȡһ���ֶε��е�ָ���������
        /// </summary>
        /// <typeparam name="T">�����������</typeparam>
        /// <param name="property">��ȡ�Ķ���</param>
        /// <returns>����ָ���ı������</returns>
        public static T GetCustomerAttribute<T>(PropertyInfo property) where T : System.Attribute
        {
            if (property == null)
            {
                return null;
            }
            Object[] objects = property.GetCustomAttributes(typeof(T), true);
            return objects != null && objects.Length > 0 ? (T)objects[0] : default(T);
        }

        /// <summary>
        ///     ��ȡһ���������е�ָ���������
        /// </summary>
        /// <typeparam name="T">�����������</typeparam>
        /// <param name="methodInfo">��ȡ�ķ���</param>
        /// <returns>����ָ���ı������</returns>
        public static T GetCustomerAttribute<T>(MethodInfo methodInfo) where T : System.Attribute
        {
            if (methodInfo == null)
            {
                return null;
            }
            Object[] objects = methodInfo.GetCustomAttributes(typeof(T), true);
            return objects != null && objects.Length > 0 ? (T)objects[0] : default(T);
        }
    }
}