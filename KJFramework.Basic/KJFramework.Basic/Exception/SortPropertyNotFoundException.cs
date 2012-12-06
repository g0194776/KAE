using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Basic.Exception
{
    /// <summary>
    ///     排序字段未找到异常
    /// </summary>
    /// <remarks>
    ///     当要对给定类型进行排序时，如果给定的类型中不存在 int类型的代表字段则要给定排序字段的标示。
    ///     但是当该给定的标示未找到的时候，将会触发此异常。
    /// </remarks>
    public class SortPropertyNotFoundException : System.Exception
    {
        /// <summary>
        ///     排序字段未找到异常
        /// </summary>
        /// <remarks>
        ///     当要对给定类型进行排序时，如果给定的类型中不存在 int类型的代表字段则要给定排序字段的标示。
        ///     但是当该给定的标示未找到的时候，将会触发此异常。
        /// </remarks>
        public SortPropertyNotFoundException() : base("排序的字段在给定的类型中没有被找到 !")
        {
        }
    }
}
