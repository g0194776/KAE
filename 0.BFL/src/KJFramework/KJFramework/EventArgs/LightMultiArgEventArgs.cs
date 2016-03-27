namespace KJFramework.EventArgs
{
    /// <summary>
    ///     KJFramework�ṩ��������������¼���ʹ�ø��¼�����ʡȥ���±�д�ض��¼�����¼���
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    public class LightMultiArgEventArgs<T> : CanDisposeEventArgs
    {
        #region ��Ա

        private T[] _target;

        /// <summary>
        ///     ��ȡĿ��
        /// </summary>
        public T[] Target
        {
            get { return _target; }
        }

        #endregion

        #region ���캯��

        /// <summary>
        ///     KJFramework�ṩ��������������¼���ʹ�ø��¼�����ʡȥ���±�д�ض��¼�����¼���
        /// </summary>
        /// <param name="target">����</param>
        public LightMultiArgEventArgs(params T[] target)
        {
            _target = target;
        }

        #endregion

    }
}