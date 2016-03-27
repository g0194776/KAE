using System;

namespace KJFramework.Messages.Attributes
{
    /// <summary>
    ///     智能属性元接口，提供了相关的基本属性结构。
    /// </summary>
    public interface IIntellectProperty
    {
        /// <summary>
        ///     获取属性顺序编号
        ///     <para>* 此编号不能重复。</para>
        /// </summary>
        int Id { get; }
        /// <summary>
        ///     获取一个值，该值标示了当前属性是否必须拥有值。
        /// </summary>
        bool IsRequire { get; }
        /// <summary>
        ///     获取一个值，该值标示了当前属性是否需要进行扩展构造动作。
        ///     <para>* 此属影响范围：第三方消息结构定义器。</para>
        /// </summary>
        bool NeedExtendAction { get; }
        /// <summary>
        ///     获取附属名称
        ///     <para>* 此属影响范围：第三方消息结构定义器。</para>
        /// </summary>
        String Tag { get; }
    }
}