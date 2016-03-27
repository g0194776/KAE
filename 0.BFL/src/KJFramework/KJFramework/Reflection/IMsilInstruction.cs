using System.Reflection.Emit;

namespace KJFramework.Reflection
{
    /// <summary>
    ///     Microsoft MSIL Lanauage �ṹԪ�ӿ�
    ///    <para>* �˽ṹ����ÿһ��MSILָ��������Ϣ��</para>
    /// </summary>
    public interface IMsilInstruction
    {
        /// <summary>
        ///     ��ȡ������ָ��
        /// </summary>
        OpCode Code { get; set; }
        /// <summary>
        ///     ��ȡ�����ò���
        /// </summary>
        object Operand { get; set; }
        /// <summary>
        ///     ��ز���Ԫ����
        /// </summary>
        byte[] OperandData { get; set; }
        /// <summary>
        ///     ��ǰָ���ƫ�Ƶ�ַ
        /// </summary>
        int Offset { get; set; }
        /// <summary>
        ///     ������ڴ���ָ��Ŀ��ӻ����
        /// </summary>
        /// <returns>���ؿ��ӻ����</returns>
        string GetCode();
    }
}