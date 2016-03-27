using System;
using System.Collections.Generic;
using System.IO.Pipes;
using KJFramework.EventArgs;
using KJFramework.Net.Uri;
using KJFramework.Tracing;

namespace KJFramework.Net.HostChannels
{
    /// <summary>
    ///     �����ܵ�����ͨѶͨ�����ṩ����صĻ�������
    /// </summary>
    public class PipeHostTransportChannel : HostTransportChannel, IPiepHostTransportChannel
    {
        #region Constructors

        /// <summary>
        ///     �����ܵ�����ͨѶͨ�����ṩ����صĻ�������
        /// </summary>
        /// <param name="name">�ܵ�����URI</param>
        /// <exception cref="System.ArgumentException">��������</exception>
        /// <exception cref="System.ArgumentNullException">��������</exception>
        public PipeHostTransportChannel(String name) :
            this(new PipeUri(name), 254)
        { }

        /// <summary>
        ///     �����ܵ�����ͨѶͨ�����ṩ����صĻ�������
        /// </summary>
        /// <param name="name">�ܵ�����URI</param>
        /// <param name="instanceCount">ʵ����</param>
        /// <exception cref="System.ArgumentException">��������</exception>
        /// <exception cref="System.ArgumentNullException">��������</exception>
        public PipeHostTransportChannel(String name, int instanceCount)
            : this(new PipeUri(name), instanceCount)
        { }

        /// <summary>
        ///     �����ܵ�����ͨѶͨ�����ṩ����صĻ�������
        /// </summary>
        /// <param name="uri">�ܵ�URL</param>
        /// <param name="instanceCount">ʵ����</param>
        /// <exception cref="System.ArgumentException">��������</exception>
        /// <exception cref="System.ArgumentNullException">��������</exception>
        public PipeHostTransportChannel(PipeUri uri, int instanceCount)
        {
            if (uri == null) throw new ArgumentNullException("uri");
            if (instanceCount > 254 || instanceCount <= 0) throw new ArgumentException("#Illegal Named channel instance count.");
            LogicalAddress = uri;
            _name = uri.PipeName;
            _instanceCount = instanceCount;
            _uri = uri;
        }

        #endregion

        #region Members

        protected readonly string _name;
        protected readonly PipeUri _uri;
        protected readonly int _instanceCount;
        private readonly object _lockAvaObj = new object();
        private readonly object _lockUsedObj = new object();
        protected readonly IList<NamedPipeServerStream> _avaStreams = new List<NamedPipeServerStream>();
        protected readonly IList<NamedPipeServerStream> _usedStreams = new List<NamedPipeServerStream>();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(PipeHostTransportChannel));
        /// <summary>
        ///    ��ȡ���õ������ܵ�����
        /// </summary>
        public int AvailableCount { get { return _avaStreams.Count; } }
        /// <summary>
        ///    ��ȡ��ʹ�õ������ܵ�����
        /// </summary>
        public int UsedCount { get { return _usedStreams.Count; } }
        /// <summary>
        ///    ��ȡ���е������ܵ�����
        /// </summary>
        public int TotalCount { get { return _instanceCount; } }

        #endregion

        #region Overrides of HostTransportChannel

        /// <summary>
        ///     ע������
        /// </summary>
        /// <returns>����ע���״̬</returns>
        public override bool Regist()
        {
            return Regist(PipeDirection.InOut);
        }

        /// <summary>
        ///     ע������
        /// </summary>
        /// <param name="direction">�����ܵ�������������</param>
        /// <returns>����ע���״̬</returns>
        public virtual bool Regist(PipeDirection direction)
        {
            if(_usedStreams.Count != 0 || _avaStreams.Count != 0)
                throw new InvalidOperationException("#You cannot did Regist operation again on an initialized Host Channel.");

                for (int i = 0; i < _instanceCount; i++)
                {
                    NamedPipeServerStream stream;
                    lock (_lockAvaObj)
                    {
                         stream= new NamedPipeServerStream(_name, direction, _instanceCount, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
                         _avaStreams.Add(stream);
                    }
                    stream.BeginWaitForConnection(CallbackWaitConnection, stream);
            }
            
            return true;
        }

        /// <summary>
        ///     ע������
        /// </summary>
        /// <returns>����ע���״̬</returns>
        public override bool UnRegist()
        {
            try
            {
                lock (_lockAvaObj)
                {
                    foreach (NamedPipeServerStream stream in _avaStreams)
                    {
                        if (stream.IsConnected) stream.Disconnect();
                        stream.Close();
                    }
                    _avaStreams.Clear();
                }
                lock (_lockUsedObj)
                {
                    foreach (NamedPipeServerStream stream in _usedStreams)
                    {
                        if (stream.IsConnected) stream.Disconnect();
                        stream.Close();
                    }
                    _usedStreams.Clear();
                }
                return true;
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                return true;
            }
        }

        #endregion

        #region Implementation of IPiepHostTransportChannel

        /// <summary>
        ///     ��ȡ�����ܵ���ʵ������
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        ///     ��ȡ�����Ĺܵ���ַ
        /// </summary>
        public PipeUri LogicalAddress { get; private set; }

        /// <summary>
        ///     ��ȡʵ������
        /// </summary>
        public int InstanceCount
        {
            get { return _instanceCount; }
        }

        #endregion

        #region Methods

        private void CallbackWaitConnection(IAsyncResult result)
        {
            NamedPipeServerStream stream = (NamedPipeServerStream)result.AsyncState;
            try
            {
                stream.EndWaitForConnection(result);
                lock (_lockAvaObj) _avaStreams.Remove(stream);
                lock (_lockUsedObj) _usedStreams.Add(stream);
                PipeTransportChannel channel = new PipeTransportChannel(_uri, stream);
                channel.Disconnected += TransportChannelDisconnected;
                //transfers current stream into different collection.
                ChannelCreatedHandler(new LightSingleArgEventArgs<ITransportChannel>(channel));
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
            }
        }
       
        #endregion

        #region Events

        void TransportChannelDisconnected(object sender, System.EventArgs e)
        {
            PipeTransportChannel channel = (PipeTransportChannel)sender;
            channel.Disconnected -= TransportChannelDisconnected;
            //transfers current stream into different collection.
            lock (_lockUsedObj) _usedStreams.Remove((NamedPipeServerStream) channel.Stream);
            lock (_lockAvaObj) _avaStreams.Add((NamedPipeServerStream) channel.Stream);
            ((NamedPipeServerStream)channel.Stream).BeginWaitForConnection(CallbackWaitConnection, channel.Stream);
            ChannelDisconnectedHandler(new LightSingleArgEventArgs<ITransportChannel>(channel));
        }

        #endregion
    }
}