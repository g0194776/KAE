using KJFramework.Messages.Attributes;

namespace KJFramework.Messages.Extends.Actions
{
    /// <summary>
    ///     扩展构造动作元接口，提供了相关的基本操作。
    /// </summary>
    public interface IExtendBuildAction : IBuildAction
    {
        /// <summary>
        ///     构造一个消息扩展
        /// </summary>
        /// <param name="attribute">字段标签</param>
        /// <param name="data">所有字段元数据</param>
        /// <returns>返回扩展值</returns>
        byte[] Bind(IntellectPropertyAttribute attribute, byte[] data);
        /// <summary>
        ///     提取消息扩展值
        /// </summary>
        /// <typeparam name="T">消息扩展类型</typeparam>
        /// <param name="attribute">字段标签</param>
        /// <param name="data">元数据</param>
        /// <returns>返回提取到的消息扩展值</returns>
        T Pickup<T>(IntellectPropertyAttribute attribute, byte[] data);
    }
}