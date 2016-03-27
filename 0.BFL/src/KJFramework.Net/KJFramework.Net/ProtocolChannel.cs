using System;
using KJFramework.EventArgs;

namespace KJFramework.Net
{
    /// <summary>
    ///     Э��ͨ��������࣬�ṩ����صĻ���������
    /// </summary>
    internal abstract class ProtocolChannel : ServiceChannel, IProtocolChannel
    {
        #region IProtocolChannel ��Ա

        /// <summary>
        ///     ����Э����Ϣ
        /// </summary>
        /// <typeparam name="TMessage">Э����Ϣ����</typeparam>
        /// <returns>����Э����Ϣ</returns>
        public abstract TMessage CreateProtocolMessage<TMessage>();

        /// <summary>
        ///     �����¼�
        /// </summary>
        public event EventHandler<LightMultiArgEventArgs<Object>> Requested;
        protected void RequestedHandler(LightMultiArgEventArgs<object> e)
        {
            EventHandler<LightMultiArgEventArgs<object>> requested = Requested;
            if (requested != null) requested(this, e);
        }

        /// <summary>
        ///     �����¼�
        /// </summary>
        public event EventHandler<LightMultiArgEventArgs<Object>> Responsed;
        protected void InvokeResponsed(LightMultiArgEventArgs<object> e)
        {
            EventHandler<LightMultiArgEventArgs<object>> responsed = Responsed;
            if (responsed != null) responsed(this, e);
        }

        #endregion
    }
}