namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Packages
{
    /// <summary>
    ///     文件数据包元接口，提供了相关的基本操作。
    /// </summary>
    public interface IFilePackage
    {
        /// <summary>
        ///     获取当前文件数据包所关联的请求令牌
        /// </summary>
        string RequestToken { get;  }
        /// <summary>
        ///     获取或设置总共的封包片数目
        /// </summary>
        int TotalPackageCount { get; set; }
        /// <summary>
        ///     获取或设置服务名
        /// </summary>
        string ServiceName { get; set; }
        /// <summary>
        ///     获取或设置服务别名
        /// </summary>
        string Name { get; set; }
        /// <summary>
        ///     获取或设置服务版本
        /// </summary>
        string Version { get; set; }
        /// <summary>
        ///     获取或设置服务描述
        /// </summary>
        string Description { get; set; }
        /// <summary>
        ///     添加一个文件数据封包片
        /// </summary>
        /// <param name="fileData">封包片</param>
        void Add(IFileData fileData);
        /// <summary>
        ///     检测当前的文件包是否已经接收完整
        /// </summary>
        /// <returns>返回确实的文件包ID</returns>
        int[] CheckComplate();
        /// <summary>
        ///     获取此文件包的完整二进制数据
        /// </summary>
        /// <returns>返回完整的二进制数据</returns>
        /// <exception cref="System.Exception">不完整的数据</exception>
        byte[] GetData();
    }
}