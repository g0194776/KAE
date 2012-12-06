using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     要删除的对象为空异常
    /// </summary>
    public class DeleteObjectHasNullException : System.Exception
    {
        /// <summary>
        ///     要删除的对象为空异常
        /// </summary>
        public DeleteObjectHasNullException() : base("要删除的对象不能为null !")
        {
        }
    }
}
