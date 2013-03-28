using System;
using System.Collections.Generic;

namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     索引表
    /// </summary>
    internal class IndexTable : IIndexTable
    {
        #region Constructor

        /// <summary>
        ///     索引表
        /// </summary>
        public IndexTable()
            : this(new FileFlag(), new List<TypeToken>())
        {
        }

        /// <summary>
        ///     索引表
        /// </summary>
        /// <param name="flag">文件标示</param>
        /// <param name="tokens">已存在的类型令牌</param>
        public IndexTable(IFileFlag flag, IList<TypeToken> tokens)
        {
            Flag = flag;
            _tokens = new Dictionary<ulong, TypeToken>();
            if (tokens != null && tokens.Count > 0)
                foreach (TypeToken token in tokens) _tokens.Add(token.Id, token);
        }

        #endregion

        #region Members

        private readonly Dictionary<ulong, TypeToken> _tokens; 

        #endregion

        #region Implementation of IIndexTable

        /// <summary>
        ///     获取文件头标示
        /// </summary>
        public IFileFlag Flag { get; private set; }
        /// <summary>
        ///     获取或设置授权信息
        /// </summary>
        public IAuthorization Authorization { get; set; }

        /// <summary>
        ///     获取或设置已使用的页数目
        /// </summary>
        public uint UsedPageCounts { get; set; }

        /// <summary>
        ///     获取已拥有的类型令牌数目
        /// </summary>
        public ushort UsedTokenCounts { get { return (ushort) _tokens.Count; } }

        /// <summary>
        ///     添加一个类型令牌
        /// </summary>
        /// <param name="token">要添加的类型令牌</param>
        /// <returns>返回类型令牌</returns>
        /// <exception cref="System.ArgumentNullException">参数不能为空</exception>
        public void AddToken(TypeToken token)
        {
            _tokens.Add(token.Id, token);
        }

        /// <summary>
        ///     获取具有指定编号的类型令牌信息
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>返回类型令牌</returns>
        public TypeToken? GetToken(ulong id)
        {
            TypeToken token;
            return _tokens.TryGetValue(id, out token) ? token : (TypeToken?) null;
        }

        /// <summary>
        ///     获取类型令牌信息集合
        /// </summary>
        public IEnumerable<TypeToken> GetTokens()
        {
            return _tokens.Values;
        }

        #endregion
    }
}