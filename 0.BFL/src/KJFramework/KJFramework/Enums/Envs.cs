namespace KJFramework.Enums
{
    /// <summary>
    ///     当前运行环境枚举
    /// </summary>
    public enum Envs : byte
    {
        /// <summary>
        ///     基于Windows平台运行的.NET环境
        /// </summary>
        Dotnet = 0x00,
        /// <summary>
        ///     基于Linux平台运行的.NET环境
        /// </summary>
        Mono = 0x01,
        /// <summary>
        ///     基于Linux平台运行的.NET环境
        /// </summary>
        DotnetCore = 0x02
    }
}