using KJFramework.Tracing;
using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Emit;

namespace KJFramework.Net.Pool
{
    /// <summary>
    ///     基于.NETFRAMEWORK 3.5中SocketAsyncEventArgs的缓存池
    /// </summary>
    public class SocketAsyncEventArgsPool : IDisposable
    {
        #region Members

        private readonly int _capacity;
        protected ConcurrentQueue<SocketAsyncEventArgs> _args;
        /// <summary>
        ///     基于.NETFRAMEWORK 3.5中SocketAsyncEventArgs的缓存池
        ///     <para>* 初始化数量：10000。</para>
        /// </summary>
        public static readonly SocketAsyncEventArgsPool Instance = new SocketAsyncEventArgsPool(10000);
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (SocketAsyncEventArgsPool));
        private static Action<SocketAsyncEventArgs> _clrearMethod;

        #endregion

        #region 构造函数

        /// <summary>
        ///     基于.NETFRAMEWORK 3.5中SocketAsyncEventArgs的缓存池
        /// </summary>
        /// <param name="capacity">初始化缓存数</param>
        protected SocketAsyncEventArgsPool(int capacity)
        {
            if (capacity <= 0)
            {
                throw new System.Exception("非法的缓存个数。");
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
        ///     初始化
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
        ///     归还
        /// </summary>
        /// <param name="args">异步事件对象</param>
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
        ///     租借
        /// </summary>
        /// <returns>返回租借后的对象</returns>
        public SocketAsyncEventArgs Rent()
        {
            if (!_args.IsEmpty)
            {
                SocketAsyncEventArgs args;
                if(_args.TryDequeue(out args))
                {
                    return args;
                }
                _tracing.Warn("无法租借SocketAsyncEventArgs, 因为尝试出队列的动作没有成功执行。");
            }
            return null;
        }

        #endregion
    }
}