using System.Collections.Generic;
using KJFramework.Messages.Types;
using KJFramework.Messages.ValueStored;

namespace KJFramework.Messages.Contracts
{
    /// <summary>
    ///   元数据对象
    /// </summary>
    public class MetadataContainer : ResourceBlock
    {
        #region Constructors.

        /// <summary>
        ///   元数据对象
        /// </summary>
        public MetadataContainer()
        {
            
        }

        /// <summary>
        ///     内部构造函数，用于初始化一个拥有了内部数据的包装对象
        /// </summary>
        /// <param name="dic">内部结构数据</param>
        internal MetadataContainer(Dictionary<byte, BaseValueStored> dic) : base(dic)
        {
            
        }


        #endregion
    }
}