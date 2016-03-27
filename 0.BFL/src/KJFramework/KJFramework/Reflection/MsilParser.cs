using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace KJFramework.Reflection
{
    /// <summary>
    ///     Microsoft MSIL Lanauage解析器
    /// </summary>
    public static class MsilParser
    {
        #region Members

        private static byte[] _il;
        private static MethodInfo _mi;

        #endregion

        #region il read methods

        private static int ReadInt16(byte[] _il, ref int position)
        {
            return ((_il[position++] | (_il[position++] << 8)));
        }
        private static ushort ReadUInt16(byte[] _il, ref int position)
        {
            return (ushort)((_il[position++] | (_il[position++] << 8)));
        }
        private static int ReadInt32(byte[] _il, ref int position)
        {
            return (((_il[position++] | (_il[position++] << 8)) | (_il[position++] << 0x10)) | (_il[position++] << 0x18));
        }
        private static ulong ReadInt64(byte[] _il, ref int position)
        {
            return (ulong)(((_il[position++] | (_il[position++] << 8)) | (_il[position++] << 0x10)) | (_il[position++] << 0x18) | (_il[position++] << 0x20) | (_il[position++] << 0x28) | (_il[position++] << 0x30) | (_il[position++] << 0x38));
        }
        private static double ReadDouble(byte[] _il, ref int position)
        {
            return (((_il[position++] | (_il[position++] << 8)) | (_il[position++] << 0x10)) | (_il[position++] << 0x18) | (_il[position++] << 0x20) | (_il[position++] << 0x28) | (_il[position++] << 0x30) | (_il[position++] << 0x38));
        }
        private static sbyte ReadSByte(byte[] _il, ref int position)
        {
            return (sbyte)_il[position++];
        }
        private static byte ReadByte(byte[] _il, ref int position)
        {
            return (byte)_il[position++];
        }
        private static Single ReadSingle(byte[] _il, ref int position)
        {
            return (Single)(((_il[position++] | (_il[position++] << 8)) | (_il[position++] << 0x10)) | (_il[position++] << 0x18));
        }
        #endregion

        #region Functions

        /// <summary>
        /// Constructs the array of ILInstructions according to the IL byte code.
        /// </summary>
        /// <param name="module"></param>
        private static IMsilInstruction[] ConstructInstructions(Module module)
        {
            byte[] il = _il;
            int position = 0;
            List<IMsilInstruction> instructions = new List<IMsilInstruction>();
            while (position < il.Length)
            {
                MsilInstruction instruction = new MsilInstruction();

                // get the operation code of the current instruction
                OpCode code = OpCodes.Nop;
                ushort value = il[position++];
                if (value != 0xfe)
                {
                    code = Reflector.SingleByteOpCodes[(int)value];
                }
                else
                {
                    value = il[position++];
                    code = Reflector.MultiByteOpCodes[(int)value];
                    value = (ushort)(value | 0xfe00);
                }
                instruction.Code = code;
                instruction.Offset = position - 1;
                int metadataToken = 0;
                // get the operand of the current operation
                switch (code.OperandType)
                {
                    case OperandType.InlineBrTarget:
                        metadataToken = ReadInt32(il, ref position);
                        metadataToken += position;
                        instruction.Operand = metadataToken;
                        break;
                    case OperandType.InlineField:
                        metadataToken = ReadInt32(il, ref position);
                        instruction.Operand = module.ResolveField(metadataToken);
                        break;
                    case OperandType.InlineMethod:
                        metadataToken = ReadInt32(il, ref position);
                        try
                        {
                            instruction.Operand = module.ResolveMethod(metadataToken);
                        }
                        catch
                        {
                            instruction.Operand = module.ResolveMember(metadataToken);
                        }
                        break;
                    case OperandType.InlineSig:
                        metadataToken = ReadInt32(il, ref position);
                        instruction.Operand = module.ResolveSignature(metadataToken);
                        break;
                    case OperandType.InlineTok:
                        metadataToken = ReadInt32(il, ref position);
                        try
                        {
                            instruction.Operand = module.ResolveType(metadataToken);
                        }
                        catch
                        {

                        }
                        // SSS : see what to do here
                        break;
                    case OperandType.InlineType:
                        metadataToken = ReadInt32(il, ref position);
                        // now we call the ResolveType always using the generic attributes type in order
                        // to support decompilation of generic methods and classes

                        // thanks to the guys from code project who commented on this missing feature

                        instruction.Operand = module.ResolveType(metadataToken, _mi.DeclaringType.GetGenericArguments(), _mi.GetGenericArguments());
                        break;
                    case OperandType.InlineI:
                        {
                            instruction.Operand = ReadInt32(il, ref position);
                            break;
                        }
                    case OperandType.InlineI8:
                        {
                            instruction.Operand = ReadInt64(il, ref position);
                            break;
                        }
                    case OperandType.InlineNone:
                        {
                            instruction.Operand = null;
                            break;
                        }
                    case OperandType.InlineR:
                        {
                            instruction.Operand = ReadDouble(il, ref position);
                            break;
                        }
                    case OperandType.InlineString:
                        {
                            metadataToken = ReadInt32(il, ref position);
                            instruction.Operand = module.ResolveString(metadataToken);
                            break;
                        }
                    case OperandType.InlineSwitch:
                        {
                            int count = ReadInt32(il, ref position);
                            int[] casesAddresses = new int[count];
                            for (int i = 0; i < count; i++)
                            {
                                casesAddresses[i] = ReadInt32(il, ref position);
                            }
                            int[] cases = new int[count];
                            for (int i = 0; i < count; i++)
                            {
                                cases[i] = position + casesAddresses[i];
                            }
                            break;
                        }
                    case OperandType.InlineVar:
                        {
                            instruction.Operand = ReadUInt16(il, ref position);
                            break;
                        }
                    case OperandType.ShortInlineBrTarget:
                        {
                            instruction.Operand = ReadSByte(il, ref position) + position;
                            break;
                        }
                    case OperandType.ShortInlineI:
                        {
                            instruction.Operand = ReadSByte(il, ref position);
                            break;
                        }
                    case OperandType.ShortInlineR:
                        {
                            instruction.Operand = ReadSingle(il, ref position);
                            break;
                        }
                    case OperandType.ShortInlineVar:
                        {
                            instruction.Operand = ReadByte(il, ref position);
                            break;
                        }
                    default:
                        {
                            throw new System.Exception("Unknown operand type.");
                        }
                }
                instructions.Add(instruction);
            }
            return instructions.ToArray();
        }

        private static object GetRefferencedOperand(Module module, int metadataToken)
        {
            AssemblyName[] assemblyNames = module.Assembly.GetReferencedAssemblies();
            for (int i = 0; i < assemblyNames.Length; i++)
            {
                Module[] modules = Assembly.Load(assemblyNames[i]).GetModules();
                for (int j = 0; j < modules.Length; j++)
                {
                    try
                    {
                        Type t = modules[j].ResolveType(metadataToken);
                        return t;
                    }
                    catch
                    {
                    }
                }
            }
            return null;
        }

        /// <summary>
        ///     将一个.NET方法体解析为一组MSIL语言结构
        /// </summary>
        /// <param name="mi">要解析的方法</param>
        /// <returns>返回解析后的MSIL语言结构</returns>
        public static IMsilInstruction[] Parse(MethodInfo mi)
        {
            _mi = mi;
            if (mi != null && mi.GetMethodBody() != null)
            {
                _il = mi.GetMethodBody().GetILAsByteArray();
                return ConstructInstructions(mi.Module);
            }
            return null;
        }

        #endregion
    }
}