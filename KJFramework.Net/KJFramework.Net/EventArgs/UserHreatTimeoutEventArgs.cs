using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.EventArgs
{
    public delegate void DELEGATE_USERHREAT_TIMEOUT<I>(Object sender, UserHreatTimeoutEventArgs<I> e);
    /// <summary>
    ///     用户心跳超时事件
    /// </summary>
    /// <remarks>
    ///     当用户未心跳事件超过了预定的间隔后将会触发该事件。
    /// </remarks>
    public class UserHreatTimeoutEventArgs<I> : System.EventArgs
    {
        private I _timeoutuser;
        /// <summary>
        ///     超时的用户
        /// </summary>
        public I TimeoutUser
        {
            get { return _timeoutuser; }
        }

        /// <summary>
        ///     用户心跳超时事件
        /// </summary>
        /// <param name="TimeoutUser" type="T">
        ///     <para>
        ///         超时的用户
        ///     </para>
        /// </param>
        /// <remarks>
        ///     当用户未心跳事件超过了预定的间隔后将会触发该事件。
        /// </remarks>
        public UserHreatTimeoutEventArgs(I TimeoutUser)
        {
            _timeoutuser = TimeoutUser;
        }
    }
}
