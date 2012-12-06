using System;
using System.Runtime.InteropServices;
namespace KJFramework.Core.Native
{
#if !MONO
    /// <summary>
    ///     KJFramework底层调用核心
    ///         * 更多的是对于系统底层API的封装。
    /// </summary>
    public partial class Native
    {
        /// <summary>
        ///     提供了Windows中对于部分API的引用
        /// </summary>
        public class Win32API
        {
            public const uint SHGFI_ICON = 0x100;
            public const uint SHGFI_LARGEICON = 0x0; // 'Large icon
            public const uint SHGFI_SMALLICON = 0x1; // 'Small icon
            public delegate IntPtr HookPro(int nCode, IntPtr wParam, IntPtr lParam);  //创建委托，进行回调(键盘钩子)
            public static int SC_MANAGER_CREATE_SERVICE = 0x0002;
            public static int SERVICE_WIN32_OWN_PROCESS = 0x00000010;
            //int SERVICE_DEMAND_START = 0x00000003;
            public static int SERVICE_ERROR_NORMAL = 0x00000001;
            public static int STANDARD_RIGHTS_REQUIRED = 0xF0000;
            public static int SERVICE_QUERY_CONFIG = 0x0001;
            public static int SERVICE_CHANGE_CONFIG = 0x0002;
            public static int SERVICE_QUERY_STATUS = 0x0004;
            public static int SERVICE_ENUMERATE_DEPENDENTS = 0x0008;
            public static int SERVICE_START = 0x0010;
            public static int SERVICE_STOP = 0x0020;
            public static int SERVICE_PAUSE_CONTINUE = 0x0040;
            public static int SERVICE_INTERROGATE = 0x0080;
            public static int SERVICE_USER_DEFINED_CONTROL = 0x0100;
            public static int GENERIC_READ = -2147483648;
            public static int ERROR_INSUFFICIENT_BUFFER = 122;
            public static int SERVICE_NO_CHANGE = -1;  
            public static int SERVICE_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED |
            SERVICE_QUERY_CONFIG |
            SERVICE_CHANGE_CONFIG |
            SERVICE_QUERY_STATUS |
            SERVICE_ENUMERATE_DEPENDENTS |
            SERVICE_START |
            SERVICE_STOP |
            SERVICE_PAUSE_CONTINUE |
            SERVICE_INTERROGATE |
            SERVICE_USER_DEFINED_CONTROL);
            public static int SERVICE_AUTO_START = 0x00000002;

