using System.Collections.Generic;

namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     索引表接口
    /// </summary>
    internal interface IIndexTable
    {
        /// <summary>
        ///     获取文件头标示
        /// </summary>
        IFileFlag Flag { get; }
        /// <summary>
        ///     获取或设置授权信息
        /// </summary>
        IAuthorization Authorization { get; set; }
        /// <summary>
        ///     获取或设置已使用的页数目
        /// </summary>
        uint UsedPageCounts { get; set; }
        /// <summary>
        ///     获取已拥有的类型令牌数目
        /// </summary>
        ushort UsedTokenCounts { get; }
        /// <summary>
        ///     添加一个类型令牌
        /// </summary>
        /// <param name="token">要添加的类型令牌</param>
        /// <exception cref="System.ArgumentNullException">参数不能为空</exception>
        void AddToken(TypeToken token);
        /// <summary>
        ///     获取具有指定编号的类型令牌信息
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>返回类型令牌</returns>
        TypeToken? GetToken(ulong id);
        /// <summary>
        ///     获取类型令牌信息集合
        /// </summary>
        IEnumerable<TypeToken> GetTokens();
    }
}