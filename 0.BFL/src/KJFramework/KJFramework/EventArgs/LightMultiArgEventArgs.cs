namespace KJFramework.EventArgs
{
    /// <summary>
    ///     KJFramework提供的轻量级多参数事件，使用该事件可以省去重新编写特定事件类的事件。
    /// </summary>
    /// <typeparam name="T">参数类型</typeparam>
    public class LightMultiArgEventArgs<T> : CanDisposeEventArgs
    {
        #region 成员

        private T[] _target;

        /// <summary>
        ///     获取目标
        /// </summary>
        public T[] Target
        {
            get { return _target; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        ///     KJFramework提供的轻量级多参数事件，使用该事件可以省去重新编写特定事件类的事件。
        /// </summary>
        /// <param name="target">参数</param>
        public LightMultiArgEventArgs(params T[] target)
        {
            _target = target;
        }

        #endregion

    }
}