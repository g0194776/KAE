using System;
using KJFramework.Basic.Enum;

namespace KJFramework.Basic.Struct
{
    /// <summary>
    ///     快捷方式数据集
    /// </summary>
    public struct ShortCutData
    {
        /// <summary>
        ///     传入参数
        /// </summary>
        public String Arguments;
        /// <summary>
        ///     快捷键
        /// </summary>
        public String HotKey;
        /// <summary>
        ///     快捷方式存放地址
        /// </summary>
        public String InkPath;
        /// <summary>
        ///     目标地址
        /// </summary>
        public String TargetPath;
        /// <summary>
        ///     工作目录
        /// </summary>
        public String WorkingDirectory;
        /// <summary>
        ///     描述信息
        /// </summary>
        public String Description;
        /// <summary>
        ///     图标地址
        /// </summary>
        public String IconLocation;
        /// <summary>
        ///     快捷方式窗体样式
        /// </summary>
        public ShortcutWindowStyle WindowStyle;

    }
}