            [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool ChangeServiceConfig2(IntPtr hService, int dwInfoLevel, IntPtr lpInfo);
            [DllImport("shell32.dll")]
            public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
            [DllImport("shell32.dll")]
            public static extern uint ExtractIconEx(string lpszFile, int nIconIndex, int[] phiconLarge, int[] phiconSmall, uint nIcons);
            [DllImport("user32.dll", EntryPoint = "SendMessageA")]
            public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, string lParam);
            /// <summary>
            ///     安装钩子原型
            /// </summary>
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr SetWindowsHookEx(int hookid, HookPro pfnhook, IntPtr hinst, int threadid);
            /// <summary>
            ///     卸载钩子原型
            /// </summary>
            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            public static extern bool UnhookWindowsHookEx(IntPtr hhook);
            /// <summary>
            ///     回调钩子原型
            /// </summary>
            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            public static extern IntPtr CallNextHookEx(IntPtr hhook, int code, IntPtr wparam, IntPtr lparam);
            /// <summary>
            ///     复制内存原型
            /// </summary>
            [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory")]
            public static extern void CopyMemory(ref KBDLLHOOKSTRUCT Source, IntPtr Destination, int Length);
            /// <summary>
            ///    打开一个进程
            /// </summary>
            [DllImport("kernel32.dll")]
            public static extern IntPtr OpenThread(uint dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
            /// <summary>
            ///    终止进程
            /// </summary>
            [DllImport("kernel32.dll")]
            public static extern bool TerminateThread(IntPtr hThread, uint dwExitCode);
            /// <summary>
            ///     获取桌面句柄
            /// </summary>
            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();
            /// <summary>
            ///     通过桌面句柄得到桌面DC句柄
            /// </summary>
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hwnd);
            /// <summary>
            ///     释放DC
            /// </summary>
            [DllImport("User32.dll")]
            public static extern int ReleaseDC(int hWnd, int hDC);
            [DllImport("GDI32.dll")]
            public static extern bool BitBlt(int hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, int hdcSrc, int nXSrc, int nYSrc, int dwRop);
            [DllImport("GDI32.dll")]
            public static extern int CreateCompatibleBitmap(int hdc, int nWidth, int nHeight);
            [DllImport("GDI32.dll")]
            public static extern int CreateCompatibleDC(int hdc);
            [DllImport("GDI32.dll")]
            public static extern bool DeleteDC(int hdc);
            [DllImport("GDI32.dll")]
            public static extern bool DeleteObject(int hObject);
            [DllImport("GDI32.dll")]
            public static extern int GetDeviceCaps(int hdc, int nIndex);
            [DllImport("GDI32.dll")]
            public static extern int SelectObject(int hdc, int hgdiobj);
            [DllImport("advapi32.dll")]
            public static extern IntPtr OpenSCManager(string lpMachineName, string lpSCDB, int scParameter);
            [DllImport("Advapi32.dll")]
            public static extern IntPtr CreateService(IntPtr SC_HANDLE, string lpSvcName, string lpDisplayName,
            int dwDesiredAccess, int dwServiceType, int dwStartType, int dwErrorControl, string lpPathName,
            string lpLoadOrderGroup, int lpdwTagId, string lpDependencies, string lpServiceStartName, string lpPassword);
            [DllImport("kernel32.dll")]
            public static extern bool CreateProcess(string lpApplicationName,
                                                     string lpCommandLine, ref SECURITY_ATTRIBUTES lpProcessAttributes,
                                                     ref SECURITY_ATTRIBUTES lpThreadAttributes, bool bInheritHandles,
                                                     uint dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory,
                                                     [In] ref STARTUPINFO lpStartupInfo,
                                                     out PROCESS_INFORMATION lpProcessInformation);
            [DllImport("advapi32.dll")]
            public static extern void CloseServiceHandle(IntPtr SCHANDLE);
            [DllImport("advapi32.dll")]
            public static extern int StartService(IntPtr SVHANDLE, int dwNumServiceArgs, string lpServiceArgVectors);
            [DllImport("advapi32.dll", SetLastError = true)]
            public static extern IntPtr OpenService(IntPtr SCHANDLE, string lpSvcName, int dwNumServiceArgs);
            [DllImport("advapi32.dll")]
            public static extern int DeleteService(IntPtr SVHANDLE);
            [DllImport("kernel32.dll")]
            public static extern int GetLastError();
            /// <summary>
            ///   SetThreadAffinityMask 指定hThread 运行在 核心 dwThreadAffinityMask
            /// </summary>
            [DllImport("kernel32.dll")]
            public static extern UIntPtr SetThreadAffinityMask(IntPtr hThread, UIntPtr dwThreadAffinityMask);
            /// <summary>
            ///   得到当前线程的handler  
            /// </summary>
            [DllImport("kernel32.dll")] 
            public static extern IntPtr GetCurrentThread();
            [DllImport("user32.dll")]
            public static extern bool IsGUIThread(bool bConvert);
            /// <summary>
            ///     内存COPY函数
            /// </summary>
            /// <param name="dest">目标地址</param>
            /// <param name="src">源地址</param>
            /// <param name="count">COPY长度</param>
            /// <returns>返回COPY后的目标地址</returns>
            [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
            public static extern IntPtr memcpy(IntPtr dest, IntPtr src, uint count);

            //函数中的参数 dwThreadAffinityMask 为无符号长整型，用位标识那个核心100
            //比如：为简洁使用四位表示101         
            //0x0001表示核心1，102         
            //0x0010表示核心2，103         
            //0x0100表示核心3，104         
            //0x1000表示核心4105         
            public static ulong SetCpuID(int id)
            {
                ulong cpuid = 0;
                if (id < 0 || id >= Environment.ProcessorCount)
                {
                    id = 0;
                }
                cpuid |= 1UL << id;
                return cpuid;
            }

        }

        public enum ServiceInfoLevel : int
        {
            SERVICE_CONFIG_DESCRIPTION = 1,
            SERVICE_CONFIG_FAILURE_ACTIONS = 2,
            SERVICE_CONFIG_DELAYED_AUTO_START_INFO = 3,
            SERVICE_CONFIG_FAILURE_ACTIONS_FLAG = 4,
            SERVICE_CONFIG_SERVICE_SID_INFO = 5,
            SERVICE_CONFIG_REQUIRED_PRIVILEGES_INFO = 6,
            SERVICE_CONFIG_PRESHUTDOWN_INFO = 7
        }

        public enum ServiceStateRequest : int
        {
            SERVICE_ACTIVE = 0x1,
            SERVICE_INACTIVE = 0x2,
            SERVICE_STATE_ALL = (SERVICE_ACTIVE + SERVICE_INACTIVE)
        }

        public enum ServiceControlType : int
        {
            SERVICE_CONTROL_STOP = 0x1,
            SERVICE_CONTROL_PAUSE = 0x2,
            SERVICE_CONTROL_CONTINUE = 0x3,
            SERVICE_CONTROL_INTERROGATE = 0x4,
            SERVICE_CONTROL_SHUTDOWN = 0x5,
            SERVICE_CONTROL_PARAMCHANGE = 0x6,
            SERVICE_CONTROL_NETBINDADD = 0x7,
            SERVICE_CONTROL_NETBINDREMOVE = 0x8,
            SERVICE_CONTROL_NETBINDENABLE = 0x9,
            SERVICE_CONTROL_NETBINDDISABLE = 0xA,
            SERVICE_CONTROL_DEVICEEVENT = 0xB,
            SERVICE_CONTROL_HARDWAREPROFILECHANGE = 0xC,
            SERVICE_CONTROL_POWEREVENT = 0xD,
            SERVICE_CONTROL_SESSIONCHANGE = 0xE,
        }
        public enum ServiceState : int
        {
            SERVICE_STOPPED = 0x1,
            SERVICE_START_PENDING = 0x2,
            SERVICE_STOP_PENDING = 0x3,
            SERVICE_RUNNING = 0x4,
            SERVICE_CONTINUE_PENDING = 0x5,
            SERVICE_PAUSE_PENDING = 0x6,
            SERVICE_PAUSED = 0x7,
        }
        public enum ServiceControlAccepted : int
        {
            SERVICE_ACCEPT_STOP = 0x1,
            SERVICE_ACCEPT_PAUSE_CONTINUE = 0x2,
            SERVICE_ACCEPT_SHUTDOWN = 0x4,
            SERVICE_ACCEPT_PARAMCHANGE = 0x8,
            SERVICE_ACCEPT_NETBINDCHANGE = 0x10,
            SERVICE_ACCEPT_HARDWAREPROFILECHANGE = 0x20,
            SERVICE_ACCEPT_POWEREVENT = 0x40,
            SERVICE_ACCEPT_SESSIONCHANGE = 0x80
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct SERVICE_STATUS
        {
            public int dwServiceType;
            public int dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct QUERY_SERVICE_CONFIG
        {
            public int dwServiceType;
            public int dwStartType;
            public int dwErrorControl;
            public string lpBinaryPathName;
            public string lpLoadOrderGroup;
            public int dwTagId;
            public string lpDependencies;
            public string lpServiceStartName;
            public string lpDisplayName;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        public enum SC_ACTION_TYPE : int
        {
            SC_ACTION_NONE = 0,
            SC_ACTION_RESTART = 1,
            SC_ACTION_REBOOT = 2,
            SC_ACTION_RUN_COMMAND = 3,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SC_ACTION
        {
            public SC_ACTION_TYPE SCActionType;
            public int Delay;
        }

        public enum InfoLevel : int
        {
            SERVICE_CONFIG_DESCRIPTION = 1,
            SERVICE_CONFIG_FAILURE_ACTIONS = 2
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SERVICE_DESCRIPTION
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpDescription;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SERVICE_FAILURE_ACTIONS
        {
            public int dwResetPeriod;
            public string lpRebootMsg;
            public string lpCommand;
            public int cActions;
            public int lpsaActions;
        }

        //声明一个Point的封送类型   
        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;
            public int y;
        }

        //声明鼠标钩子的封送结构类型   
        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStruct
        {
            public POINT pt;
            public int hWnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }


        //键盘Hook结构函数
        [StructLayout(LayoutKind.Sequential)]
        public struct KBDLLHOOKSTRUCT
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct STARTUPINFO
        {
            public Int32 cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public Int32 dwX;
            public Int32 dwY;
            public Int32 dwXSize;
            public Int32 dwYSize;
            public Int32 dwXCountChars;
            public Int32 dwYCountChars;
            public Int32 dwFillAttribute;
            public Int32 dwFlags;
            public Int16 wShowWindow;
            public Int16 cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            public int nLength;
            public unsafe byte* lpSecurityDescriptor;
            public int bInheritHandle;
        }
    }

#endif
}
