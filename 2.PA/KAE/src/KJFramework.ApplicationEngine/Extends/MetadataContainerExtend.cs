using KJFramework.Messages.Types;

namespace KJFramework.ApplicationEngine.Extends
{
    /// <summary>
    ///     MetadataContainer扩展方法类
    /// </summary>
    public static class MetadataContainerExtend
    {
        #region Methods.

        /// <summary>
        ///     MetadataContainer扩展方法
        /// </summary>
        /// <typeparam name="T">返回的类型</typeparam>
        /// <param name="obj">metadataContainer对象</param>
        /// <param name="id">id编号</param>
        /// <returns>value值</returns>
        public static T GetAttributeByIdSafety<T>(this ResourceBlock obj, byte id)
        {
            if (obj == null) return default(T);
            T value;
            return obj.TryGetAttributeAsType(id, out value) ? value : default(T);
        }

        #endregion
    }
}