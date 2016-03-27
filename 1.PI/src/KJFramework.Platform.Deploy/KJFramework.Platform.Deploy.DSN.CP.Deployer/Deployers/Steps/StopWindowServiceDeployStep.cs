using System;
using System.Diagnostics;
using KJFramework.Services;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers.Steps
{
    /// <summary>
    ///     ֹͣWINDOW SERVICE�Ĳ�����
    /// </summary>
    public class StopWindowServiceDeployStep : DeployStep
    {
        #region Overrides of DeployStep

        /// <summary>
        ///     ִ�в���
        /// </summary>
        /// <param name="context">������</param>
        /// <param name="args">��ز���</param>
        /// <returns>����ִ�еĽ��</returns>
        protected override bool InnerExecute(out object[] context, params object[] args)
        {
            if (args == null || args.Length != 2)
            {
                throw new ArgumentException("Wrong arguments.");
            }
            bool exists = (bool)args[0];
            string serviceName = (string)args[1];
            if (!exists)
            {
                _reporter.Notify("#Can not stop a service, because this service not existed. #name: " + serviceName + ".");
                throw new System.Exception("Can not stop a service, because this service not existed. #name: " + serviceName + ".");
            }
            string processName = "Worker-" + serviceName;
            bool isFind = false;
            bool isKilled = false;
            _reporter.Notify("#Begin killing process #name: " + processName + "......");
            foreach (var process in Process.GetProcesses())
            {
                if (process.ProcessName == processName)
                {
                    isFind = true;
                    process.Kill();
                    isKilled = true;
                    break;
                }
            }
            if (!isFind)
            {
                _reporter.Notify("#Kill process #name: " + processName + " failed, because not found this process.");
            }
            else if (!isKilled)
            {
                _reporter.Notify("#Kill process #name: " + processName + " failed.");
            }
            else
            {
                _reporter.Notify("#Kill process #name: " + processName + " successed!");
            }
            _reporter.Notify("#Begin stoping service #name: " + serviceName + "......");
            IWindowsService service = new WindowsService(serviceName);
            bool result = service.Stop();
            _reporter.Notify(result
                                 ? "#Service #name: " + serviceName + " stoped."
                                 :  "#Service #name: " + serviceName + " stop failed.");
            context = result ? new object[] {true, serviceName} : null;
            return result;
        }

        #endregion
    }
}