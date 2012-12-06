using System;
using System.Collections.Generic;

namespace KJFramework.Basic.Collection
{
    /// <summary>
    ///     扩展类型，提供了Dictionary<>泛型的基本功能以及相关的扩展功能：
    ///        * 自定义索引器
    /// </summary>
    /// <typeparam name="TKeyType">键类型</typeparam>
    /// <typeparam name="T">值类型</typeparam>
    [Serializable]
    public class XDictionary<TKeyType, T> : Dictionary<TKeyType, T>
    {
        /// <summary>
        ///     根据指定索引获取集合中对应值
        /// </summary>
        /// <param name="indexer">索引</param>
        /// <returns>返回对应的值</returns>
        public new T this[TKeyType indexer]
        {
            get 
            {
                T obj = default(T);
                try
                {
                    obj = base[indexer];
                }
                catch(System.Exception)
                {}
                return obj;
            }
            set 
            {
                base[indexer] = value;
            }
        }
    }
}
