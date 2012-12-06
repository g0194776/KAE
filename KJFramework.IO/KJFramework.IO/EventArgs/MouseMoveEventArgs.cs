namespace KJFramework.IO.EventArgs
{
    /// <summary>
    ///     ����ƶ��¼�
    /// </summary>
    public class MouseMoveEventArgs : System.EventArgs
    {
        #region ��Ա

        private double _x;
        /// <summary>
        ///     X������
        /// </summary>
        public double X
        {
            get { return _x; }
            set { _x = value; }
        }
        private double _y;
        /// <summary>
        ///     Y������
        /// </summary>
        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }

        #endregion

        #region ���캯��

        /// <summary>
        ///     ����ƶ��¼�
        /// </summary>
        /// <param name="x">X������</param>
        /// <param name="y">Y������</param>
        public MouseMoveEventArgs(double x, double y)
        {
            _y = y;
            _x = x;
        }

        #endregion
    }
}