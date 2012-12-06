using System;
using System.Reflection;

namespace KJFramework.Helpers
{
    /// <summary>
    ///     标记属性帮助器，提供了相关的基本操作。
    /// </summary>
    public class AttributeHelper
    {
        /// <summary>
        ///     获取一个类型的中的指定标记属性
        /// </summary>
        /// <typeparam name="T">标记属性类型</typeparam>
        /// <param name="type">获取的对象</param>
        /// <returns>返回指定的标记属性</returns>
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
        ///     获取一个类型的中的指定标记属性
        /// </summary>
        /// <typeparam name="T">标记属性类型</typeparam>
        /// <param name="type">获取的对象</param>
        /// <returns>返回指定的标记属性</returns>
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
        ///     获取一个字段的中的指定标记属性
        /// </summary>
        /// <typeparam name="T">标记属性类型</typeparam>
        /// <param name="field">获取的对象</param>
        /// <returns>返回指定的标记属性</returns>
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
        ///     获取一个字段的中的指定标记属性
        /// </summary>
        /// <typeparam name="T">标记属性类型</typeparam>
        /// <param name="property">获取的对象</param>
        /// <returns>返回指定的标记属性</returns>
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
        ///     获取一个方法的中的指定标记属性
        /// </summary>
        /// <typeparam name="T">标记属性类型</typeparam>
        /// <param name="methodInfo">获取的方法</param>
        /// <returns>返回指定的标记属性</returns>
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