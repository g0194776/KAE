namespace KJFramework.IO.EventArgs
{
    /// <summary>
    ///     鼠标移动事件
    /// </summary>
    public class MouseMoveEventArgs : System.EventArgs
    {
        #region 成员

        private double _x;
        /// <summary>
        ///     X轴坐标
        /// </summary>
        public double X
        {
            get { return _x; }
            set { _x = value; }
        }
        private double _y;
        /// <summary>
        ///     Y轴坐标
        /// </summary>
        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        ///     鼠标移动事件
        /// </summary>
        /// <param name="x">X轴坐标</param>
        /// <param name="y">Y轴坐标</param>
        public MouseMoveEventArgs(double x, double y)
        {
            _y = y;
            _x = x;
        }

        #endregion
    }
}