using System;
using System.Collections.Generic;
using KJFramework.Messages.Configuration;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Enums;
using KJFramework.Messages.Proxies;
using KJFramework.Messages.Structs;
using KJFramework.Messages.Types;
using KJFramework.Messages.ValueStored.DataProcessor.Mapping;
using KJFramework.Tracing;
using KJFramework.Messages.ValueStored;

namespace KJFramework.Messages.Engine
{
    /// <summary>
    ///     元数据对象引擎，提供了相关的基本操作。
    /// </summary>
    public sealed class MetadataObjectEngine
    {
        #region Members

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(IntellectObjectEngine));

        #endregion

        #region Method

        /// <summary>
        ///     第三方数据转换成元数据  
        /// </summary>
        /// <param name="metadata">第三方数据对象</param>
        /// <returns>元数据的二进制表现形式</returns>
        public static byte[] ToBytes(ResourceBlock metadata)
        {
            if (metadata == null) return null;
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = null;
            try
            {
                proxy = MemorySegmentProxyFactory.Create();
                ToBytes(metadata, proxy);
                return proxy.GetBytes();
            }
            catch
            {
                if (proxy != null) proxy.Dispose();
                throw;
            }
        }

        /// <summary>
        ///     将一个对象字段转换为二进制元数据
        /// </summary>
        /// <param name="metadata">需要转换成元数据的Value字典</param>
        /// <param name="proxy">内存段代理器</param>
        internal unsafe static void ToBytes(ResourceBlock metadata, IMemorySegmentProxy proxy)
        {
            Dictionary<byte, BaseValueStored> valueStoreds = metadata.GetMetaDataDictionary();
            uint markRangeLength = (uint) (valueStoreds.Count*5);
            MemoryPosition wrapperStartPosition = proxy.GetPosition();
            proxy.Skip(4U);
            proxy.WriteUInt16((ushort) (valueStoreds.Count));
            MemoryPosition wrapperMarkRangeStartPosition = proxy.GetPosition();
            proxy.Skip(markRangeLength);
            MemoryPosition wrapperOffsetStartPosition = proxy.GetPosition();
            MarkRange* markRange = stackalloc MarkRange[valueStoreds.Count];
            int i = 0;
            foreach (KeyValuePair<byte, BaseValueStored> valueStored in valueStoreds)
            {
                //转化成二进制数组的形式
                MemoryPosition wrapperCurrentPosition = proxy.GetPosition();
                uint currentOffset = (uint)MemoryPosition.CalcLength(proxy.SegmentCount, wrapperOffsetStartPosition, wrapperCurrentPosition);
                (markRange + i)->Id = valueStored.Key;
                (markRange + i++)->Flag = (uint)((currentOffset << 8) | (int)valueStored.Value.TypeId);
                if (valueStored.Value.IsNull) continue;
                valueStored.Value.ToBytes(proxy);
            }
            MemoryPosition wrapperEndPosition = proxy.GetPosition();
            //回写mark Range
            proxy.WriteBackMemory(wrapperMarkRangeStartPosition, markRange, markRangeLength);
            //回写4bytes总长
            proxy.WriteBackInt32(wrapperStartPosition, MemoryPosition.CalcLength(proxy.SegmentCount, wrapperStartPosition, wrapperEndPosition));
        }

        /// <summary>
        ///     将一个二进制元数据转换为对象字段
        /// </summary>
        /// <param name="byteData">需要反序列化的二进制数组</param>
        /// <param name="offset">内存段偏移</param>
        /// <param name="length">内存段长度</param>
        public static MetadataContainer GetObject(byte[] byteData, uint offset, uint length)
        {
            MetadataContainer metadataObject = new MetadataContainer();

            #region 解析数据部分

            //记录当前解析数据段的绝对偏移起始位置
            uint offsetFlag = offset;
            //计算当前解析数据段的总长度
            uint totalLength = BitConverter.ToUInt32(byteData, (int)offset);
            if (byteData.Length - offset < length - 4) throw new System.Exception("Illegal binary data length! #length: " + totalLength);
            //计算当前解析数据段的markRange个数（即包含元素的个数）
            //offset+4表示计算完后偏移4bytes（当前数据段总长所占的4Bytes）
            ushort markRangeCount = BitConverter.ToUInt16(byteData, (int)(offset += 4));
            //计算当前解析数据段markRange所占总字节数
            uint markRangeLength = (uint) (markRangeCount*5);
            //offset+2表示偏移2bytes（当前数据段mark Range个数所占的2Bytes）
            offset += 2;
            MarkRange markRange1 = new MarkRange(), markRange2 = new MarkRange();
            for (int i = 0; i < markRangeCount; i++) 
            {             
                uint offsetFlagStart, offsetFlagEnd;
                //解析mark Range
                //提取当前数据段前一个元素的ID
                markRange1.Id = (i == 0) ? byteData[offset + 5 * i] : markRange2.Id;
                //提取当前数据段前一个元素的Flag
                markRange1.Flag = (i == 0) ? BitConverter.ToUInt32(byteData, (int)(offset + 5 * i + 1)) : markRange2.Flag;
                //提取当前数据段后一个元素的ID
                markRange2.Id = (i == markRangeCount - 1) ? (byte)0 : byteData[offset + 5 * (i + 1)];
                //提取当前数据段后一个元素的Flag
                markRange2.Flag = (i == markRangeCount - 1) ? 0 : BitConverter.ToUInt32(byteData, (int)(offset + 5 * (i + 1) + 1));
                //计算偏移
                //当前解析数据段某一个元素的相对偏移起始位置（相对于当前的mark range）
                offsetFlagStart = (markRange1.Flag & 0xFFFFFF00) >> 8;
                //当前解析数据段某一个元素的相对偏移结束位置（相对于当前的mark range）
                if (i < markRangeCount - 1) offsetFlagEnd = (markRange2.Flag & 0xFFFFFF00) >> 8;
                else offsetFlagEnd = totalLength - 6 - markRangeLength;
                //提取当前解析数据段某一个元素的类型值
                PropertyTypes idFlag = (PropertyTypes) byteData[offset + 5 * i + 1];
                //计算当前解析数据段某一个元素的绝对偏移起始位置(整个完整数据段的偏移)
                uint offsetStart = offsetFlagStart + 6 + markRangeLength + offsetFlag;
                //计算当前解析数据段某一个元素的长度（所占字节数）
                uint offsetLength = offsetFlagEnd - offsetFlagStart;
                //开始解析........................................................................
                //系统内置类型
                if ((byte) idFlag <= MetadataObjectSetting.TYPE_BOUNDARY)
                {
                    BaseValueStored valueStored =  (BaseValueStored) SystemTypeMapping.GetValueStored((byte) idFlag).Clone();
                    valueStored.ToData(metadataObject, markRange1.Id, byteData, (int)offsetStart, offsetLength);
                }
                //用户自定义类型
                else
                {
                    BaseValueStored valueStored = (BaseValueStored) ExtensionTypeMapping.GetValueStored((byte) idFlag).Clone();
                    valueStored.ToData(metadataObject, markRange1.Id, byteData, (int)offsetStart, offsetLength);
                }
            }

            #endregion

            return metadataObject;
        }

        #endregion
    }
}
