namespace KJFramework.EventArgs
{
    /// <summary>
    ///     KJFramework�ṩ����������һ�����¼���ʹ�ø��¼�����ʡȥ���±�д�ض��¼�����¼���
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    public class LightSingleArgEventArgs<T> : CanDisposeEventArgs
    {
        #region ��Ա

        private T _target;

        /// <summary>
        ///     ��ȡĿ��
        /// </summary>
        public T Target
        {
            get { return _target; }
        }

        #endregion

        #region ���캯��

        /// <summary>
        ///     KJFramework�ṩ����������һ�����¼���ʹ�ø��¼�����ʡȥ���±�д�ض��¼�����¼���
        /// </summary>
        /// <param name="target">����</param>
        public LightSingleArgEventArgs(T target)
        {
            _target = target;
        }

        #endregion

    }
}