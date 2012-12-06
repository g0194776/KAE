namespace KJFramework.Platform.Deploy.Maintenance
{
    /// <summary>
    ///     可维护的动态服务元接口，提供了相关的基本操作。
    /// </summary>
    public interface IMaintenanceDynamicService
    {
        /// <summary>
        ///     开始心跳
        /// </summary>
        void StartHeartBeat();
        /// <summary>
        ///     停止心跳
        /// </summary>
        void StopHeartBeat();
    }
}