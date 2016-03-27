using System;
namespace KJFramework.EventArgs
{
    /// <summary>
    ///     ����Ϣ�¼�
    /// </summary>
    public class NewInfomationEventArgs : System.EventArgs, IDisposable
    {
        #region ��Ա

        private String _infomation;
        /// <summary>
        ///     ��ȡ�ռ�������Ϣ
        /// </summary>
        public string Infomation
        {
            get { return _infomation; }
        }

        #endregion

        #region ���캯��

        /// <summary>
        ///     ����Ϣ�¼�
        /// </summary>
        /// <param name="infomation">�ռ���������Ϣ</param>
        public NewInfomationEventArgs(String infomation)
        {
            _infomation = infomation;
        }

        #endregion

        #region ��������

        ~NewInfomationEventArgs()
        {
            Dispose();
        }

        #endregion

        #region IDisposable ��Ա

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}