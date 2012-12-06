namespace KJFramework.Plugin
{
    /// <summary>
    ///     拥有可执行效果的插件元接口，提供了相关的“执行”基本操作。
    /// </summary>
    public interface IExecutable : IControlable
    {
        /// <summary>
        ///     停止时需要做的动作
        /// </summary>
        void OnTerminate();
    }
}
