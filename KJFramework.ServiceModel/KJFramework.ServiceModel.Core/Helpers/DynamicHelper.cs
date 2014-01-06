using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using KJFramework.Helpers;
using KJFramework.Net.Transaction.Identities;
using KJFramework.ServiceModel.Core.Attributes;
using KJFramework.ServiceModel.Core.Contracts;
using KJFramework.ServiceModel.Core.EventArgs;
using KJFramework.ServiceModel.Core.Managers;
using KJFramework.ServiceModel.Core.Objects;

namespace KJFramework.ServiceModel.Core.Helpers
{
    /// <summary>
    ///     ��̬���������ṩ����صĻ���������
    /// </summary>
    public class DynamicHelper
    {
        public delegate object FastInvokeHandler(object target, object[] paramters);

        /// <summary>
        ///     ����һ��������Լ������ʵ��
        ///     <para>* �봫��TΪһ���ӿ����͡�</para>
        /// </summary>
        /// <typeparam name="T">������Լ�ӿ�</typeparam>
        /// <returns>���ط�����������ʵ��</returns>
        public static T Create<T>()
            where T : class
        {
            #region ��ȡ������Լ����

            ServiceMethodPickupObject[] methods = null;
            Type service = typeof(T);
            ServiceContractAttribute contract = AttributeHelper.GetCustomerAttribute<ServiceContractAttribute>(service);
            if (contract == null)
            {
                Type[] interfaceTypes = service.GetInterfaces();
                if (interfaceTypes == null || interfaceTypes.Length == 0) throw new System.Exception("invalid service contract !");
                //��ӺϷ����жϣ�������
                foreach (Type interfaceType in interfaceTypes)
                {
                    contract = AttributeHelper.GetCustomerAttribute<ServiceContractAttribute>(interfaceType);
                    //�ҵ�һ���ӿ�
                    if (contract != null)
                    {
                        methods = Enumerable.DefaultIfEmpty(ServiceHelper.GetServiceMethods(interfaceType)).ToArray();
                        break;
                    }
                }
                if (contract == null) throw new System.Exception("invalid service contract !");
            }
            else methods = ServiceHelper.GetServiceMethods(service);

            #endregion

            #region ������̬����

            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("KJFramework.ServiceModel.Dynamic.Instance"), AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("KJFramework.ServiceModel.Dynamic.Module");
            TypeBuilder typeBuilder = moduleBuilder.DefineType(service.FullName, TypeAttributes.Public | TypeAttributes.Class, typeof(ContractDefaultAction), new[] { service });

            #endregion

            if (methods == null || methods.Length == 0) throw new System.Exception("��ǰ������Լ��Ӧ�����ٴ���һ����Լ������");

            #region ʵ�ַ���

            foreach (ServiceMethodPickupObject tempMethod in methods)
            {
                MethodInfo methodInfo = tempMethod.Method.GetCoreMethod();
                MethodBuilder methodBuilder = typeBuilder.DefineMethod(methodInfo.Name, MethodAttributes.Public | MethodAttributes.Virtual,
                                         methodInfo.ReturnType,
                                         methodInfo.GetParameters().Select(parameter => parameter.ParameterType).ToArray());
                WriteProgram<T>(methodInfo, tempMethod.Operation, methodBuilder.GetILGenerator());
            }

            #endregion)

            Type newType = typeBuilder.CreateType();
            return (T)newType.Assembly.CreateInstance(newType.FullName);
        }

