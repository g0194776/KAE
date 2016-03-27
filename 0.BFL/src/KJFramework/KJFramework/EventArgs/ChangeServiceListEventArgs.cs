using System;
using System.Collections;

namespace KJFramework.EventArgs
{
    public delegate void DELEGATE_SERVICE_CHANGE(Object sender, ChangeServiceListEventArgs e);

    /// <summary>
    ///     更换服务列表事件
    /// </summary>
    public class ChangeServiceListEventArgs : System.EventArgs
    {
        private int _serverid;
        /// <summary>
        ///     服务器编号
        /// </summary>
        public int ServerID
        {
            get { return _serverid; }
        }

        private ArrayList _newservicelist;
        /// <summary>
        ///     更换的新服务ID列表
        /// </summary>
        public ArrayList NewServiceList
        {
            get { return _newservicelist; }
        }

        /// <summary>
        ///     更换服务列表事件
        /// </summary>
        /// <param name="ServerID" type="int">
        ///     <para>
        ///         服务器编号
        ///     </para>
        /// </param>
        /// <param name="NewServiceList" type="System.Collections.ArrayList">
        ///     <para>
        ///         更换的新服务ID列表
        ///     </para>
        /// </param>
        public ChangeServiceListEventArgs(int ServerID, ArrayList NewServiceList)
        {
            _serverid = ServerID;
            _newservicelist = NewServiceList;
        }

    }
}
