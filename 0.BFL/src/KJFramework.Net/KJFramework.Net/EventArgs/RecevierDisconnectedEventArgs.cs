using System;
namespace KJFramework.Net.EventArgs
{
    /// <summary>
    ///     �������Ͽ������¼�
    /// </summary>
    public class RecevierDisconnectedEventArgs : System.EventArgs
    {
        #region ��Ա

        private String _key;
        /// <summary>
        ///     ��ȡ������Ψһ��ʶ
        /// </summary>
        public String Key
        {
            get { return _key; }
        }

        #endregion

        #region ���캯��

        /// <summary>
        ///     �������Ͽ������¼�
        /// </summary>
        /// <param name="key">Ψһ��ʶ</param>
        public RecevierDisconnectedEventArgs(String key)
        {
            _key = key;
        }

        #endregion
    }
}