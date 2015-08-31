using System;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace KJFramework.Net
{
    /// <summary>
    ///     套接字帮助器
    /// </summary>
    internal static class SocketHelper
    {
        #region Constructor

        /// <summary>
        ///     套接字帮助器
        /// </summary>
        static SocketHelper()
        {
            #region Create clear method.

            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            Type t_SocketAsyncEventArgs = typeof(SocketAsyncEventArgs);
            FieldInfo m_CurrentSocket = t_SocketAsyncEventArgs.GetField("m_CurrentSocket", flags);
            FieldInfo m_AcceptSocket = t_SocketAsyncEventArgs.GetField("m_AcceptSocket", flags);
            FieldInfo m_ConnectSocket = t_SocketAsyncEventArgs.GetField("m_ConnectSocket", flags);
            FieldInfo m_PinnedSocketAddress = t_SocketAsyncEventArgs.GetField("m_PinnedSocketAddress", flags);
            FieldInfo m_SocketAddressGCHandle = t_SocketAsyncEventArgs.GetField("m_SocketAddressGCHandle", flags);
            Type t_GCHandle = typeof(GCHandle);
            MethodInfo GCHandle_IsAllocated = t_GCHandle.GetProperty("IsAllocated").GetGetMethod();
            MethodInfo GCHandle_Free = t_GCHandle.GetMethod("Free");
            DynamicMethod method = new DynamicMethod("_SocketAsyncEventArgs_Clear_", typeof(void), new[] { typeof(SocketAsyncEventArgs) }, typeof(object), true);
            ILGenerator il = method.GetILGenerator();
            Label ret = il.DefineLabel();
            // m_CurrentSocket = null;
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Stfld, m_CurrentSocket);
            // m_AcceptSocket = null;
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Stfld, m_AcceptSocket);
            // m_ConnectSocket = null;
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Stfld, m_ConnectSocket);
            // if (m_PinnedSocketAddress != null) 
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, m_PinnedSocketAddress);
            il.Emit(OpCodes.Brfalse_S, ret);
            //   if (m_SocketAddressGCHandle.IsAllocated) 
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldflda, m_SocketAddressGCHandle);
            il.Emit(OpCodes.Call, GCHandle_IsAllocated);
            il.Emit(OpCodes.Brfalse_S, ret);
            //     m_SocketAddressGCHandle.Free();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldflda, m_SocketAddressGCHandle);
            il.Emit(OpCodes.Call, GCHandle_Free);
            //     m_PinnedSocketAddress = null;
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Stfld, m_PinnedSocketAddress);
            // return;
            il.MarkLabel(ret);
            il.Emit(OpCodes.Ret);
            _clearMethod = (Action<SocketAsyncEventArgs>)method.CreateDelegate(typeof(Action<SocketAsyncEventArgs>));

            #endregion
        }

        #endregion

        #region Membes

        private static readonly Action<SocketAsyncEventArgs> _clearMethod;

        #endregion

        #region Methods

        /// <summary>
        ///     清理一个SocketAsyncEventArgs内部资源
        /// </summary>
        /// <param name="args">SocketAsyncEventArgs</param>
        public static void Clear(SocketAsyncEventArgs args)
        {
            _clearMethod(args);
        }

        #endregion
    }
}