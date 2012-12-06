using System;
using System.Diagnostics;
using System.IO;

namespace KJFramework.IO.Helper
{
    /// <summary>
    ///     命令帮助器, 提供了相关的基本操作。
    /// </summary>
    public class CommandHelper
    {
        /// <summary>
        ///     执行指定DOS命令
        /// </summary>
        /// <param name="command" type="string">
        ///     <para>
        ///         要执行的DOS命令
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回执行结果
        /// </returns>
        public static StreamReader ExecuteCommand(String command)
        {
            using(Process commandProcess = new Process())
            {
                try
                {
                    commandProcess.StartInfo.FileName = "cmd.exe";
                    commandProcess.StartInfo.Arguments = "/c " + command;
                    commandProcess.StartInfo.UseShellExecute = false;
                    commandProcess.StartInfo.RedirectStandardInput = true;
                    commandProcess.StartInfo.CreateNoWindow = true;
                    commandProcess.StartInfo.RedirectStandardError = true;
                    commandProcess.StartInfo.RedirectStandardOutput = true;
                    commandProcess.Start();
                    return commandProcess.StandardOutput;
                    
                }
                catch (System.Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        ///     打开指定路径文件
        /// </summary>
        /// <param name="filePath" type="string">
        ///     <para>
        ///         要打开的文件路径
        ///     </para>
        /// </param>
        public static void OpenFile(String filePath)
        {
            Process.Start(filePath);
        }
    }
}
