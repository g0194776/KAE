namespace KJFramework.ServiceModel.Enums
{
    /// <summary>
    ///     参数数据类型
    /// </summary>
    public enum ArgTypes : byte
    {
        Class = 0x00,
        Array = 0x01,
        Enum = 0x02,
        Other = 0x03,
        IntellectObject = 0x04,
        UnDefinded = 0xFF,
    }
}