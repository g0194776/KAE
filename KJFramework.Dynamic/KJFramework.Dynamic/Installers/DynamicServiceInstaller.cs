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
    ///     ��̬����װ�����ṩ����صĻ���������
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
        ///     ��װ����
        /// </summary>
        /// <param name="obj">��װ����</param>
        /// <returns>
        ///     ���ذ�װ��״̬
        /// </returns>
        public IWindowsService Install(DynamicDomainServiceInfo obj)
        {
            IntPtr scIntptr = Native.Win32API.OpenSCManager(null, null, Native.Win32API.SC_MANAGER_CREATE_SERVICE);
            if (scIntptr == IntPtr.Zero)
            {
                Native.Win32API.CloseServiceHandle(scIntptr);
                _tracing.Warn("�޷��򿪷��������� !");
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
                _tracing.Warn("��������ʧ�� !");
                return null;
            }
            Native.SERVICE_DESCRIPTION serviceDescription = new Native.SERVICE_DESCRIPTION();
            serviceDescription.lpDescription = obj.Service.Infomation.Description;
            //�����ṹ��ָ��
            IntPtr pnt = Marshal.AllocHGlobal(Marshal.SizeOf(serviceDescription));
            if (pnt == IntPtr.Zero) throw new System.Exception("���ܹ����������ڴ�ռ䡣");
            try
            {
                Marshal.StructureToPtr(serviceDescription, pnt, false);
                //���÷�������
                if (
                    !Native.Win32API.ChangeServiceConfig2(svcIntPtr,
                                                          (int) Native.ServiceInfoLevel.SERVICE_CONFIG_DESCRIPTION, pnt))
                    throw new System.Exception("���÷���������Ϣʧ�ܡ�");
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
        ///     ��װ����
        /// </summary>
        /// <param name="obj">��װ����</param>
        /// <param name="serviceName">��������</param>
        /// <param name="name">�������</param>
        /// <param name="description">��������</param>
        /// <returns>
        ///     ���ذ�װ��״̬
        /// </returns>
        public IWindowsService Install(DynamicDomainServiceInfo obj, string serviceName, string name, string description)
        {
            IntPtr scIntptr = Native.Win32API.OpenSCManager(null, null, Native.Win32API.SC_MANAGER_CREATE_SERVICE);
            if (scIntptr == IntPtr.Zero)
            {
                Native.Win32API.CloseServiceHandle(scIntptr);
                _tracing.Warn("�޷��򿪷��������� !");
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
                _tracing.Warn("��������ʧ�� !");
                return null;
            }
            Native.SERVICE_DESCRIPTION serviceDescription = new Native.SERVICE_DESCRIPTION();
            serviceDescription.lpDescription = description;
            //�����ṹ��ָ��
            IntPtr pnt = Marshal.AllocHGlobal(Marshal.SizeOf(serviceDescription));
            if (pnt == IntPtr.Zero) throw new System.Exception("���ܹ����������ڴ�ռ䡣");
            try
            {
                Marshal.StructureToPtr(serviceDescription, pnt, false);
                //���÷�������
                if (
                    !Native.Win32API.ChangeServiceConfig2(svcIntPtr,
                                                          (int) Native.ServiceInfoLevel.SERVICE_CONFIG_DESCRIPTION, pnt))
                    throw new System.Exception("���÷���������Ϣʧ�ܡ�");
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
        ///     ж�ط���
        /// </summary>
        /// <param name="name">������</param>
        /// <returns>
        ///     ����д��״̬
        /// </returns>
        public bool UnInstall(string name)
        {
            if (String.IsNullOrEmpty(name)) return false;
            IntPtr scIntptr = Native.Win32API.OpenSCManager(null, null, Native.Win32API.SC_MANAGER_CREATE_SERVICE);
            if (scIntptr == IntPtr.Zero)
            {
                Native.Win32API.CloseServiceHandle(scIntptr);
                _tracing.Warn("�޷��򿪷��������� !");
                return false;
            }
            IntPtr svcIntptr = Native.Win32API.OpenService(scIntptr, name, Native.Win32API.SERVICE_ALL_ACCESS);
            Native.Win32API.CloseServiceHandle(scIntptr);
            if (svcIntptr == IntPtr.Zero)
            {
                Native.Win32API.CloseServiceHandle(svcIntptr);
                _tracing.Warn("�򿪷���ʧ�� !");
                return false;
            }
            bool state = Native.Win32API.DeleteService(svcIntptr) > 0;
            Native.Win32API.CloseServiceHandle(svcIntptr);
            return state;
        }

        #endregion
    }
}