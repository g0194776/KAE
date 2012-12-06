using System;
using System.IO;
using KJFramework.Logger;
using KJFramework.Platform.Deploy.DSN.Common.Configurations;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Packages;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers.Steps
{
    /// <summary>
    ///     ����������ļ�������
    /// </summary>
    public class SaveBinaryFileDeployStep : DeployStep
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
            if (args == null || args.Length != 1)
            {
                throw new ArgumentException("Wrong arguments.");
            }
            IFilePackage package = (IFilePackage) args[0];
            byte[] data = package.GetData();
            _reporter.Notify("#Pickup file package binary data......");
            if (data == null)
            {
                Logs.Logger.Log("Can not get binary data form a file package. #token: " + package.RequestToken);
                context = null;
                return false;
            }
            string savePath = DSNSettingConfigSection.Current.Settings.SaveDir + package.RequestToken + ".rar";
            _reporter.Notify("#Begin combin save path......");
            if (File.Exists(savePath))
            {
                Logs.Logger.Log("Try to delete file, beacuse the target file has already existed. #path: " + savePath);
                File.Delete(savePath);
            }
            if (!Directory.Exists(DSNSettingConfigSection.Current.Settings.SaveDir))
            {
                Directory.CreateDirectory(DSNSettingConfigSection.Current.Settings.SaveDir);
            }
            _reporter.Notify("#Prepare to saving file......");
            using (FileStream fileStream = new FileStream(savePath, FileMode.CreateNew))
            {
                fileStream.Write(data, 0, data.Length);
                fileStream.Flush();
            }
            _reporter.Notify("#Save file successed!");
            context = new Object[] { DSNSettingConfigSection.Current.Settings.ManageDir + package.ServiceName + "\\", savePath, package };
            return true;
        }

        #endregion
    }
}