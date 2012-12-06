using System;
using System.Collections.Generic;
using KJFramework.Basic.Enum;
using KJFramework.Net.Channels.Enums;
using KJFramework.Statistics;

namespace KJFramework.Net.Channels
{
    /// <summary>
    ///     ͨѶ��������࣬�ṩ����صĻ���������
    /// </summary>
    public abstract class CommunicationObject : ICommunicationObject
    {
        #region Implementation of IStatisticable<IStatistic>

        protected Dictionary<StatisticTypes, IStatistic> _statistics;
        protected bool _enable;
        protected CommunicationStates _communicationState;

        /// <summary>
        /// ��ȡ������ͳ����
        /// </summary>
        public Dictionary<StatisticTypes, IStatistic> Statistics
        {
            get { return _statistics; }
            set { _statistics = value; }
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

        #region Implementation of ICommunicationObject

        /// <summary>
        ///     ֹͣ
        /// </summary>
        public abstract void Abort();
        /// <summary>
        ///     ��
        /// </summary>
        public abstract void Open();
        /// <summary>
        ///     �ر�
        /// </summary>
        public abstract void Close();
        /// <summary>
        ///     �첽��
        /// </summary>
        /// <param name="callback">�ص�����</param>
        /// <param name="state">״̬</param>
        /// <returns>�����첽���</returns>
        public virtual IAsyncResult BeginOpen(AsyncCallback callback, object state)
        {
            Action action = Open;
            return action.BeginInvoke(callback, state);
        }
        /// <summary>
        ///     �첽�ر�
        /// </summary>
        /// <param name="callback">�ص�����</param>
        /// <param name="state">״̬</param>
        /// <returns>�����첽���</returns>
        public virtual IAsyncResult BeginClose(AsyncCallback callback, object state)
        {
            Action action = Close;
            return action.BeginInvoke(callback, state);
        }
        /// <summary>
        ///     �첽��
        /// </summary>
        public virtual void EndOpen(IAsyncResult result)
        {
            if (result != null)
            {
                Action action = (Action) result.AsyncState;
                action.EndInvoke(result);
            }
        }
        /// <summary>
        ///     �첽�ر�
        /// </summary>
        public virtual void EndClose(IAsyncResult result)
        {
            if (result != null)
            {
                Action action = (Action)result.AsyncState;
                action.EndInvoke(result);
            }
        }
        /// <summary>
        ///     ��ȡ�����õ�ǰ����״̬
        /// </summary>
        public bool Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }
        /// <summary>
        ///     ��ȡ��ǰͨѶ״̬
        /// </summary>
        public CommunicationStates CommunicationState
        {
            get { return _communicationState; }
        }
        /// <summary>
        ///     �ѹر��¼�
        /// </summary>
        public event EventHandler Closed;
        protected void ClosedHandler(System.EventArgs e)
        {
            EventHandler closed = Closed;
            if (closed != null) closed(this, e);
        }
        /// <summary>
        ///     ���ڹر��¼�
        /// </summary>
        public event EventHandler Closing;
        protected void ClosingHandler(System.EventArgs e)
        {
            EventHandler closing = Closing;
            if (closing != null) closing(this, e);
        }
        /// <summary>
        ///     �Ѵ����¼�
        /// </summary>
        public event EventHandler Faulted;
        protected void FaultedHandler(System.EventArgs e)
        {
            EventHandler faulted = Faulted;
            if (faulted != null) faulted(this, e);
        }
        /// <summary>
        ///     �ѿ����¼�
        /// </summary>
        public event EventHandler Opened;
        protected void OpenedHandler(System.EventArgs e)
        {
            EventHandler opened = Opened;
            if (opened != null) opened(this, e);
        }
        /// <summary>
        ///     ���ڿ����¼�
        /// </summary>
        public event EventHandler Opening;
        protected void OpeningHandler(System.EventArgs e)
        {
            EventHandler opening = Opening;
            if (opening != null) opening(this, e);
        }

        #endregion
    }
}