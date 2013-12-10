namespace KJFramework.Messages.Configuration
{
    /// <summary>
    ///   元数据对象形式下的设置集
    /// </summary>
    public static class MetadataObjectSetting
    {
        #region Members.

        /// <summary>
        ///   获取或设置一个值，该值标示了当对一个不存在的ID进行Get操作时，当前元数据对象是否允许返回此ID所代表的类型的默认值
        ///   <para>* 默认值为: false</para>
        /// </summary>
        public static bool ALLOW_GET_NULL_ID_DEFAULT_VALUE = false;


        /// <summary>
        ///   获取一个值，该值标示了可扩展类型与系统内置类型的临界值
        ///   <para>* 默认值为: 127</para>
        /// </summary>
        public const byte TYPE_BOUNDARY = 127; 


        #endregion
    }
}