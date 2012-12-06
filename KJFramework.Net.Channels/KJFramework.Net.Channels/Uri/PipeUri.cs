using System;
using KJFramework.Basic.Enum;
using KJFramework.Logger;

namespace KJFramework.Net.Channels.Uri
{
    /// <summary>
    ///     Pipe资源地址标示类，提供了相关的基本操作。
    /// </summary>
    public class PipeUri : Uri
    {
        #region 成员

        protected String _machineName;
        protected String _pipeName;

        /// <summary>
        ///     获取或设置IPC名称
        /// </summary>
        public String PipeName
        {
            get { return _pipeName; }
            set { _pipeName = value; }
        }

        /// <summary>
        ///     获取或设置机器名
        /// </summary>
        public String MachineName
        {
            get { return _machineName; }
            set { _machineName = value; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        ///     资源地址标示类，提供了相关的基本操作。
        /// </summary>
        public PipeUri() : base("")
        { }

        /// <summary>
        ///     资源地址标示类，提供了相关的基本操作。
        /// </summary>
        /// <param name="url" type="string">
        ///     <para>
        ///         完整的URL地址
        ///     </para>
        /// </param>
        public PipeUri(String url) : base(url)
        {
            int offset = url.LastIndexOf('/');
            _serverUri = url.Substring(offset, url.Length - offset);
        }

        #endregion

        #region 父类方法

        /// <summary>
        ///     获取服务器内部使用的Uri形态
        /// </summary>
        /// <returns>返回Uri</returns>
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
                    throw new System.Exception("非法的Pipe资源地址标示。");
                }
                String[] inners = Address.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                if (inners.Length <= 1 || inners.Length > 2)
                {
                    throw new System.Exception("非法的Pipe资源地址标示。");
                }
                _machineName = inners[0];
                _pipeName = inners[1];
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex, DebugGrade.High, Logs.Name);
                throw new System.Exception("非法的Pipe资源地址标示。");
            }
        }

        #endregion
    }
}