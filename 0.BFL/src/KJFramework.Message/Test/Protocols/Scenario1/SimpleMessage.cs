using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace Test.Protocols.Scenario1
{
    /// <summary>
    ///     һ���򵥵���Ϣ�ṹ
    ///     <para>Ҫ���������ʽΪ���ܳ��� + �����ֶ�ֵ(�ַ����Ļ���ǰ���и����ȱ�ʾ)</para>
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