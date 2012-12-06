using System;
using System.Diagnostics;
using KJFramework.Core.Native;

namespace KJFramework.IO.Helper
{
    /// <summary>
    ///     进程帮助器, 提供了相关的基本操作。
    /// </summary>
    public class ProcessHelper
    {
        #region 成员

        /// <summary>
        ///     最小工作集
        /// </summary>
        private static readonly IntPtr MinWorkset = new IntPtr(5);

        #endregion

        /// <summary>
        ///     释放内存
        ///        * 将进程工作集内存最小化操作。
        /// </summary>
        public static void ReleaseMemory()
        {
            Process.GetCurrentProcess().MinWorkingSet = MinWorkset;
        }

        /// <summary>
        ///     释放内存
        ///        * 将进程工作集设为指定值。
        /// </summary>
        /// <param name="min">工作集最大值</param>
        /// <param name="max">工作集最小值</param>
        public static void ReleaseMemory(IntPtr min, IntPtr max)
        {
            Process.GetCurrentProcess().MinWorkingSet = min;
            Process.GetCurrentProcess().MaxWorkingSet = max;
        }

        /// <summary>
        ///     获得当前计算机所有进程
        /// </summary>
        /// <returns>
        ///     返回当前计算机所有进程
        /// </returns>
        public static Process[] GetAllProcessFromComputer()
        {
            return Process.GetProcesses();
        }

        /// <summary>
        ///     获得当前计算机进程数目
        /// </summary>
        /// <returns>
        ///     返回当前计算机进程数目
        /// </returns>
        public static int GetComputerProcessCount()
        {
            return Process.GetProcesses().Length;
        }

        /// <summary>
        ///     结束指定执行进程
        /// </summary>
        /// <param name="thread">执行进程</param>
        /// <returns>返回结束的状态</returns>
        public static bool Kill(ProcessThread thread)
        {
            #if !MONO
            if (thread != null)
            {
                IntPtr ptr = Native.Win32API.OpenThread(1, false, (uint) thread.Id);
                return Native.Win32API.TerminateThread(ptr, 1);
            }
            #endif
            return false;
        }

        /// <summary>
        ///     结束指定进程名称的进程
        /// </summary>
        /// <param name="processName" type="string">
        ///     <para>
        ///         要结束的进程名称
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回结束的状态
        /// </returns>
        public static bool Kill(String processName)
        {
            Process[] tempProcessCollection = Process.GetProcesses();
            bool isKill = false;

            for (int i = 0; i < tempProcessCollection.Length; i++)
            {
                try
                {
                    if (tempProcessCollection[i].ProcessName == processName)
                    {
                        tempProcessCollection[i].Kill();
                        isKill = true;
                    }
                }
                catch(System.Exception e)
                {
                    Debug.WriteLine("杀死进程出现异常 ： " + e.Message);
                }
            }

            return isKill;
        }
    }
}
