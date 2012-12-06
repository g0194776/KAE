namespace KJFramework
{
    /// <summary>
    ///     可控制对象接口，提供了一个对象的开始，停止方法。
    /// </summary>
    public interface IControlable
    {
        /// <summary>
        ///     开始执行
        /// </summary>
        void Start();
        /// <summary>
        ///     停止执行
        /// </summary>
        void Stop();  
    }
}