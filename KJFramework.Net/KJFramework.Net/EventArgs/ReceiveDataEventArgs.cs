namespace KJFramework.Net.EventArgs
{
    /// <summary>
    ///     �����������¼�
    /// </summary>
    public class ReceiveDataEventArgs : System.EventArgs
    {
        #region ��Ա

        private byte[] _data;
        /// <summary>
        ///     ��ȡ���յ�����
        /// </summary>
        public byte[] Data
        {
            get { return _data; }
        }

        #endregion

        #region ���캯��

        /// <summary>
        ///     �����������¼�
        /// </summary>
        /// <param name="data">�µ�����</param>
        public ReceiveDataEventArgs(byte[] data)
        {
            _data = data;
        }

        #endregion
    }
}