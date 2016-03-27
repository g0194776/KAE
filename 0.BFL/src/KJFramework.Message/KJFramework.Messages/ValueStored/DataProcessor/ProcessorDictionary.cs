using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using KJFramework.Messages.Engine;
using KJFramework.Messages.Enums;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.Proxies;
using KJFramework.Messages.Types;

namespace KJFramework.Messages.ValueStored.DataProcessor
{
    /// <summary>
    ///     用于处理非数组类型的集合类
    /// </summary>
    public static class ProcessorDictionary
    {
        #region Constructors

        static ProcessorDictionary()
        {
            InitializeValueProcessorDictionary();
            InitializeDataProcessorDictionary();
        }

        #endregion

        #region Members

        internal static Dictionary<PropertyTypes, Action<IMemorySegmentProxy, BaseValueStored>> Processers = new Dictionary<PropertyTypes, Action<IMemorySegmentProxy, BaseValueStored>>();
        internal static Dictionary<byte, Action<IMemorySegmentProxy, BaseValueStored>> ValueActions = new Dictionary<byte, Action<IMemorySegmentProxy, BaseValueStored>>();
        internal static Dictionary<byte, Action<ResourceBlock, byte, byte[], int, uint>> DataActions = new Dictionary<byte, Action<ResourceBlock, byte, byte[], int, uint>>();

        #endregion

        #region Methods.

