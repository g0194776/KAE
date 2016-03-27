using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     被抛弃的数目上限设置不正确 
    /// </summary>
    public class DisclareIndexUnCorrectException : System.Exception
    {
        /// <summary>
        ///     被抛弃的数目上限设置不正确 
        /// </summary>
        public DisclareIndexUnCorrectException() : base("被抛弃的数目上限设置不正确 ！")
        {
        }
    }
}
