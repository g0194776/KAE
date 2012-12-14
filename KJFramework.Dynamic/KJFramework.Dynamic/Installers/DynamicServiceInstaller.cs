using KJFramework.Core.Native;
using KJFramework.Dynamic.Structs;
using KJFramework.Installers;
using KJFramework.Services;
using KJFramework.Tracing;
using System;
using System.Runtime.InteropServices;

namespace KJFramework.Dynamic.Installers
{
    /// <summary>
    ///     动态服务安装器，提供了相关的基本操作。
    /// </summary>
    public class DynamicServiceInstaller : IServiceInstaller<DynamicDomainServiceInfo>
    {
        #region Members

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(DynamicServiceInstaller));

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

        #region Implementation of IServiceInstaller<IDynamicDomainService>

        /// <summary>
        ///     安装服务
        /// </summary>
        /// <param name="obj">安装对象</param>
        /// <returns>
        ///     返回安装的状态
        /// </returns>
        public IWindowsService Install(DynamicDomainServiceInfo obj)
        {
            IntPtr scIntptr = Native.Win32API.OpenSCManager(null, null, Native.Win32API.SC_MANAGER_CREATE_SERVICE);
            if (scIntptr == IntPtr.Zero)
            {
                Native.Win32API.CloseServiceHandle(scIntptr);
                _tracing.Warn("无法打开服务控制面板 !");
                return null;
            }
            IntPtr svcIntPtr = Native.Win32API.CreateService(scIntptr, obj.Service.Infomation.ServiceName,
                                                             obj.Service.Infomation.Name,
                                                             Native.Win32API.SERVICE_ALL_ACCESS,
                                                             Native.Win32API.SERVICE_WIN32_OWN_PROCESS,
                                                             Native.Win32API.SERVICE_AUTO_START,
                                                             Native.Win32API.SERVICE_ERROR_NORMAL,
                                                             obj.FilePath, null, 0, null, null, null);
            Native.Win32API.CloseServiceHandle(scIntptr);
            if (svcIntPtr == IntPtr.Zero)
            {
                Native.Win32API.CloseServiceHandle(svcIntPtr);
                _tracing.Warn("创建服务失败 !");
                return null;
            }
            Native.SERVICE_DESCRIPTION serviceDescription = new Native.SERVICE_DESCRIPTION();
            serviceDescription.lpDescription = obj.Service.Infomation.Description;
            //创建结构体指针
            IntPtr pnt = Marshal.AllocHGlobal(Marshal.SizeOf(serviceDescription));
            if (pnt == IntPtr.Zero) throw new System.Exception("不能够申请更多的内存空间。");
            try
            {
                Marshal.StructureToPtr(serviceDescription, pnt, false);
                //设置服务描述
                if (
                    !Native.Win32API.ChangeServiceConfig2(svcIntPtr,
                                                          (int) Native.ServiceInfoLevel.SERVICE_CONFIG_DESCRIPTION, pnt))
                    throw new System.Exception("设置服务描述信息失败。");
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                throw;
            }
            finally
            {
                Marshal.FreeHGlobal(pnt);
            }
            return new WindowsService(svcIntPtr);
        }

        /// <summary>
        ///     安装服务
        /// </summary>
        /// <param name="obj">安装对象</param>
        /// <param name="serviceName">服务名称</param>
        /// <param name="name">服务别名</param>
        /// <param name="description">服务描述</param>
        /// <returns>
        ///     返回安装的状态
        /// </returns>
        public IWindowsService Install(DynamicDomainServiceInfo obj, string serviceName, string name, string description)
        {
            IntPtr scIntptr = Native.Win32API.OpenSCManager(null, null, Native.Win32API.SC_MANAGER_CREATE_SERVICE);
            if (scIntptr == IntPtr.Zero)
            {
                Native.Win32API.CloseServiceHandle(scIntptr);
                _tracing.Warn("无法打开服务控制面板 !");
                return null;
            }
            IntPtr svcIntPtr = Native.Win32API.CreateService(scIntptr, serviceName, name,
                                                             Native.Win32API.SERVICE_ALL_ACCESS,
                                                             Native.Win32API.SERVICE_WIN32_OWN_PROCESS,
                                                             Native.Win32API.SERVICE_AUTO_START,
                                                             Native.Win32API.SERVICE_ERROR_NORMAL,
                                                             obj.FilePath, null, 0, null, null, null);
            Native.Win32API.CloseServiceHandle(scIntptr);
            if (svcIntPtr == IntPtr.Zero)
            {
                Native.Win32API.CloseServiceHandle(svcIntPtr);
                _tracing.Warn("创建服务失败 !");
                return null;
            }
            Native.SERVICE_DESCRIPTION serviceDescription = new Native.SERVICE_DESCRIPTION();
            serviceDescription.lpDescription = description;
            //创建结构体指针
            IntPtr pnt = Marshal.AllocHGlobal(Marshal.SizeOf(serviceDescription));
            if (pnt == IntPtr.Zero) throw new System.Exception("不能够申请更多的内存空间。");
            try
            {
                Marshal.StructureToPtr(serviceDescription, pnt, false);
                //设置服务描述
                if (
                    !Native.Win32API.ChangeServiceConfig2(svcIntPtr,
                                                          (int) Native.ServiceInfoLevel.SERVICE_CONFIG_DESCRIPTION, pnt))
                    throw new System.Exception("设置服务描述信息失败。");
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                throw;
            }
            finally
            {
                Marshal.FreeHGlobal(pnt);
            }
            return new WindowsService(svcIntPtr);
        }

        /// <summary>
        ///     卸载服务
        /// </summary>
        /// <param name="name">服务名</param>
        /// <returns>
        ///     返回写在状态
        /// </returns>
        public bool UnInstall(string name)
        {
            if (String.IsNullOrEmpty(name)) return false;
            IntPtr scIntptr = Native.Win32API.OpenSCManager(null, null, Native.Win32API.SC_MANAGER_CREATE_SERVICE);
            if (scIntptr == IntPtr.Zero)
            {
                Native.Win32API.CloseServiceHandle(scIntptr);
                _tracing.Warn("无法打开服务控制面板 !");
                return false;
            }
            IntPtr svcIntptr = Native.Win32API.OpenService(scIntptr, name, Native.Win32API.SERVICE_ALL_ACCESS);
            Native.Win32API.CloseServiceHandle(scIntptr);
            if (svcIntptr == IntPtr.Zero)
            {
                Native.Win32API.CloseServiceHandle(svcIntptr);
                _tracing.Warn("打开服务失败 !");
                return false;
            }
            bool state = Native.Win32API.DeleteService(svcIntptr) > 0;
            Native.Win32API.CloseServiceHandle(svcIntptr);
            return state;
        }

        #endregion
    }
}