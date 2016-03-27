using KJFramework.Tracing;
using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Emit;

namespace KJFramework.Net.Pool
{
    /// <summary>
    ///     ����.NETFRAMEWORK 3.5��SocketAsyncEventArgs�Ļ����
    /// </summary>
    public class SocketAsyncEventArgsPool : IDisposable
    {
        #region Members

        private readonly int _capacity;
        protected ConcurrentQueue<SocketAsyncEventArgs> _args;
        /// <summary>
        ///     ����.NETFRAMEWORK 3.5��SocketAsyncEventArgs�Ļ����
        ///     <para>* ��ʼ��������10000��</para>
        /// </summary>
        public static readonly SocketAsyncEventArgsPool Instance = new SocketAsyncEventArgsPool(10000);
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (SocketAsyncEventArgsPool));
        private static Action<SocketAsyncEventArgs> _clrearMethod;

        #endregion

        #region ���캯��

        /// <summary>
        ///     ����.NETFRAMEWORK 3.5��SocketAsyncEventArgs�Ļ����
        /// </summary>
        /// <param name="capacity">��ʼ��������</param>
        protected SocketAsyncEventArgsPool(int capacity)
        {
            if (capacity <= 0)
            {
                throw new System.Exception("�Ƿ��Ļ��������");
            }
            _capacity = capacity;
            Initialize();
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Functions

        /// <summary>
        ///     ��ʼ��
        /// </summary>
        protected void Initialize()
        {
            #region Create clear method

            FieldInfo fieldInfo = typeof(SocketAsyncEventArgs).GetField("m_CurrentSocket", BindingFlags.Instance | BindingFlags.NonPublic);
            DynamicMethod dynamicMethod = new DynamicMethod("ClearSocket", typeof(void), new[] { typeof(SocketAsyncEventArgs) }, true);
            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldnull);
            ilGenerator.Emit(OpCodes.Stfld, fieldInfo);
            ilGenerator.Emit(OpCodes.Ret);
            _clrearMethod = (Action<SocketAsyncEventArgs>)dynamicMethod.CreateDelegate(typeof(Action<SocketAsyncEventArgs>));

            #endregion
            _args = new ConcurrentQueue<SocketAsyncEventArgs>();
            for (int i = 0; i < _capacity; i++)
            {
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                _args.Enqueue(args);
            }
        }

        /// <summary>
        ///     �黹
        /// </summary>
        /// <param name="args">�첽�¼�����</param>
        public void Giveback(SocketAsyncEventArgs args)
        {
            if (args == null)
            {
                return;
            }
            args.BufferList = null;
            args.AcceptSocket = null;
            args.UserToken = null;
            args.RemoteEndPoint = null;
            args.SetBuffer(null, 0, 0);
            _clrearMethod(args);
            _args.Enqueue(args);
        }

        /// <summary>
        ///     ���
        /// </summary>
        /// <returns>��������Ķ���</returns>
        public SocketAsyncEventArgs Rent()
        {
            if (!_args.IsEmpty)
            {
                SocketAsyncEventArgs args;
                if(_args.TryDequeue(out args))
                {
                    return args;
                }
                _tracing.Warn("�޷����SocketAsyncEventArgs, ��Ϊ���Գ����еĶ���û�гɹ�ִ�С�");
            }
            return null;
        }

        #endregion
    }
}