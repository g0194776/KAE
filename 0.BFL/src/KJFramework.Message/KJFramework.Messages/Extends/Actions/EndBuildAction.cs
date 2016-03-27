namespace KJFramework.Messages.Extends.Actions
{
    /// <summary>
    ///     消息尾部构造动作抽象父类，提供了相关的基本操作。
    /// </summary>
    public abstract class EndBuildAction : BuildAction, IEndBuildAction
    {
        #region Implementation of IEndBuildAction

        /// <summary>
        ///     构造一个消息结尾
        ///     <para>* 消息结尾构造规定： 所有头部数据的索引，均大于等于50000。</para>
        /// </summary>
        /// <param name="data">所有字段元数据</param>
        /// <returns>返回结尾</returns>
        public abstract byte[] Bind(byte[] data);

        /// <summary>
        ///     提取消息结尾
        ///     <para>* 消息结尾构造规定： 所有头部数据的索引，均大于等于50000。</para>
        /// </summary>
        /// <typeparam name="T">消息结尾类型</typeparam>
        /// <param name="data">元数据</param>
        /// <returns>返回提取到的消息结尾</returns>
        public abstract T Pickup<T>(byte[] data);

        #endregion
    }
}