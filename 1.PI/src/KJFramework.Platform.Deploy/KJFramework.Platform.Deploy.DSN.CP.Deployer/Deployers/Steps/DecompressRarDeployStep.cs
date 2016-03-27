using System;
using KJFramework.IO.Helper;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers.Steps
{
    /// <summary>
    ///     解压缩RAR文件部署步骤
    /// </summary>
    public class DecompressRarDeployStep : DeployStep
    {
        #region Overrides of DeployStep

        /// <summary>
        ///     执行步骤
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="args">相关参数</param>
        /// <returns>返回执行的结果</returns>
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