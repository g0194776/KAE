using System;
using KJFramework.IO.Helper;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers.Steps
{
    /// <summary>
    ///     ��ѹ��RAR�ļ�������
    /// </summary>
    public class DecompressRarDeployStep : DeployStep
    {
        #region Overrides of DeployStep

        /// <summary>
        ///     ִ�в���
        /// </summary>
        /// <param name="context">������</param>
        /// <param name="args">��ز���</param>
        /// <returns>����ִ�еĽ��</returns>
        protected override bool InnerExecute(out Object[] context, params object[] args)
        {
            if (args == null || args.Length < 3)
            {
                throw new ArgumentException("Wrong arguments.");
            }
            string deCompressPath = (string)args[0];
            string targetPath = (string)args[1];
            if (string.IsNullOrEmpty(deCompressPath) || string.IsNullOrEmpty(targetPath))
            {
                throw new ArgumentException("Can not get a path. (Decompress path/Target Path)");
            }
            _reporter.Notify("#Starting decompress this rar file......");
            bool result = FileHelper.DecompressRAR(deCompressPath, targetPath);
            _reporter.Notify(result ? "#Decompress file successed." : "#Decompress file failed.");
            context = new[] { result, deCompressPath, args[2] };
            return result;
        }

        #endregion
    }
}