        /// <summary>
        ///     ��һ��������д���������(MSIL)
        /// </summary>
        /// <typeparam name="T">������Լ</typeparam>
        /// <param name="methodInfo">������Ϣ</param>
        /// <param name="attribute">������������</param>
        /// <param name="ilGenerator">IL������</param>
        private static void WriteProgram<T>(MethodInfo methodInfo, OperationAttribute attribute, ILGenerator ilGenerator)
        {
            #region Declare some ctor(s) and fied info.

            ConstructorInfo ctor1 = typeof(ClientLowProxyRequestEventArgs).GetConstructor(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    null,
                    new Type[] { },
                    null
                );

            ConstructorInfo ctor2 = typeof(AfterCallEventArgs).GetConstructor(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    null,
                    new[] { typeof(TransactionIdentity), typeof(bool), typeof(bool) },
                    null
                );
            MethodInfo method2 = typeof(ContractDefaultAction).GetMethod(
                "CallingHandler",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { typeof(ClientLowProxyRequestEventArgs) },
                null
                );
            MethodInfo afterCallMethod = typeof(ContractDefaultAction).GetMethod(
                "AfterCallHandler",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { typeof(AfterCallEventArgs) },
                null
                );
            MethodInfo method5;
            MethodInfo method3;
            MethodInfo method4 = typeof(ContractDefaultAction).GetMethod(
                "Create",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { typeof(bool) },
                null
                );
            FieldInfo field1 = typeof(ContractDefaultAction).GetField("_manager", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            FieldInfo identity = typeof(ClientLowProxyRequestEventArgs).GetField("Identity", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo methodToken = typeof(ClientLowProxyRequestEventArgs).GetField("MethodToken", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo isOneWay = typeof(ClientLowProxyRequestEventArgs).GetField("NeedAck", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo isAsync = typeof(ClientLowProxyRequestEventArgs).GetField("IsAsync", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo hasCallback = typeof(ClientLowProxyRequestEventArgs).GetField("HasCallback", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo arguments = typeof(ClientLowProxyRequestEventArgs).GetField("Arguments", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            //������ʵ������ʱ��index 0 �Ĳ���λ���� index 1�ϡ�
            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            int length = parameterInfos.Length + 1;
            LocalBuilder objValues = ilGenerator.DeclareLocal(typeof(Object[]));
            LocalBuilder eventArgs = ilGenerator.DeclareLocal(typeof(ClientLowProxyRequestEventArgs));
            LocalBuilder afterCallEventArgs = ilGenerator.DeclareLocal(typeof(AfterCallEventArgs));
            #endregion

            #region Initialize event args.

            ilGenerator.Emit(OpCodes.Nop);
            ilGenerator.Emit(OpCodes.Newobj, ctor1);
            ilGenerator.Emit(OpCodes.Stloc, eventArgs);

            #endregion

            #region Initialize argument(s) array.

            if (parameterInfos.Length > 0)
            {
                ilGenerator.Emit(OpCodes.Ldc_I4, parameterInfos.Length);
                ilGenerator.Emit(OpCodes.Newarr, typeof(Object));
                ilGenerator.Emit(OpCodes.Stloc, objValues);
            }

            #endregion

            //isasnyc  field.
            ilGenerator.Emit(OpCodes.Ldloc, eventArgs);
            ilGenerator.Emit(OpCodes.Ldc_I4, attribute.IsAsync ? 1 : 0);
            ilGenerator.Emit(OpCodes.Stfld, isAsync);

            #region Generate session id.

            ilGenerator.Emit(OpCodes.Ldloc, eventArgs);
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldloc, eventArgs);
            ilGenerator.Emit(OpCodes.Ldfld, isAsync);
            ilGenerator.Emit(OpCodes.Call, method4);
            ilGenerator.Emit(OpCodes.Stfld, identity);

            #endregion

            #region Initialize other field(s).

            //method token field.
            ilGenerator.Emit(OpCodes.Ldloc, eventArgs);
            ilGenerator.Emit(OpCodes.Ldc_I4, attribute.MethodToken == 0 ? methodInfo.MetadataToken : attribute.MethodToken);
            ilGenerator.Emit(OpCodes.Stfld, methodToken);
            //is one way field.
            ilGenerator.Emit(OpCodes.Ldloc, eventArgs);
            ilGenerator.Emit(OpCodes.Ldc_I4, attribute.IsOneWay ? 0 : 1);
            ilGenerator.Emit(OpCodes.Stfld, isOneWay);

            //hasCallback  field.
            ilGenerator.Emit(OpCodes.Ldloc, eventArgs);
            ilGenerator.Emit(OpCodes.Ldc_I4, (parameterInfos.Length > 0 && attribute.IsAsync) ? 1 : 0);
            ilGenerator.Emit(OpCodes.Stfld, hasCallback);

            #endregion

            #region Initialize method argument(s).

            if (length > 1)
            {
                //�������в�������װ��
                for (int i = 1; i < length; i++)
                {
                    ilGenerator.Emit(OpCodes.Ldloc, objValues);
                    ilGenerator.Emit(OpCodes.Ldc_I4, (i - 1));
                    ilGenerator.Emit(OpCodes.Ldarg, i);
                    ilGenerator.Emit(OpCodes.Box, parameterInfos[i - 1].ParameterType);
                    ilGenerator.Emit(OpCodes.Stelem_Ref);
                }
            }

            #endregion

            #region Calling handler.

            ilGenerator.Emit(OpCodes.Ldloc, eventArgs);
            ilGenerator.Emit(OpCodes.Ldloc, objValues);
            ilGenerator.Emit(OpCodes.Stfld, arguments);
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldloc, eventArgs);
            //calling handler.
            ilGenerator.Emit(OpCodes.Call, method2);

            #endregion

            #region Initialize after call event args.

            ilGenerator.Emit(OpCodes.Nop);
            ilGenerator.Emit(OpCodes.Ldloc, eventArgs);
            ilGenerator.Emit(OpCodes.Ldfld, identity);
            ilGenerator.Emit(OpCodes.Ldc_I4, attribute.IsAsync ? 1 : 0);
            ilGenerator.Emit(OpCodes.Ldc_I4, attribute.IsOneWay ? 1 : 0);
            ilGenerator.Emit(OpCodes.Newobj, ctor2);
            ilGenerator.Emit(OpCodes.Stloc, afterCallEventArgs);

            #endregion

            #region Active after call event.

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldloc, afterCallEventArgs);
            ilGenerator.Emit(OpCodes.Callvirt, afterCallMethod);

            #endregion

            //async mode.
            if (attribute.IsAsync)
            {
                ilGenerator.Emit(OpCodes.Ret);
                return;
            }

            if (!attribute.IsOneWay && !attribute.IsAsync && methodInfo.ReturnParameter != null && methodInfo.ReturnType.FullName.ToLower() == "system.void")
            {
                method5 = typeof(RequestManager).GetMethod(
                    "CheckException", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    null, new[] { typeof(TransactionIdentity) }, null);
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldfld, field1);
                ilGenerator.Emit(OpCodes.Ldloc, eventArgs);
                ilGenerator.Emit(OpCodes.Ldfld, identity);
                ilGenerator.Emit(OpCodes.Callvirt, method5);
            }
            //sync mode & has return value.
            if (!attribute.IsOneWay && methodInfo.ReturnParameter != null && methodInfo.ReturnType.FullName.ToLower() != "system.void")
            {
                method3 = typeof(RequestManager).GetMethod(
                "GetResult", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { typeof(TransactionIdentity), typeof(bool) },
                null
                ).MakeGenericMethod(methodInfo.ReturnType);
                //get return value.
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldfld, field1);
                ilGenerator.Emit(OpCodes.Ldloc, eventArgs);
                ilGenerator.Emit(OpCodes.Ldfld, identity);
                ilGenerator.Emit(OpCodes.Ldc_I4, attribute.IsAsync ? 1 : 0);
                ilGenerator.Emit(OpCodes.Callvirt, method3);
            }

            //ilGenerator.Emit(OpCodes.Ldnull);
            ilGenerator.Emit(OpCodes.Ret);
        }

        #region Methods

        public static FastInvokeHandler GetMethodInvoker(MethodInfo methodInfo)
        {
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(object),
                                                            new Type[] { typeof(object), typeof(object[]) },
                                                            methodInfo.DeclaringType.Module);
            ILGenerator il = dynamicMethod.GetILGenerator();
            ParameterInfo[] ps = methodInfo.GetParameters();
            Type[] paramTypes = new Type[ps.Length];
            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                    paramTypes[i] = ps[i].ParameterType.GetElementType();
                else
                    paramTypes[i] = ps[i].ParameterType;
            }
            LocalBuilder[] locals = new LocalBuilder[paramTypes.Length];
            for (int i = 0; i < paramTypes.Length; i++)
            {
                locals[i] = il.DeclareLocal(paramTypes[i], true);
            }
            for (int i = 0; i < paramTypes.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_1);
                EmitFastInt(il, i);
                il.Emit(OpCodes.Ldelem_Ref);
                EmitCastToReference(il, paramTypes[i]);
                il.Emit(OpCodes.Stloc, locals[i]);
            }
            if (!methodInfo.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                    il.Emit(OpCodes.Ldloca_S, locals[i]);
                else
                    il.Emit(OpCodes.Ldloc, locals[i]);
            }
            if (methodInfo.IsStatic)
                il.EmitCall(OpCodes.Call, methodInfo, null);
            else
                il.EmitCall(OpCodes.Callvirt, methodInfo, null);
            if (methodInfo.ReturnType == typeof(void))
                il.Emit(OpCodes.Ldnull);
            else
                EmitBoxIfNeeded(il, methodInfo.ReturnType);

            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                {
                    il.Emit(OpCodes.Ldarg_1);
                    EmitFastInt(il, i);
                    il.Emit(OpCodes.Ldloc, locals[i]);
                    if (locals[i].LocalType.IsValueType)
                        il.Emit(OpCodes.Box, locals[i].LocalType);
                    il.Emit(OpCodes.Stelem_Ref);
                }
            }

