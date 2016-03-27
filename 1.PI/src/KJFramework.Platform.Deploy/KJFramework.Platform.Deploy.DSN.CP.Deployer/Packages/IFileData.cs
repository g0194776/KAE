namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Packages
{
    /// <summary>
    ///     文件数据元接口，提供了相关的基本属性结构。
    /// </summary>
    public interface IFileData
    {
        /// <summary>
        ///     获取或设置当前文件数据包的编号
        /// </summary>
        int CurrentId { get; set; }
        /// <summary>
        ///     获取当前包的二进制数据
        /// </summary>
        byte[] Data { get;  }
    }
}