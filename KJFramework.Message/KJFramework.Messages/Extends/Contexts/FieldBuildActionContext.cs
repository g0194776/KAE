using System;
using System.Collections.Generic;
using System.Linq;
using KJFramework.Messages.Attributes;

namespace KJFramework.Messages.Extends.Contexts
{
    /// <summary>
    ///     消息字段构造动作上下文，提供了相关的基本操作。
    /// </summary>
    internal class FieldBuildActionContext<T> : IFieldBuildActionContext<T>
    {
        #region 构造函数

        /// <summary>
        ///     消息字段构造动作上下文，提供了相关的基本操作。
        /// </summary>
        /// <param name="metadatas">元数据集合</param>
        internal FieldBuildActionContext(ref Dictionary<IntellectPropertyAttribute, T> metadatas)
        {
            _metadatas = metadatas;
        }

        #endregion

        #region 成员

        private readonly Dictionary<IntellectPropertyAttribute, T> _metadatas;

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IFieldBuildActionContext

        /// <summary>
        ///     获取具有指定编号的消息字段上下文
        /// </summary>
        /// <param name="id">字段编号</param>
        /// <returns>返回消息字段上下文</returns>
        public KeyValuePair<IIntellectProperty, T>? GetValue(int id)
        {
            if (_metadatas != null)
            {
                var result = _metadatas.Where(data => data.Key.Id == id);
                if (result != null && result.Count() > 0)
                {
                    var item = result.First();
                    return new KeyValuePair<IIntellectProperty, T>(item.Key, item.Value);
                }
            }
            return null;
        }

        /// <summary>
        ///     获取具有指定编号的消息字段上下文
        /// </summary>
        /// <param name="tag">字段附属名称</param>
        /// <returns>返回消息字段上下文</returns>
        public KeyValuePair<IIntellectProperty, T>? GetValue(string tag)
        {
            if (String.IsNullOrEmpty(tag))
            {
                return null;
            }
            if (_metadatas != null)
            {
                var result = _metadatas.Where(data => data.Key.Tag == tag);
                if (result != null && result.Count() > 0)
                {
                    var item = result.First();
                    return new KeyValuePair<IIntellectProperty, T>(item.Key, item.Value);
                }
            }
            return null;
        }

        #endregion
    }
}