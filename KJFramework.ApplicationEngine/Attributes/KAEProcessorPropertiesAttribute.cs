using System;
using KJFramework.ApplicationEngine.Eums;

namespace KJFramework.ApplicationEngine.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class KAEProcessorPropertiesAttribute : System.Attribute
    {
        #region Members.

        /// <summary>
        ///    获取或设置协议编号
        /// </summary>
        public byte ProtocolId { get; set; }
        /// <summary>
        ///    获取或设置服务编号
        /// </summary>
        public byte ServiceId { get; set; }
        /// <summary>
        ///    获取或设置详细服务分类编号
        /// </summary>
        public byte DetailsId { get; set; }
        /// <summary>
        ///    获取或设置协议类型
        /// </summary>
        public ProtocolTypes ProtocolType { get; set; }

        #endregion
    }
}