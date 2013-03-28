using System;
using System.Collections.Generic;

namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     ������
    /// </summary>
    internal class IndexTable : IIndexTable
    {
        #region Constructor

        /// <summary>
        ///     ������
        /// </summary>
        public IndexTable()
            : this(new FileFlag(), new List<TypeToken>())
        {
        }

        /// <summary>
        ///     ������
        /// </summary>
        /// <param name="flag">�ļ���ʾ</param>
        /// <param name="tokens">�Ѵ��ڵ���������</param>
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
        ///     ��ȡ�ļ�ͷ��ʾ
        /// </summary>
        public IFileFlag Flag { get; private set; }
        /// <summary>
        ///     ��ȡ��������Ȩ��Ϣ
        /// </summary>
        public IAuthorization Authorization { get; set; }

        /// <summary>
        ///     ��ȡ��������ʹ�õ�ҳ��Ŀ
        /// </summary>
        public uint UsedPageCounts { get; set; }

        /// <summary>
        ///     ��ȡ��ӵ�е�����������Ŀ
        /// </summary>
        public ushort UsedTokenCounts { get { return (ushort) _tokens.Count; } }

        /// <summary>
        ///     ���һ����������
        /// </summary>
        /// <param name="token">Ҫ��ӵ���������</param>
        /// <returns>������������</returns>
        /// <exception cref="System.ArgumentNullException">��������Ϊ��</exception>
        public void AddToken(TypeToken token)
        {
            _tokens.Add(token.Id, token);
        }

        /// <summary>
        ///     ��ȡ����ָ����ŵ�����������Ϣ
        /// </summary>
        /// <param name="id">���</param>
        /// <returns>������������</returns>
        public TypeToken? GetToken(ulong id)
        {
            TypeToken token;
            return _tokens.TryGetValue(id, out token) ? token : (TypeToken?) null;
        }

        /// <summary>
        ///     ��ȡ����������Ϣ����
        /// </summary>
        public IEnumerable<TypeToken> GetTokens()
        {
            return _tokens.Values;
        }

        #endregion
    }
}