        /// <summary>
        ///     初始化普通类型第三方数据转元数据的处理器
        /// </summary>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        private static void InitializeValueProcessorDictionary()
        {
            ValueActions.Add((byte)PropertyTypes.Boolean,
                        delegate(IMemorySegmentProxy proxy, BaseValueStored valueStored)
                        {
                            if(valueStored.GetValue<bool>() == DefaultValue.Boolean) return;
                            proxy.WriteBoolean(valueStored.GetValue<bool>());
                        });
            ValueActions.Add((byte)PropertyTypes.Byte,
                        delegate(IMemorySegmentProxy proxy, BaseValueStored valueStored)
                        {
                            if (valueStored.GetValue<byte>() == DefaultValue.Byte) return;
                            proxy.WriteByte(valueStored.GetValue<byte>());
                        });
            ValueActions.Add((byte)PropertyTypes.SByte,
                        delegate(IMemorySegmentProxy proxy, BaseValueStored valueStored)
                        {
                            if (valueStored.GetValue<sbyte>() == DefaultValue.SByte) return;
                            proxy.WriteSByte(valueStored.GetValue<sbyte>());
                        });
            ValueActions.Add((byte)PropertyTypes.Char,
                        delegate(IMemorySegmentProxy proxy, BaseValueStored valueStored)
                        {
                            if (valueStored.GetValue<char>() == DefaultValue.Char) return;
                            proxy.WriteChar(valueStored.GetValue<char>());
                        });
            ValueActions.Add((byte)PropertyTypes.Int16,
                        delegate(IMemorySegmentProxy proxy, BaseValueStored valueStored)
                        {
                            if (valueStored.GetValue<short>() == DefaultValue.Int16) return;
                            proxy.WriteInt16(valueStored.GetValue<short>());
                        });
            ValueActions.Add((byte)PropertyTypes.UInt16,
                        delegate(IMemorySegmentProxy proxy, BaseValueStored valueStored)
                        {
                            if (valueStored.GetValue<ushort>() == DefaultValue.UInt16) return;
                            proxy.WriteUInt16(valueStored.GetValue<ushort>());
                        });
            ValueActions.Add((byte)PropertyTypes.Int32,
                        delegate(IMemorySegmentProxy proxy, BaseValueStored valueStored)
                        {
                            if (valueStored.GetValue<int>() == DefaultValue.Int32) return;
                            proxy.WriteInt32(valueStored.GetValue<int>());
                        });
            ValueActions.Add((byte)PropertyTypes.UInt32,
                        delegate(IMemorySegmentProxy proxy, BaseValueStored valueStored)
                        {
                            if (valueStored.GetValue<uint>() == DefaultValue.UInt32) return;
                            proxy.WriteUInt32(valueStored.GetValue<uint>());
                        });
            ValueActions.Add((byte)PropertyTypes.Int64,
                        delegate(IMemorySegmentProxy proxy, BaseValueStored valueStored)
                        {
                            if (valueStored.GetValue<long>() == DefaultValue.Int64) return;
                            proxy.WriteInt64(valueStored.GetValue<long>());
                        });
            ValueActions.Add((byte)PropertyTypes.UInt64,
                        delegate(IMemorySegmentProxy proxy, BaseValueStored valueStored)
                        {
                            if (valueStored.GetValue<ulong>() == DefaultValue.UInt64) return;
                            proxy.WriteUInt64(valueStored.GetValue<ulong>());
                        });
            ValueActions.Add((byte)PropertyTypes.Float,
                        delegate(IMemorySegmentProxy proxy, BaseValueStored valueStored)
                        {
                            if (valueStored.GetValue<float>() == DefaultValue.Float) return;
                            proxy.WriteFloat(valueStored.GetValue<float>());
                        });
            ValueActions.Add((byte)PropertyTypes.Double,
                        delegate(IMemorySegmentProxy proxy, BaseValueStored valueStored)
                        {
                            if (valueStored.GetValue<double>() == DefaultValue.Double) return;
                            proxy.WriteDouble(valueStored.GetValue<double>());
                        });
            ValueActions.Add((byte)PropertyTypes.Decimal,
                        delegate(IMemorySegmentProxy proxy, BaseValueStored valueStored)
                        {
                            if (valueStored.GetValue<decimal>() == DefaultValue.Decimal) return;
                            proxy.WriteDecimal(valueStored.GetValue<decimal>());
                        });
            ValueActions.Add((byte)PropertyTypes.String, (proxy, valueStored) => proxy.WriteString(valueStored.GetValue<string>()));
            ValueActions.Add((byte)PropertyTypes.DateTime,
                        delegate(IMemorySegmentProxy proxy, BaseValueStored valueStored)
                        {
                            if (valueStored.GetValue<DateTime>() == DefaultValue.DateTime) return;
                            proxy.WriteDateTime(valueStored.GetValue<DateTime>());
                        });
            ValueActions.Add((byte)PropertyTypes.Guid,
                        delegate(IMemorySegmentProxy proxy, BaseValueStored valueStored)
                        {
                            if (valueStored.GetValue<Guid>() == DefaultValue.Guid) return;
                            proxy.WriteGuid(valueStored.GetValue<Guid>());
                        });
            ValueActions.Add((byte)PropertyTypes.IPEndPoint, (proxy, valueStored) => proxy.WriteIPEndPoint(valueStored.GetValue<IPEndPoint>()));
            ValueActions.Add((byte)PropertyTypes.IntPtr,
                        delegate(IMemorySegmentProxy proxy, BaseValueStored valueStored)
                        {
                            if (valueStored.GetValue<IntPtr>() == DefaultValue.IntPtr) return;
                            proxy.WriteIntPtr(valueStored.GetValue<IntPtr>());
                        });
            ValueActions.Add((byte)PropertyTypes.ResourceBlock, (proxy, valueStored) => MetadataObjectEngine.ToBytes(valueStored.GetValue<ResourceBlock>(), proxy));
            ValueActions.Add((byte)PropertyTypes.TimeSpan,
                        delegate(IMemorySegmentProxy proxy, BaseValueStored valueStored)
                        {
                            if (valueStored.GetValue<TimeSpan>() == DefaultValue.TimeSpan) return;
                            proxy.WriteTimeSpan(valueStored.GetValue<TimeSpan>());
                        });
            ValueActions.Add((byte)PropertyTypes.BitFlag, (proxy, valueStored) => proxy.WriteBitFlag(valueStored.GetValue<BitFlag>()));
            ValueActions.Add((byte)PropertyTypes.Blob,
                        delegate(IMemorySegmentProxy proxy, BaseValueStored valueStored)
                        {
                            if (proxy == null) throw new ArgumentNullException("proxy");
                            if (valueStored == null) throw new ArgumentNullException("valueStored");
                            byte[] data = valueStored.GetValue<Blob>().Compress();
                            proxy.WriteMemory(data, 0U, (uint)data.Length);
                        });
            ValueActions.Add((byte)PropertyTypes.Null, delegate { });
        }

