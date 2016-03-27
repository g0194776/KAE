using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace Test.Protocols.Scenario1
{
    /// <summary>
    ///     一个简单的消息结构
    ///     <para>要求的最终形式为：总长度 + 各个字段值(字符串的话，前面有个长度标示)</para>
    /// </summary>
    public class SimpleMessage : IntellectObject
    {
        [IntellectProperty(0, IsRequire = true)]
        public int ProtocolId { get; set; }

        [IntellectProperty(1, IsRequire = true)]
        public int Serviceid { get; set; }

        [IntellectProperty(2, IsRequire = true)]
        public int ServiceDetailsId { get; set; }

        [IntellectProperty(3)]
        public string Content { get; set; }
    }
}