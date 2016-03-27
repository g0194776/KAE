using System;
using System.Net;
using System.Net.Sockets;
using KJFramework.Tracing;

namespace KJFramework.Net.Health
{
    /// <summary>
    ///   基于TCP协议的健康检查通信信道
    /// </summary>
    public class AliveCheckTcpConnection<K>
    {
        #region Constructor

        /// <summary>
        ///   基于TCP协议的健康检查通信信道
        ///     <para>* 使用此构造默认检查间隔为: 30s</para>
        /// </summary>
        /// <remarks>
        ///       当一个健康检查通信信道创建成功后，它的任务就是每隔一个指定的时间段就去检查一次
        ///   通信状态，如果还是无法连接成功则继续等待下一个激活的时间。
        ///       但是，如果连接成功后，该健康检查的通信信道会激活自身的Succeed事件，并自动关闭当前的健康检查信道，
        ///   也就是说，一旦连接成功，那么这个信道将不会继续完成轮训的时间段任务。如果需要重启开启，请调用它的Run()方法。
        ///   
        ///   注意：此信道在初始化的后会自动开启，在初始化后手动调用一次Run()方法
        /// </remarks>
        /// <param name="key">Key名称</param>
        /// <param name="iep">需要检查的远程通信端口</param>
        /// <exception cref="System.ArgumentNullException">参数不能为空</exception>
        public AliveCheckTcpConnection(K key, IPEndPoint iep)
            : this(key, iep, new TimeSpan(0, 0, 30))
        {
        }
        
        /// <summary>
        ///   基于TCP协议的健康检查通信信道
        /// </summary>
        /// <remarks>
        ///       当一个健康检查通信信道创建成功后，它的任务就是每隔一个指定的时间段就去检查一次
        ///   通信状态，如果还是无法连接成功则继续等待下一个激活的时间。
        ///       但是，如果连接成功后，该健康检查的通信信道会激活自身的Succeed事件，并自动关闭当前的健康检查信道，
        ///   也就是说，一旦连接成功，那么这个信道将不会继续完成轮训的时间段任务。如果需要重启开启，请调用它的Run()方法。
        ///   
        ///   注意：此信道在初始化的后会自动开启，在初始化后手动调用一次Run()方法
        /// </remarks>
        /// <param name="key">Key名称</param>
        /// <param name="iep">需要检查的远程通信端口</param>
        /// <param name="timeSpan">检查间隔</param>
        /// <exception cref="System.ArgumentNullException">参数不能为空</exception>
        public AliveCheckTcpConnection(K key, IPEndPoint iep, TimeSpan timeSpan)
        {
            if (iep == null) throw new ArgumentNullException("iep");
            _iep = iep;
            _timeSpan = timeSpan;
            Key = key;
            Run();
        }

        #endregion

        #region Members

        private bool _enable;
        private Socket _socket;
        private IPEndPoint _iep;
        private TimeSpan _timeSpan;
        private System.Threading.Timer _timer;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(AliveCheckTcpConnection<K>));

        /// <summary>
        ///   获取当前通信信道的KEY
        /// </summary>
        public K Key { get; protected set; }

        #endregion

        #region Methods
        
        //thread timer's callback function.
        private void Callback(object state)
        {
            if (!_enable) return;
            if (!CheckCommunication())
            {
                _tracing.Warn("#Health check failed with {0} remote ip endpoint.", _iep);
                return;
            }
            //active event & close currently connection.
            try
            {
                _enable = false;
                if (_timer != null) _timer.Dispose();
                SucceedHandler();
            }
            catch (System.Exception ex) { _tracing.Error(ex, null); }
        }

        //this is a method that whick it used to check currently target network enviroement.
        private bool CheckCommunication()
        {
            Socket socket;
            if (_socket != null) socket = _socket;
            else _socket = socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(_iep);
                if (!socket.Connected) return false;
                //connected & begin to clean current LOW-LAYER resouce.
                _socket.Disconnect(false);
                _socket.Dispose();
                return true;
            }
            catch(System.Exception ex) { _tracing.Error(ex, null); }
            return false;
        }

        /// <summary>
        ///   运行当前的健康检查通信信道
        ///   <para>*注意: 此信道在初始化的后会自动开启，在初始化后手动调用一次Run()方法</para>
        /// </summary>
        public void Run()
        {
            if (_enable) return;
            _timer = new System.Threading.Timer(Callback, this, TimeSpan.Zero, _timeSpan);
            _enable = true;
        }

	    #endregion

        #region Event.

        /// <summary>
        ///   连接成功事件
        /// </summary>
        public event EventHandler Succeed;
        protected virtual void SucceedHandler()
        {
            EventHandler handler = Succeed;
            if (handler != null) handler(this, System.EventArgs.Empty);
        }

        #endregion
    }
}