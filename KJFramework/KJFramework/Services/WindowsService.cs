using System;
using KJFramework.Core.Native;
#if !MONO
namespace KJFramework.Services
{
    /// <summary>
    ///     WINDOWS服务，提供了相关的基本操作
    /// </summary>
    public class WindowsService : IWindowsService
    {
        #region 成员

        protected string _serviceName;
        protected string _displayName;
        protected string _filePath;
        protected string _directoryPath;
        protected readonly IntPtr _serviceHandle;

        #endregion

        #region 构造函数

        /// <summary>
        ///     WINDOWS服务，提供了相关的基本操作
        /// </summary>
        /// <param name="serviceHandle">服务句柄</param>
        public WindowsService(IntPtr serviceHandle)
        {
            if (serviceHandle == IntPtr.Zero)
            {
                throw new System.Exception("无效的系统服务句柄。");
            }
            _serviceHandle = serviceHandle;
        }

        /// <summary>
        ///     WINDOWS服务，提供了相关的基本操作
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public WindowsService(String serviceName)
        {
            if (String.IsNullOrEmpty(serviceName))
            {
                throw new ArgumentNullException("serviceName");
            }
            IntPtr scIntptr = Native.Win32API.OpenSCManager(null, null, Native.Win32API.SC_MANAGER_CREATE_SERVICE);
            if (scIntptr == IntPtr.Zero)
            {
                Native.Win32API.CloseServiceHandle(scIntptr);
                throw new System.Exception("无法打开服务面板 !");
            }
            IntPtr svcIntptr = Native.Win32API.OpenService(scIntptr, serviceName, Native.Win32API.SERVICE_ALL_ACCESS);
            Native.Win32API.CloseServiceHandle(scIntptr);
            if (svcIntptr == IntPtr.Zero)
            {
                Native.Win32API.CloseServiceHandle(svcIntptr);
                throw new System.Exception("无法打开指定服务 !");
            }
            _serviceHandle = svcIntptr;
        }

        #endregion

        #region Implementation of IDisposable
        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IWindowsService

        /// <summary>
        ///     开启服务
        /// </summary>
        /// <returns>返回开启的状态</returns>
        public bool Start()
        {
            if (Native.Win32API.StartService(_serviceHandle, 0, null) == 0)
            {
                Native.Win32API.CloseServiceHandle(_serviceHandle);
                return false;
            }
            return true;
        }

        /// <summary>
        ///     停止服务
        /// </summary>
        /// <returns>返回停止的状态</returns>
        public bool Stop()
        {
            if (_serviceHandle != IntPtr.Zero)
            {
                Native.Win32API.CloseServiceHandle(_serviceHandle);
            }
            return true;
        }

        /// <summary>
        ///     获取或设置服务名称
        /// </summary>
        public string ServiceName
        {
            get { return _serviceName; }
            set { _serviceName = value; }
        }

        /// <summary>
        ///     获取或设置显示名称
        /// </summary>
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        /// <summary>
        ///     获取或设置服务可执行文件路径
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        /// <summary>
        ///     获取或设置服务可执行文件目录
        /// </summary>
        public string DirectoryPath
        {
            get { return _directoryPath; }
            set { _directoryPath = value; }
        }

        #endregion
    }
}
#endif