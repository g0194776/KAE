using System;

namespace KJFramework.EventArgs
{
    public delegate void DELEGATE_SERVERHREAT(Object sender, ServerHreatEventArgs e);
    /// <summary>
    ///     服务器心跳事件
    /// </summary>
    public class ServerHreatEventArgs : System.EventArgs
    {
        private int _serverid;
        /// <summary>
        ///     服务器编号
        /// </summary>\
        public int ServerID
        {
            get { return _serverid; }
        }

        /// <summary>
        ///     服务器心跳事件
        /// </summary>
        /// <param name="ServerID" type="int">
        ///     <para>
        ///         服务器编号
        ///     </para>
        /// </param>
        public ServerHreatEventArgs(int ServerID)
        {
            _serverid = ServerID;
        }   
    }
}
