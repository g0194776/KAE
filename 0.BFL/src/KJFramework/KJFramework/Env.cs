using KJFramework.Enums;

namespace KJFramework
{
    /// <summary>
    ///     共有运行环境设置类
    /// </summary>
    public static class Env
    {
        #region Members.

        /// <summary>
        ///     获取或设置当前运行环境信息
        /// </summary>
        public static Envs Target { get; set; }

        #endregion
    }
}