            il.Emit(OpCodes.Ret);
            FastInvokeHandler invoder = (FastInvokeHandler)dynamicMethod.CreateDelegate(typeof(FastInvokeHandler));
            return invoder;
        }

        private static void EmitCastToReference(ILGenerator il, Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, type);
            }
            else
            {
                il.Emit(OpCodes.Castclass, type);
            }
        }

        private static void EmitBoxIfNeeded(ILGenerator il, Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Box, type);
            }
        }

        private static void EmitFastInt(ILGenerator il, int value)
        {
            switch (value)
            {
                case -1:
                    il.Emit(OpCodes.Ldc_I4_M1);
                    return;
                case 0:
                    il.Emit(OpCodes.Ldc_I4_0);
                    return;
                case 1:
                    il.Emit(OpCodes.Ldc_I4_1);
                    return;
                case 2:
                    il.Emit(OpCodes.Ldc_I4_2);
                    return;
                case 3:
                    il.Emit(OpCodes.Ldc_I4_3);
                    return;
                case 4:
                    il.Emit(OpCodes.Ldc_I4_4);
                    return;
                case 5:
                    il.Emit(OpCodes.Ldc_I4_5);
                    return;
                case 6:
                    il.Emit(OpCodes.Ldc_I4_6);
                    return;
                case 7:
                    il.Emit(OpCodes.Ldc_I4_7);
                    return;
                case 8:
                    il.Emit(OpCodes.Ldc_I4_8);
                    return;
            }

            if (value > -129 && value < 128)
            {
                il.Emit(OpCodes.Ldc_I4_S, (SByte)value);
            }
            else
            {
                il.Emit(OpCodes.Ldc_I4, value);
            }
        }

        //internal static void WriteProgram(MethodInfo methodInfo, ILGenerator ilGenerator)
        //{
        //    BinaryArgContext[] binaryArgContexts = null;
        //    //������ʵ������ʱ��index 0 �Ĳ���Ϊֹ�� index 1�ϡ�
        //    ParameterInfo[] parameterInfos = methodInfo.GetParameters();
        //    int length = parameterInfos.Length + 1;
        //    if (length > 1)
        //    {
        //        /*
        //         * ���￪ʼ��װ����
        //         * ��������װΪһ�� BinaryArgContext ���顣
        //         * 
        //         * [δ�꣬������]
        //         * 2010-05-24
        //         */
        //        LocalBuilder requestMethodObjectBuilder = ilGenerator.DeclareLocal(typeof(RequestMethodObject));
        //        LocalBuilder requestServiceMessage = ilGenerator.DeclareLocal(typeof(RequestServiceMessage));

        //        FieldInfo[] args = new FieldInfo[parameterInfos.Length];
        //        args = args.Select((field, i) => methodInfo.GetType().GetField(parameterInfos[i].Name)).ToArray();

        //        ilGenerator.Emit(OpCodes.Newobj, typeof(RequestMethodObject));

        //        for (int i = 1; i < length; i++)
        //        {
        //            ilGenerator.Emit(OpCodes.Ldarg, i);
        //            ilGenerator.Emit(OpCodes.Stloc, args[i - 1]);
        //        }
        //        //����Ҫ��������¼��Ĳ�����
        //        //MSIL Ends.
        //        ilGenerator.Emit(OpCodes.Ret);
        //        RequestServiceMessage message = new RequestServiceMessage();
        //        RequestMethodObject requestMethodObject = new RequestMethodObject(args.Length);
        //        requestMethodObject.MethodFullName = methodInfo.Name;
        //        for (int i = 0; i < args.Length; i++)
        //        {
        //            requestMethodObject.AddArg((BinaryArgContext)ArgumentHelper.ConvertArg(args[i].FieldType, false, i));
        //        }
        //        message.RequestObject = requestMethodObject;
        //    }
        //}

        #endregion
    }
}