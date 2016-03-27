using System;

namespace KJFramework.Security
{
    /// <summary>
    ///     ���ƣ��ṩ����صĻ�����ȫ���ԡ�
    /// </summary>
    [Serializable]
    public class Token : IToken
    {
        #region ���캯��

        /// <summary>
        ///     ���ƣ��ṩ����صĻ�����ȫ���ԡ�
        /// </summary>
        /// <param name="content">��������</param>
        public Token(byte[] content)
        {
            _content = content;
        }

        #endregion

        #region IToken Members

        private byte[] _content;

        /// <summary>
        ///     ��ȡ��������
        /// </summary>
        public byte[] Content
        {
            get { return _content; }
        }

        #endregion
    }
}