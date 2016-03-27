using System;
using KJFramework.Net.Enums;
using KJFramework.Statistics;

namespace KJFramework.Net
{
    /// <summary>
    ///     Í¨Ñ¶¶ÔÏóÔª½Ó¿Ú£¬Ìá¹©ÁËÏà¹ØµÄ»ù±¾²Ù×÷¡£
    /// </summary>
    public interface ICommunicationObject : IStatisticable<IStatistic>, IDisposable
    {
        /// <summary>
        ///     Í£Ö¹
        /// </summary>
        void Abort();
        /// <summary>
        ///     ´ò¿ª
        /// </summary>
        void Open();
        /// <summary>
        ///     ¹Ø±Õ
        /// </summary>
        void Close();
        /// <summary>
        ///     Òì²½´ò¿ª
        /// </summary>
        /// <param name="callback">»Øµ÷º¯Êý</param>
        /// <param name="state">×´Ì¬</param>
        /// <returns>·µ»ØÒì²½½á¹û</returns>
        IAsyncResult BeginOpen(AsyncCallback callback, Object state);
        /// <summary>
        ///     Òì²½¹Ø±Õ
        /// </summary>
        /// <param name="callback">»Øµ÷º¯Êý</param>
        /// <param name="state">×´Ì¬</param>
        /// <returns>·µ»ØÒì²½½á¹û</returns>
        IAsyncResult BeginClose(AsyncCallback callback, Object state);
        /// <summary>
        ///     Òì²½´ò¿ª
        /// </summary>
        void EndOpen(IAsyncResult result);
        /// <summary>
        ///     Òì²½¹Ø±Õ
        /// </summary>
        void EndClose(IAsyncResult result);
        /// <summary>
        ///     »ñÈ¡»òÉèÖÃµ±Ç°¿ÉÓÃ×´Ì¬
        /// </summary>
        bool Enable { get; set; }
        /// <summary>
        ///     获取或设置附属标记
        /// </summary>
        object Tag { get; set; }
        /// <summary>
        ///     »ñÈ¡µ±Ç°Í¨Ñ¶×´Ì¬
        /// </summary>
        CommunicationStates CommunicationState { get; }
        /// <summary>
        ///     ÒÑ¹Ø±ÕÊÂ¼þ
        /// </summary>
        event EventHandler Closed;
        /// <summary>
        ///     ÕýÔÚ¹Ø±ÕÊÂ¼þ
        /// </summary>
        event EventHandler Closing;
        /// <summary>
        ///     ÒÑ´íÎóÊÂ¼þ
        /// </summary>
        event EventHandler Faulted;
        /// <summary>
        ///     ÒÑ¿ªÆôÊÂ¼þ
        /// </summary>
        event EventHandler Opened;
        /// <summary>
        ///     ÕýÔÚ¿ªÆôÊÂ¼þ
        /// </summary>
        event EventHandler Opening;
    }
}