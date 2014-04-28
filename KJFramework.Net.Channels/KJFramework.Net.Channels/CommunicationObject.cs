using System;
using System.Collections.Generic;
using KJFramework.Basic.Enum;
using KJFramework.Net.Channels.Enums;
using KJFramework.Statistics;

namespace KJFramework.Net.Channels
{
    /// <summary>
    ///     Í¨Ñ¶¶ÔÏó³éÏóÀà£¬Ìá¹©ÁËÏà¹ØµÄ»ù±¾²Ù×÷¡£
    /// </summary>
    public abstract class CommunicationObject : ICommunicationObject
    {
        #region Implementation of IStatisticable<IStatistic>

        protected Dictionary<StatisticTypes, IStatistic> _statistics;
        protected bool _enable;
        protected CommunicationStates _communicationState;

        /// <summary>
        /// »ñÈ¡»òÉèÖÃÍ³¼ÆÆ÷
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
        ///     获取或设置附属标记
        /// </summary>
        public object Tag { get; set; }
        /// <summary>
        ///     Í£Ö¹
        /// </summary>
        public abstract void Abort();
        /// <summary>
        ///     ´ò¿ª
        /// </summary>
        public abstract void Open();
        /// <summary>
        ///     ¹Ø±Õ
        /// </summary>
        public abstract void Close();
        /// <summary>
        ///     Òì²½´ò¿ª
        /// </summary>
        /// <param name="callback">»Øµ÷º¯Êý</param>
        /// <param name="state">×´Ì¬</param>
        /// <returns>·µ»ØÒì²½½á¹û</returns>
        public virtual IAsyncResult BeginOpen(AsyncCallback callback, object state)
        {
            Action action = Open;
            return action.BeginInvoke(callback, state);
        }
        /// <summary>
        ///     Òì²½¹Ø±Õ
        /// </summary>
        /// <param name="callback">»Øµ÷º¯Êý</param>
        /// <param name="state">×´Ì¬</param>
        /// <returns>·µ»ØÒì²½½á¹û</returns>
        public virtual IAsyncResult BeginClose(AsyncCallback callback, object state)
        {
            Action action = Close;
            return action.BeginInvoke(callback, state);
        }
        /// <summary>
        ///     Òì²½´ò¿ª
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
        ///     Òì²½¹Ø±Õ
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
        ///     »ñÈ¡»òÉèÖÃµ±Ç°¿ÉÓÃ×´Ì¬
        /// </summary>
        public bool Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }
        /// <summary>
        ///     »ñÈ¡µ±Ç°Í¨Ñ¶×´Ì¬
        /// </summary>
        public CommunicationStates CommunicationState
        {
            get { return _communicationState; }
        }
        /// <summary>
        ///     ÒÑ¹Ø±ÕÊÂ¼þ
        /// </summary>
        public event EventHandler Closed;
        protected void ClosedHandler(System.EventArgs e)
        {
            EventHandler closed = Closed;
            if (closed != null) closed(this, e);
        }
        /// <summary>
        ///     ÕýÔÚ¹Ø±ÕÊÂ¼þ
        /// </summary>
        public event EventHandler Closing;
        protected void ClosingHandler(System.EventArgs e)
        {
            EventHandler closing = Closing;
            if (closing != null) closing(this, e);
        }
        /// <summary>
        ///     ÒÑ´íÎóÊÂ¼þ
        /// </summary>
        public event EventHandler Faulted;
        protected void FaultedHandler(System.EventArgs e)
        {
            EventHandler faulted = Faulted;
            if (faulted != null) faulted(this, e);
        }
        /// <summary>
        ///     ÒÑ¿ªÆôÊÂ¼þ
        /// </summary>
        public event EventHandler Opened;
        protected void OpenedHandler(System.EventArgs e)
        {
            EventHandler opened = Opened;
            if (opened != null) opened(this, e);
        }
        /// <summary>
        ///     ÕýÔÚ¿ªÆôÊÂ¼þ
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