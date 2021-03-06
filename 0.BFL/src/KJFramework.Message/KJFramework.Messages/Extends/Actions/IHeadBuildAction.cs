namespace KJFramework.Messages.Extends.Actions
{
    /// <summary>
    ///     消息头部构造动作元接口，提供了相关的基本操作。
    /// </summary>
    public interface IHeadBuildAction : IBuildAction
    {
        /// <summary>
        ///     构造一个消息头部
        ///     <para>* 消息头部构造规定： 所有头部数据的索引，均小于0。</para>
        /// </summary>
        /// <param name="data">所有字段元数据</param>
        /// <returns>返回头部</returns>
        byte[] Bind(byte[] data);
        /// <summary>
        ///     提取消息头部
        ///     <para>* 消息头部构造规定： 所有头部数据的索引，均小于0。</para>
        /// </summary>
        /// <typeparam name="T">消息头部类型</typeparam>
        /// <param name="data">元数据</param>
        /// <returns>返回提取到的消息头部</returns>
        T Pickup<T>(byte[] data);
    }
}