        /// <summary>
        ///     初始化普通类型元数据转第三方数据的处理器
        /// </summary>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        private static unsafe void InitializeDataProcessorDictionary()
        {
            DataActions.Add((byte)PropertyTypes.Boolean,
                delegate(ResourceBlock metadataObject, byte id, byte[] byteData, int offsetStart, uint offsetLength)
                {
                    if (offsetLength == 0) metadataObject.SetAttribute(id, new BooleanValueStored(DefaultValue.Boolean));
                    else metadataObject.SetAttribute(id, new BooleanValueStored(BitConverter.ToBoolean(byteData, offsetStart)));
                });
            DataActions.Add((byte)PropertyTypes.Byte,
                delegate(ResourceBlock metadataObject, byte id, byte[] byteData, int offsetStart, uint offsetLength)
                {
                    if (offsetLength == 0) metadataObject.SetAttribute(id, new ByteValueStored(DefaultValue.Byte));
                    else metadataObject.SetAttribute(id, new ByteValueStored(byteData[offsetStart]));
                });
            DataActions.Add((byte)PropertyTypes.SByte,
                delegate(ResourceBlock metadataObject, byte id, byte[] byteData, int offsetStart, uint offsetLength)
                {
                    if (offsetLength == 0) metadataObject.SetAttribute(id, new SByteValueStored(DefaultValue.SByte));
                    else
                    {
                        fixed (byte* pByte = (&byteData[offsetStart]))
                            metadataObject.SetAttribute(id, new SByteValueStored(*(sbyte*)pByte));
                    }
                });
            DataActions.Add((byte)PropertyTypes.Char,
                delegate(ResourceBlock metadataObject, byte id, byte[] byteData, int offsetStart, uint offsetLength)
                {
                    if (offsetLength == 0) metadataObject.SetAttribute(id, new CharValueStored(DefaultValue.Char));
                    else metadataObject.SetAttribute(id, new CharValueStored(BitConverter.ToChar(byteData, offsetStart)));
                });
            DataActions.Add((byte)PropertyTypes.Int16,
                delegate(ResourceBlock metadataObject, byte id, byte[] byteData, int offsetStart, uint offsetLength)
                {
                    if (offsetLength == 0) metadataObject.SetAttribute(id, new Int16ValueStored(DefaultValue.Int16));
                    else metadataObject.SetAttribute(id, new Int16ValueStored(BitConverter.ToInt16(byteData, offsetStart)));
                });
            DataActions.Add((byte)PropertyTypes.UInt16,
                delegate(ResourceBlock metadataObject, byte id, byte[] byteData, int offsetStart, uint offsetLength)
                {
                    if (offsetLength == 0) metadataObject.SetAttribute(id, new UInt16ValueStored(DefaultValue.UInt16));
                    else metadataObject.SetAttribute(id, new UInt16ValueStored(BitConverter.ToUInt16(byteData, offsetStart)));
                });
            DataActions.Add((byte)PropertyTypes.Int32,
                delegate(ResourceBlock metadataObject, byte id, byte[] byteData, int offsetStart, uint offsetLength)
                {
                    if (offsetLength == 0) metadataObject.SetAttribute(id, new Int32ValueStored(DefaultValue.Int32));
                    else metadataObject.SetAttribute(id, new Int32ValueStored(BitConverter.ToInt32(byteData, offsetStart)));
                });
            DataActions.Add((byte)PropertyTypes.UInt32,
                delegate(ResourceBlock metadataObject, byte id, byte[] byteData, int offsetStart, uint offsetLength)
                {
                    if (offsetLength == 0) metadataObject.SetAttribute(id, new UInt32ValueStored(DefaultValue.UInt32));
                    else metadataObject.SetAttribute(id, new UInt32ValueStored(BitConverter.ToUInt32(byteData, offsetStart)));
                });
            DataActions.Add((byte)PropertyTypes.Int64,
                delegate(ResourceBlock metadataObject, byte id, byte[] byteData, int offsetStart, uint offsetLength)
                {
                    if (offsetLength == 0) metadataObject.SetAttribute(id, new Int64ValueStored(DefaultValue.Int64));
                    else metadataObject.SetAttribute(id, new Int64ValueStored(BitConverter.ToInt64(byteData, offsetStart)));
                });
            DataActions.Add((byte)PropertyTypes.UInt64,
                delegate(ResourceBlock metadataObject, byte id, byte[] byteData, int offsetStart, uint offsetLength)
                {
                    if (offsetLength == 0) metadataObject.SetAttribute(id, new UInt64ValueStored(DefaultValue.UInt64));
                    else metadataObject.SetAttribute(id, new UInt64ValueStored(BitConverter.ToUInt64(byteData, offsetStart)));
                });
            DataActions.Add((byte)PropertyTypes.Float,
                delegate(ResourceBlock metadataObject, byte id, byte[] byteData, int offsetStart, uint offsetLength)
                {
                    if (offsetLength == 0) metadataObject.SetAttribute(id, new FloatValueStored(DefaultValue.Float));
                    else metadataObject.SetAttribute(id, new FloatValueStored(BitConverter.ToSingle(byteData, offsetStart)));
                });
            DataActions.Add((byte)PropertyTypes.Double,
                delegate(ResourceBlock metadataObject, byte id, byte[] byteData, int offsetStart, uint offsetLength)
                {
                    if (offsetLength == 0) metadataObject.SetAttribute(id, new DoubleValueStored(DefaultValue.Double));
                    else metadataObject.SetAttribute(id, new DoubleValueStored(BitConverter.ToDouble(byteData, offsetStart)));
                });
            DataActions.Add((byte)PropertyTypes.Decimal,
                delegate(ResourceBlock metadataObject, byte id, byte[] byteData, int offsetStart, uint offsetLength)
                {
                    if (offsetLength == 0) metadataObject.SetAttribute(id, new DecimalValueStored(DefaultValue.Decimal));
                    else
                    {
                        fixed (byte* pByte = (&byteData[offsetStart]))
                            metadataObject.SetAttribute(id, new DecimalValueStored(*(decimal*)pByte));
                    }
                });
            DataActions.Add((byte)PropertyTypes.String,
                delegate(ResourceBlock metadataObject, byte id, byte[] byteData, int offsetStart, uint offsetLength)
                {
                    if (offsetLength == 0) metadataObject.SetAttribute(id, new StringValueStored(null));
                    else metadataObject.SetAttribute(id, new StringValueStored(Encoding.UTF8.GetString(byteData, offsetStart, (int)offsetLength)));
                });
            DataActions.Add((byte)PropertyTypes.DateTime,
                delegate(ResourceBlock metadataObject, byte id, byte[] byteData, int offsetStart, uint offsetLength)
                {
                    if (offsetLength == 0) metadataObject.SetAttribute(id, new DateTimeValueStored(DefaultValue.DateTime));
                    else metadataObject.SetAttribute(id, new DateTimeValueStored(new DateTime(BitConverter.ToInt64(byteData, offsetStart))));
                });
            DataActions.Add((byte)PropertyTypes.Guid,
                delegate(ResourceBlock metadataObject, byte id, byte[] byteData, int offsetStart, uint offsetLength)
                {
                    if (offsetLength == 0) metadataObject.SetAttribute(id, new GuidValueStored(DefaultValue.Guid));
                    else
                    {
                        fixed (byte* pByte = (&byteData[offsetStart]))
                            metadataObject.SetAttribute(id, new GuidValueStored(*(Guid*)pByte));
                    }
                });
            DataActions.Add((byte)PropertyTypes.IPEndPoint,
                delegate(ResourceBlock metadataObject, byte id, byte[] byteData, int offsetStart, uint offsetLength)
                {
                    fixed (byte* pData = (&byteData[offsetStart]))
                    {
                        IPEndPoint iep = new IPEndPoint(new IPAddress(*(long*)pData), *(int*)(pData + 8));
                        metadataObject.SetAttribute(id, new IPEndPointValueStored(iep));
                    }
                });
            DataActions.Add((byte)PropertyTypes.IntPtr,
                delegate(ResourceBlock metadataObject, byte id, byte[] byteData, int offsetStart, uint offsetLength)
                {
                    if (offsetLength == 0) metadataObject.SetAttribute(id, new IntPtrValueStored(DefaultValue.IntPtr));
                    else metadataObject.SetAttribute(id, new IntPtrValueStored(new IntPtr(BitConverter.ToInt32(byteData, offsetStart))));
                });
            DataActions.Add((byte)PropertyTypes.ResourceBlock,
                delegate(ResourceBlock metadataObject, byte id, byte[] byteData, int offsetStart, uint offsetLength)
                {
                    metadataObject.SetAttribute(id, new ResourceBlockStored(MetadataObjectEngine.GetObject(byteData, (uint)offsetStart, offsetLength)));
                });
            DataActions.Add((byte)PropertyTypes.TimeSpan,
                delegate(ResourceBlock metadataObject, byte id, byte[] byteData, int offsetStart, uint offsetLength)
                {
                    if (offsetLength == 0) metadataObject.SetAttribute(id, new TimeSpanValueStored(DefaultValue.TimeSpan));
                    else
                    {
                        fixed (byte* pData = (&byteData[offsetStart]))
                            metadataObject.SetAttribute(id, new TimeSpanValueStored(new TimeSpan(*(long*)pData)));
                    }
                });
            DataActions.Add((byte)PropertyTypes.BitFlag,
                delegate(ResourceBlock metadataObject, byte id, byte[] byteData, int offsetStart, uint offsetLength)
                {
                    metadataObject.SetAttribute(id, new BitFlagValueStored(new BitFlag(byteData[offsetStart])));
                });
            DataActions.Add((byte)PropertyTypes.Blob,
                delegate(ResourceBlock metadataObject, byte id, byte[] byteData, int offsetStart, uint offsetLength)
                {
                    metadataObject.SetAttribute(id, new BlobValueStored(new Blob(byteData, offsetStart, (int)offsetLength)));
                });
            DataActions.Add((byte)PropertyTypes.Null,
                delegate(ResourceBlock metadataObject, byte id, byte[] byteData, int offsetStart, uint offsetLength)
                {
                    metadataObject.SetAttribute(id, new NullValueStored());
                });
        }
  
        #endregion
    }
}
