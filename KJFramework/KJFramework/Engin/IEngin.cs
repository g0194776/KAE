namespace KJFramework.Engin
{
    /// <summary>
    ///     引擎元接口，提供了相关的基本操作。
    /// </summary>
    public interface IEngin : IControlable
    {
        /// <summary>
        ///     初始化
        /// </summary>
        void Initialize();
        /// <summary>
        ///     获取一个值，该值指示了当前引擎是否已经完成了所有的调度工作。
        /// </summary>
        bool IsFinish { get; }
    }
}