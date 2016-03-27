using System;
using KJFramework.Dynamic.Installers;
using KJFramework.Dynamic.Structs;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Packages;
using KJFramework.Services;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers.Steps
{
    public class CreateWindowServiceDeployStep : DeployStep
    {
        #region Overrides of DeployStep

        /// <summary>
        ///     执行步骤
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="args">相关参数</param>
        /// <returns>返回执行的结果</returns>
        protected override bool InnerExecute(out object[] context, params object[] args)
        {
            if (args == null || args.Length != 3)
            {
                throw new ArgumentException("Wrong arguments.");
            }
            bool deCompressResult = (bool)args[0];
            string targetPath = (string)args[1];
            IFilePackage package = (IFilePackage)args[2];
            //Decompress failed.
            if (!deCompressResult)
            {
                context = null;
                return false;
            }
            DynamicServiceInstaller installer = new DynamicServiceInstaller();
            string filePath = targetPath + "Worker-" + package.ServiceName + ".exe";
            _reporter.Notify("#Begin install windows service: " + package.ServiceName + "......");
            IWindowsService ws = installer.Install(new DynamicDomainServiceInfo
                                                       {
                                                           DirectoryPath = targetPath,
                                                           FilePath = filePath
                                                       }, package.ServiceName, package.Name, package.Description);
            if (ws == null)
            {
                _reporter.Notify("#Install windows service: " + package.ServiceName + " failed!");
                context = null;
                return false;
            }
            _reporter.Notify("#Install windows service: " + package.ServiceName + " successfully!");
            if (!ws.Start())
            {
                _reporter.Notify("#Run windows service: " + package.ServiceName + " failed!");
                context = null;
                return false;
            }
            _reporter.Notify("#Run service: " + package.ServiceName + " successed!");
            context = new object[] { filePath, ws, package };
            return true;
        }

        #endregion
    }
}