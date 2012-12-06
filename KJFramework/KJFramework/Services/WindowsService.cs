using System;
using KJFramework.Core.Native;
#if !MONO
namespace KJFramework.Services
{
    /// <summary>
    ///     WINDOWS�����ṩ����صĻ�������
    /// </summary>
    public class WindowsService : IWindowsService
    {
        #region ��Ա

        protected string _serviceName;
        protected string _displayName;
        protected string _filePath;
        protected string _directoryPath;
        protected readonly IntPtr _serviceHandle;

        #endregion

        #region ���캯��

        /// <summary>
        ///     WINDOWS�����ṩ����صĻ�������
        /// </summary>
        /// <param name="serviceHandle">������</param>
        public WindowsService(IntPtr serviceHandle)
        {
            if (serviceHandle == IntPtr.Zero)
            {
                throw new System.Exception("��Ч��ϵͳ��������");
            }
            _serviceHandle = serviceHandle;
        }

        /// <summary>
        ///     WINDOWS�����ṩ����صĻ�������
        /// </summary>
        /// <param name="serviceName">��������</param>
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
                throw new System.Exception("�޷��򿪷������ !");
            }
            IntPtr svcIntptr = Native.Win32API.OpenService(scIntptr, serviceName, Native.Win32API.SERVICE_ALL_ACCESS);
            Native.Win32API.CloseServiceHandle(scIntptr);
            if (svcIntptr == IntPtr.Zero)
            {
                Native.Win32API.CloseServiceHandle(svcIntptr);
                throw new System.Exception("�޷���ָ������ !");
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
        ///     ��������
        /// </summary>
        /// <returns>���ؿ�����״̬</returns>
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
        ///     ֹͣ����
        /// </summary>
        /// <returns>����ֹͣ��״̬</returns>
        public bool Stop()
        {
            if (_serviceHandle != IntPtr.Zero)
            {
                Native.Win32API.CloseServiceHandle(_serviceHandle);
            }
            return true;
        }

        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        public string ServiceName
        {
            get { return _serviceName; }
            set { _serviceName = value; }
        }

        /// <summary>
        ///     ��ȡ��������ʾ����
        /// </summary>
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        /// <summary>
        ///     ��ȡ�����÷����ִ���ļ�·��
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        /// <summary>
        ///     ��ȡ�����÷����ִ���ļ�Ŀ¼
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