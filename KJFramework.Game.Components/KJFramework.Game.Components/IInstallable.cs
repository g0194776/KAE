namespace KJFramework.Game.Components
{
    /// <summary>
    ///     可支持安装元接口，提供了相关的基本操作。
    /// </summary>
    public interface IInstallable
    {
        /// <summary>
        ///     安装
        /// </summary>
        /// <returns>返回安装的状态</returns>
        bool Install();
        /// <summary>
        ///     卸载
        /// </summary>
        /// <returns>返回卸载的状态</returns>
        bool UnInstall();
    }
}