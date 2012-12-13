using KJFramework.Helpers;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace KJFramework.Reflection
{
    /// <summary>
    ///     反射器，提供了相关的基本操作。
    /// </summary>
    public static class Reflector
    {
        #region 构造函数

        /// <summary>
        ///     反射器，提供了相关的基本操作。
        /// </summary>
        static Reflector()
        {
            LoadOpCodes();
        }

        #endregion

        #region Members

        public static Module[] Modules;
        public static OpCode[] MultiByteOpCodes;
        public static OpCode[] SingleByteOpCodes;
        public static Dictionary<int, object> Cache = new Dictionary<int, object>();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(Reflector));

        #endregion

        #region Functions

        /// <summary>
        ///     根据一个指定的方法信息，动态生成对于指定类型的该方法签名
        /// </summary>
        /// <param name="typeBuilder">类型生成器</param>
        /// <param name="methodInfo">参照的方法模板</param>
        /// <param name="methodAttributes">方法属性</param>
        /// <returns>返回生成后的方法生成器</returns>
        public static MethodBuilder GeneratMethodSignature(TypeBuilder typeBuilder, MethodInfo methodInfo, MethodAttributes methodAttributes)
        {
            if (methodInfo == null || typeBuilder == null)
            {
                return null;
            }
            try
            {
                ParameterInfo[] parameterInfos = TypeHelper.GetParameteres(methodInfo);
                return parameterInfos == null || parameterInfos.Length == 0 ?
                    typeBuilder.DefineMethod(methodInfo.Name, methodAttributes, methodInfo.ReturnType, null) :
                    typeBuilder.DefineMethod(methodInfo.Name, methodAttributes, methodInfo.ReturnType, parameterInfos.Select(parameter => parameter.ParameterType).ToArray());
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                return null;
            }
        }

        /// <summary>
        ///     根据一个程序集名称动态创建一个程序集生成器
        ///         *　该方法生成出来的程序集生成器的访问权限为：Run
        /// </summary>
        /// <param name="name">程序集名称</param>
        /// <returns>返回程序集生成器</returns>
        public static AssemblyBuilder Create(String name)
        {
            return AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(name), AssemblyBuilderAccess.Run);
        }

        /// <summary>
        ///     根据一个程序集名称动态创建一个程序集生成器
        /// </summary>
        /// <param name="name">程序集名称</param>
        /// <param name="assemblyBuilderAccess">访问权限</param>
        /// <returns>返回程序集生成器</returns>
        public static AssemblyBuilder Create(String name, AssemblyBuilderAccess assemblyBuilderAccess)
        {
            return Create(AppDomain.CurrentDomain, name, assemblyBuilderAccess);
        }

        /// <summary>
        ///     根据一个程序集名称动态在指定的应用程序域中创建一个程序集生成器
        /// </summary>
        /// <param name="demain">应用程序域</param>
        /// <param name="name">程序集名称</param>
        /// <param name="assemblyBuilderAccess">访问权限</param>
        /// <returns>返回程序集生成器</returns>
        public static AssemblyBuilder Create(AppDomain demain, String name, AssemblyBuilderAccess assemblyBuilderAccess)
        {
            if (demain == null)
            {
                return null;
            }
            return demain.DefineDynamicAssembly(new AssemblyName(name), assemblyBuilderAccess);
        }

        public static void LoadOpCodes()
        {
            SingleByteOpCodes = new OpCode[0x100];
            MultiByteOpCodes = new OpCode[0x100];
            FieldInfo[] infoArray1 = typeof(OpCodes).GetFields();
            for (int num1 = 0; num1 < infoArray1.Length; num1++)
            {
                FieldInfo info1 = infoArray1[num1];
                if (info1.FieldType == typeof(OpCode))
                {
                    OpCode code1 = (OpCode)info1.GetValue(null);
                    ushort num2 = (ushort)code1.Value;
                    if (num2 < 0x100)
                    {
                        SingleByteOpCodes[(int)num2] = code1;
                    }
                    else
                    {
                        if ((num2 & 0xff00) != 0xfe00)
                        {
                            throw new System.Exception("Invalid OpCode.");
                        }
                        MultiByteOpCodes[num2 & 0xff] = code1;
                    }
                }
            }
        }


        /// <summary>
        /// Retrieve the friendly name of a type
        /// </summary>
        /// <param name="typeName">
        /// The complete name to the type
        /// </param>
        /// <returns>
        /// The simplified name of the type (i.e. "int" instead f System.Int32)
        /// </returns>
        public static string ProcessSpecialTypes(string typeName)
        {
            string result = typeName;
            switch (typeName)
            {
                case "System.string":
                case "System.String":
                case "String":
                    result = "string"; break;
                case "System.Int32":
                case "Int":
                case "Int32":
                    result = "int"; break;
            }
            return result;
        }

        //public static string SpaceGenerator(int count)
        //{
        //    string result = "";
        //    for (int i = 0; i < count; i++) result += " ";
        //    return result;
        //}

        //public static string AddBeginSpaces(string source, int count)
        //{
        //    string[] elems = source.Split('\n');
        //    string result = "";
        //    for (int i = 0; i < elems.Length; i++)
        //    {
        //        result += SpaceGenerator(count) + elems[i] + "\n";
        //    }
        //    return result;
        //}

        #endregion
    }
}