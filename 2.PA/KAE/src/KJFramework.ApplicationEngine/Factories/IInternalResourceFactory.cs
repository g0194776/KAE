namespace KJFramework.ApplicationEngine.Factories
{
    /// <summary>
    ///    内部资源工厂接口
    /// </summary>
    public interface IInternalResourceFactory
    {
        #region Methods.

        /// <summary>
        ///    初始化当前资源工厂
        /// </summary>
        void Initialize();
        /// <summary>
        ///    通过一个字符串的全名称来获取指定的资源
        /// </summary>
        /// <param name="fullname">资源的全名称</param>
        /// <returns>返回资源实例，如果不存在则返回null</returns>
        object GetResource(string fullname);

        #endregion
    }
}