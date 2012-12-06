using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     检查器内部对象集合没有被找到异常
    /// </summary>
    /// <remarks>
    ///     当检查器内部对象集合 == null时，触发该异常。
    /// </remarks>
    public class CheckerMemberNotFoundException : System.Exception
    {
        /// <summary>
        ///     检查器内部对象集合没有被找到异常
        /// </summary>
        /// <remarks>
        ///     当检查器内部对象集合 == null时，触发该异常。
        /// </remarks>
        public CheckerMemberNotFoundException() : base("检查器内部对象集合没有被找到 !")
        {
        }
    }
}
