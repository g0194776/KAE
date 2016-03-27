namespace KJFramework.Dynamic.Visitors
{
    /// <summary>
    ///     组件通道访问器元接口，提供了相关的基本操作
    /// </summary>
    public interface IComponentTunnelVisitor
    {
        /// <summary>
        ///     获取指定组件的隧道
        /// </summary>
        /// <param name="componentName">组件名称</param>
        /// <returns>返回组件的隧道</returns>
        T GetTunnel<T>(string componentName) where T : class;
    }
}