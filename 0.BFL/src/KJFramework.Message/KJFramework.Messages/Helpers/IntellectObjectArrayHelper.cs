using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using KJFramework.Messages.Contracts;

namespace KJFramework.Messages.Helpers
{
    /// <summary>
    ///     智能类型数组帮助器
    /// </summary>
    internal static class IntellectObjectArrayHelper
    {
        #region Members

        private static readonly object _lockObj = new object();
        private static readonly Dictionary<string, object> _methods = new Dictionary<string, object>();

        #endregion

        #region Methods

        /// <summary>
        ///     获取指定类型的功能函数
        /// </summary>
        /// <param name="type">数组类型</param>
        /// <typeparam name="T">数组元素类型</typeparam>
        /// <returns>返回一个功能函数</returns>
        public static Func<int, T[]> GetFunc<T>(Type type)
            where T : IntellectObject
        {
            lock (_lockObj)
            {
                object obj;
                string fullname = type.FullName;
                if (_methods.TryGetValue(fullname, out obj)) return (Func<int, T[]>)obj;
                //create cache method.
                DynamicMethod dynamicMethod = new DynamicMethod(string.Format("CreateArrayInstnaceBy: {0}", fullname),
                                                                MethodAttributes.Public | MethodAttributes.Static,
                                                                CallingConventions.Standard, type,
                                                                new[] { typeof(int) }, typeof(object), true);
                ILGenerator generator = dynamicMethod.GetILGenerator();
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Newarr, type.GetElementType());
                generator.Emit(OpCodes.Ret);
                Func<int, T[]> func = (Func<int, T[]>)dynamicMethod.CreateDelegate(typeof(Func<int, T[]>));
                _methods.Add(fullname, func);
                return func;
            }
        }

        #endregion
    }
}