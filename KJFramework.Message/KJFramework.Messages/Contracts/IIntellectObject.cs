namespace KJFramework.Messages.Contracts
{
    /// <summary>
    ///     智能对象接口，提供了对于智能转换二进制能力的基础支持。
    /// </summary>
    public interface IIntellectObject
    {
        /// <summary>
        ///     获取或设置二进制数据体
        /// </summary>
        byte[] Body { get; set; }
        /// <summary>
        ///     获取一个值，该值表示了当前是否已经从第三方客户数据转换为元数据。
        /// </summary>
        bool IsBind { get; }
        /// <summary>
        ///     获取一个值，该值表示了当前实体类是不是以兼容模式解析的。
        /// </summary>
        bool CompatibleMode { get; }
        /// <summary>
        ///     绑定到元数据
        /// </summary>
        void Bind();
    }
}