using System;
namespace KJFramework.Net.EventArgs
{
    public delegate void DelegateUpdateProcessing(Object sender, UpdateProcessingEventArgs e);
    /// <summary>
    ///     ����״̬�¼�
    /// </summary>
    public class UpdateProcessingEventArgs : System.EventArgs
    {
        private String _state;
        /// <summary>
        ///     ��ȡ��ǰ���µ�״̬
        /// </summary>
        public String State
        {
            get { return _state; }
        }

        /// <summary>
        ///     ����״̬�¼�
        /// </summary>
        /// <param name="state">��ǰ���µ�״̬</param>
        public UpdateProcessingEventArgs(String state)
        {
            _state = state;
        }

    }
}