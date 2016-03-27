using System;
using System.Reflection.Emit;

namespace KJFramework.Reflection
{
    /// <summary>
    ///     Microsoft MSIL Lanauage �ṹ
    ///    <para>* �˽ṹ����ÿһ��MSILָ��������Ϣ��</para>
    /// </summary>
    public class MsilInstruction : IMsilInstruction
    {
        #region Members

        protected OpCode _code;
        protected object _operand;
        protected byte[] _operandData;
        protected int _offset;

        /// <summary>
        ///     ��ȡ������ָ��
        /// </summary>
        public OpCode Code
        {
            get { return _code; }
            set { _code = value; }
        }

        /// <summary>
        ///     ��ȡ�����ò���
        /// </summary>
        public object Operand
        {
            get { return _operand; }
            set { _operand = value; }
        }

        /// <summary>
        ///     ��ز���Ԫ����
        /// </summary>
        public byte[] OperandData
        {
            get { return _operandData; }
            set { _operandData = value; }
        }

        /// <summary>
        ///     ��ǰָ���ƫ�Ƶ�ַ
        /// </summary>
        public int Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        #endregion

        #region Functions

        /// <summary>
        ///     ������ڴ���ָ��Ŀ��ӻ����
        /// </summary>
        /// <returns>���ؿ��ӻ����</returns>
        public string GetCode()
        {
            string result = "";
            result += GetExpandedOffset(_offset) + " : " + _code;
            if (_operand != null)
            {
                switch (_code.OperandType)
                {
                    case OperandType.InlineField:
                        System.Reflection.FieldInfo fOperand = ((System.Reflection.FieldInfo)_operand);
                        result += " " + Reflector.ProcessSpecialTypes(fOperand.FieldType.ToString()) + " " +
                            Reflector.ProcessSpecialTypes(fOperand.ReflectedType.ToString()) +
                            "::" + fOperand.Name + "";
                        break;
                    case OperandType.InlineMethod:
                        try
                        {
                            System.Reflection.MethodInfo mOperand = (System.Reflection.MethodInfo)_operand;
                            result += " ";
                            if (!mOperand.IsStatic) result += "instance ";
                            result += Reflector.ProcessSpecialTypes(mOperand.ReturnType.ToString()) +
                                " " + Reflector.ProcessSpecialTypes(mOperand.ReflectedType.ToString()) +
                                "::" + mOperand.Name + "()";
                        }
                        catch
                        {
                            try
                            {
                                System.Reflection.ConstructorInfo mOperand = (System.Reflection.ConstructorInfo)_operand;
                                result += " ";
                                if (!mOperand.IsStatic) result += "instance ";
                                result += "void " +
                                    Reflector.ProcessSpecialTypes(mOperand.ReflectedType.ToString()) +
                                    "::" + mOperand.Name + "()";
                            }
                            catch
                            {
                            }
                        }
                        break;
                    case OperandType.ShortInlineBrTarget:
                    case OperandType.InlineBrTarget:
                        result += " " + GetExpandedOffset((int)_operand);
                        break;
                    case OperandType.InlineType:
                        result += " " + Reflector.ProcessSpecialTypes(_operand.ToString());
                        break;
                    case OperandType.InlineString:
                        if (_operand.ToString() == "\r\n") result += " \"\\r\\n\"";
                        else result += " \"" + _operand.ToString() + "\"";
                        break;
                    case OperandType.ShortInlineVar:
                        result += _operand.ToString();
                        break;
                    case OperandType.InlineI:
                    case OperandType.InlineI8:
                    case OperandType.InlineR:
                    case OperandType.ShortInlineI:
                    case OperandType.ShortInlineR:
                        result += _operand.ToString();
                        break;
                    case OperandType.InlineTok:
                        if (_operand is Type)
                            result += ((Type)_operand).FullName;
                        else
                            result += "not supported";
                        break;

                    default: result += "not supported"; break;
                }
            }
            return result;
        }

        /// <summary>
        /// Add enough zeros to a number as to be represented on 4 characters
        /// </summary>
        /// <param name="offset">
        /// The number that must be represented on 4 characters
        /// </param>
        /// <returns>
        /// </returns>
        private string GetExpandedOffset(long offset)
        {
            string result = offset.ToString();
            for (int i = 0; result.Length < 4; i++)
            {
                result = "0" + result;
            }
            return result;
        }

        #endregion
    }
}