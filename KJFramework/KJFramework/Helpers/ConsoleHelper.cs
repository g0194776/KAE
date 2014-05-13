using System;
using KJFramework.Core.Native;
using KJFramework.Enums;

namespace KJFramework.Helpers
{
    /// <summary>
    ///     控制台帮助器，提供了相关的基本操作。
    /// </summary>
    public static class ConsoleHelper
    {
        private static Object _lockObj = new Object();
        private static bool _isConsole;
        private static bool _isInitializeChkConsole;

        /// <summary>
        ///     获取一个值，该值标示了当前运行的环境是否是CUI
        ///     <para>* 注: 该值是有缓存的</para>
        /// </summary>
        public static bool IsConsole
        {
            get
            {
                if (!_isInitializeChkConsole)
                {
#if !MONO
                    _isConsole = !Native.Win32API.IsGUIThread(false);
#else
                    _isConsole = true;
#endif
                    _isInitializeChkConsole = true;
                }
                return _isConsole;
            }
        }


        /// <summary>
        ///     向控制台输出指定内容
        /// </summary>
        /// <param name="context">输出内容</param>
        public static void Print(String context)
        {
            Print(context, ConsoleColor.White);
        }

        /// <summary>
        ///     向控制台输出具有指定前景色的内容
        /// </summary>
        /// <param name="context">输出内容</param>
        /// <param name="consoleColor">前景色</param>
        public static void Print(String context, ConsoleColor consoleColor)
        {
            Print(context, consoleColor, ConsoleColor.Gray, true);
        }

        /// <summary>
        ///     向控制台输出具有指定前景色的内容，并可以还原控制台输出前景色
        /// </summary>
        /// <param name="context">输出内容</param>
        /// <param name="nowConsoleColor">前景色</param>
        /// <param name="orgConsoleColor">要还原的前景色</param>
        /// <param name="autoRest">还原标志位</param>
        public static void Print(String context, ConsoleColor nowConsoleColor, ConsoleColor orgConsoleColor, bool autoRest)
        {
            lock (_lockObj)
            {
                Console.ForegroundColor = nowConsoleColor;
                Console.Write(context);
                if (autoRest)
                {
                    Console.ForegroundColor = orgConsoleColor;
                }
            }
        }


        /// <summary>
        ///     向控制台输出具有指定前景色的内容，并可以还原控制台输出前景色
        /// </summary>
        /// <param name="context">输出内容</param>
        /// <param name="nowConsoleColor">前景色</param>
        /// <param name="orgConsoleColor">要还原的前景色</param>
        public static void Print(String context, ConsoleColor nowConsoleColor, ConsoleColor orgConsoleColor)
        {
            Print(context, nowConsoleColor, orgConsoleColor, true);
        }

        /// <summary>
        ///     向控制台输出一个换行
        /// </summary>
        public static void PrintLine()
        {
            PrintLine("");
        }

        /// <summary>
        ///     向控制台输出指定内容，并换行
        /// </summary>
        /// <param name="context">输出内容</param>
        public static void PrintLine(String context)
        {
            PrintLine(context, ConsoleColor.White);
        }

        /// <summary>
        ///     向控制台输出具有指定前景色的内容，并换行
        /// </summary>
        /// <param name="context">输出内容</param>
        /// <param name="consoleColor">前景色</param>
        public static void PrintLine(String context, ConsoleColor consoleColor)
        {
            PrintLine(context, consoleColor, ConsoleColor.Gray, true);
        }

        /// <summary>
        ///     向控制台输出具有指定前景色的内容，可以还原控制台输出前景色，并换行
        /// </summary>
        /// <param name="context">输出内容</param>
        /// <param name="nowConsoleColor">前景色</param>
        /// <param name="orgConsoleColor">要还原的前景色</param>
        /// <param name="autoRest">还原标志位</param>
        public static void PrintLine(String context, ConsoleColor nowConsoleColor, ConsoleColor orgConsoleColor, bool autoRest)
        {
            lock (_lockObj)
            {
                Console.ForegroundColor = nowConsoleColor;
                Console.WriteLine(context);
                if (autoRest)
                {
                    Console.ForegroundColor = orgConsoleColor;
                }
            }
        }

        /// <summary>
        ///     重置控制台前景色
        /// </summary>
        /// <param name="consoleColor">前景色</param>
        public static void ResetConsoleColor(ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
        }

        /// <summary>
        ///     准备显示进度
        /// </summary>
        /// <param name="prepareString">准备名称</param>
        public static void PrepareShowProcess(String prepareString)
        {
            Print(prepareString, ConsoleColor.White);
        }

        /// <summary>
        ///     准备显示进度
        /// </summary>
        /// <param name="prepareString">准备名称</param>
        /// <param name="consoleColor">前景色</param>
        public static void PrepareShowProcess(String prepareString, ConsoleColor consoleColor)
        {
            Print(prepareString, consoleColor);
        }

        /// <summary>
        ///     显示进度数字
        /// </summary>
        /// <param name="processValue">进度值</param>
        /// <param name="showProcessType">显示进度类型</param>
        /// <param name="consoleColor">前景色</param>
        public static void ShowProcess(int processValue, ShowProcessTypes showProcessType, ConsoleColor consoleColor)
        {
            if (processValue > 100 || processValue < 0)
            {
                throw new System.Exception("要输出的进度值不正确。");
            }
            if (showProcessType == ShowProcessTypes.First)
            {
                if (processValue < 10)
                {
                    Print(processValue + "%", consoleColor);
                }
                else
                {
                    Print(processValue + "%", consoleColor);
                }
            }
            else
            {
                if (processValue < 10)
                {
                    Console.Write("\b\b");
                    Print(processValue + "%", consoleColor);
                }
                else
                {
                    Console.Write("\b\b\b");
                    Print(processValue + "%", consoleColor);
                }
            }
        }

        /// <summary>
        ///     结束显示进度
        /// </summary>
        public static void EndShowProcess()
        {
            Console.WriteLine();
        }

        /// <summary>
        ///     等待输入
        /// </summary>
        /// <returns>返回用户的输入</returns>
        public static String Wait()
        {
            return Console.ReadLine();
        }

        /// <summary>
        ///     输出空行
        /// </summary>
        /// <param name="count">空行数目</param>
        public static void PrintNewLine(int count)
        {
            if (count < 0)
            {
                throw new System.Exception("要打印的行数不正确。");
            }
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine("");
            }
        }

        /// <summary>
        ///     输出等待命令提示信息，直到用户输入了规定的结束符号
        /// </summary>
        /// <param name="notifyText">提示信息</param>
        /// <param name="endText">结束符号</param>
        /// <param name="result">用户键入的结果 [out]</param>
        public static void Wait(String notifyText, String endText, out String result)
        {
            Print(notifyText);
            result = Wait();
            while (endText.ToLower() != result.ToLower())
            {
                Wait(notifyText, endText, out result);
            }
        }
    }
}
