using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     移除集合元素异常
    /// </summary>
    /// <remarks>
    ///     当移除集合中的指定元素失败时, 抛出该异常
    /// </remarks>
    public class CollectionMemberRemoveException : System.Exception
    {
        /// <summary>
        ///     移除集合元素异常
        /// </summary>
        /// <remarks>
        ///     当移除集合中的指定元素失败时, 抛出该异常
        /// </remarks>
        public CollectionMemberRemoveException() : base("删除集合中的元素时, 发生了异常!")
        {
        }
    }
}
