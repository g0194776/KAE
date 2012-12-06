using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     插件目录未找到异常
    /// </summary>
    /// <remarks>
    ///     当读取的插件目录不存在时，触发此异常
    /// </remarks>
    public class PluginPathNotFoundException : System.Exception
    {
        public PluginPathNotFoundException() : base("当前读取的插件目录没有找到 !")
        {
        }
    }
}
