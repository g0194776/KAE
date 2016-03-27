using System;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Objects
{
    public class KeyValueItem : IntellectObject
    {
        #region Members
        [IntellectProperty(0, IsRequire = true)]
        public string Key { get; set; }

        [IntellectProperty(1, IsRequire = false)]
        public string Value { get; set; }

        [IntellectProperty(2, IsRequire = false)]
        public DateTime CreateTime { get; set; }

        [IntellectProperty(3, IsRequire = false)]
        public DateTime LastOprTime { get; set; }
        #endregion
    }
}