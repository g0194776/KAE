using System;

namespace KJFramework.EventArgs
{
    public delegate void DELEGATE_DELETE_SERVER(Object sender, DeleteServerEventArgs e) ;
    /// <summary>
    ///     删除服务器连接对象事件
    /// </summary>
    public class DeleteServerEventArgs: System.EventArgs
    {
        private int _serverid;
        /// <summary>
        ///     服务器编号
        /// </summary>
        public int ServerID
        {
            get { return _serverid; }
        }

        /// <summary>
        ///      删除服务器连接对象事件
        /// </summary>
        /// <param name="ServerID" type="int">
        ///     <para>
        ///         服务器编号
        ///     </para>
        /// </param>
        public DeleteServerEventArgs(int ServerID)
        {
            _serverid = ServerID;
        }
    }
}
