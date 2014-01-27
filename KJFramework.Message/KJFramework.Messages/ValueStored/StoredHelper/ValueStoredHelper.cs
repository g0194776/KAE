using System;
using System.Reflection;
using System.Reflection.Emit;

namespace KJFramework.Messages.ValueStored.StoredHelper
{
    /// <summary>
    ///     用于Value值存储的帮忙类
    /// </summary>
    public static class ValueStoredHelper
    {
        /// <summary>
        ///     返回具体类型的对象存储实例
        /// </summary>
        /// <typeparam name="T">处理的类型</typeparam>
        /// <returns>返回一个动态实例</returns>
        public static PropertyValueStored<T> BuildMethod<T>()
        {
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("Assembly"), AssemblyBuilderAccess.Run);
            ModuleBuilder defineDynamicModule = assemblyBuilder.DefineDynamicModule("Module");
            TypeBuilder typeBuilder = defineDynamicModule.DefineType("KJFramework.DynamicModule.ValueStored", TypeAttributes.Public | TypeAttributes.Class, typeof(PropertyValueStored<T>), new Type[] {});
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("Get", MethodAttributes.Public | MethodAttributes.Virtual);
            GenericTypeParameterBuilder[] gpas = methodBuilder.DefineGenericParameters("K");
            methodBuilder.SetParameters(typeof(T));
            methodBuilder.SetReturnType(gpas[0]);
            methodBuilder.DefineParameter(1, ParameterAttributes.None, "value");
            ILGenerator ilGenerator = methodBuilder.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_1);

            //(T)value
            //ilGenerator.Emit(OpCodes.Castclass, typeof(T));
            ilGenerator.Emit(OpCodes.Ret);

            Type type = typeBuilder.CreateType();
            return (PropertyValueStored<T>)typeBuilder.Assembly.CreateInstance(type.FullName);
        }
    }
}
