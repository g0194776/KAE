using System;
using KJFramework.Net.Enums;
using KJFramework.Tracing;

namespace KJFramework.Net.Uri
{
    /// <summary>
    ///     Pipe×ÊÔ´µØÖ·±êÊ¾Àà£¬Ìá¹©ÁËÏà¹ØµÄ»ù±¾²Ù×÷¡£
    /// </summary>
    public class PipeUri : Uri
    {
        #region ³ÉÔ±

        protected String _machineName;
        protected String _pipeName;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(PipeUri));

        /// <summary>
        ///     »ñÈ¡»òÉèÖÃIPCÃû³Æ
        /// </summary>
        public String PipeName
        {
            get { return _pipeName; }
            set { _pipeName = value; }
        }

        /// <summary>
        ///     »ñÈ¡»òÉèÖÃ»úÆ÷Ãû
        /// </summary>
        public String MachineName
        {
            get { return _machineName; }
            set { _machineName = value; }
        }

        #endregion

        #region ¹¹Ôìº¯Êý

        /// <summary>
        ///     ×ÊÔ´µØÖ·±êÊ¾Àà£¬Ìá¹©ÁËÏà¹ØµÄ»ù±¾²Ù×÷¡£
        /// </summary>
        public PipeUri() : base("")
        { }

        /// <summary>
        ///     ×ÊÔ´µØÖ·±êÊ¾Àà£¬Ìá¹©ÁËÏà¹ØµÄ»ù±¾²Ù×÷¡£
        /// </summary>
        /// <param name="url" type="string">
        ///     <para>
        ///         ÍêÕûµÄURLµØÖ·
        ///     </para>
        /// </param>
        public PipeUri(String url) : base(url)
        {
            int offset = url.LastIndexOf('/');
            _serverUri = url.Substring(offset, url.Length - offset);
        }

        #endregion

        #region ¸¸Àà·½·¨

        /// <summary>
        ///    获取当前URL所代表的网络类型
        /// </summary>
        public override NetworkTypes NetworkType
        {
            get { return NetworkTypes.Pipe; }
        }

        /// <summary>
        ///     »ñÈ¡·þÎñÆ÷ÄÚ²¿Ê¹ÓÃµÄUriÐÎÌ¬
        /// </summary>
        /// <returns>·µ»ØUri</returns>
        public override string GetServiceUri()
        {
            return _serverUri;
        }

        protected override void Split()
        {
            base.Split();
            try
            {
                if (_prefix.ToLower() != "pipe")
                {
                    throw new UriFormatException("·Ç·¨µÄPipe×ÊÔ´µØÖ·±êÊ¾¡£");
                }
                String[] inners = Address.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                if (inners.Length <= 1 || inners.Length > 2)
                {
                    throw new UriFormatException("·Ç·¨µÄPipe×ÊÔ´µØÖ·±êÊ¾¡£");
                }
                _machineName = inners[0];
                _pipeName = inners[1];
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                throw new System.UriFormatException("·Ç·¨µÄPipe×ÊÔ´µØÖ·±êÊ¾¡£");
            }
        }

        #endregion
    }
}