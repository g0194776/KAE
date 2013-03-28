namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     根节点类型存储位移的数据块
    /// </summary>
    internal unsafe struct StorePositionData
    {
        #region Members

        /// <summary>
        ///     内部的结构以每5个字节为一个结构进行存储，最大可以存储Global.MaxDataStoreVolume个数据块
        ///     <para>* 内部结构为{FileId(1 byte) + StartPageId(2 bytes) + PageCount(2 bytes)}</para>
        /// </summary>
        private fixed byte _data[(Global.MaxDataStoreVolume * 5) + 1];

        /// <summary>
        ///     获取内部保存的位置信息个数
        /// </summary>
        public byte Count
        {
            get
            {
                fixed (byte* pByte = _data)
                    return *pByte;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     根据指定的文件编号获取一个位置信息数据
        /// </summary>
        /// <param name="fileId">文件编号</param>
        /// <returns>如果能找到，则返回位置信息，否则返回null</returns>
        public PositionData? GetPositionDataByFileId(byte fileId)
        {
            int offset = 1, count = Count;
            fixed (byte* pByte = _data)
            {
                for (int i = 0; i < count; i++)
                {
                    PositionData* data = (PositionData*)(pByte + offset);
                    if (data->FileId == fileId) return *data;
                    offset += sizeof(PositionData);
                }
                return null;
            }
        }

        /// <summary>
        ///     根据指定的文件编号获取一个位置信息数据
        /// </summary>
        /// <param name="fileId">文件编号</param>
        /// <param name="currentOffset">如果能找到指定条件的位置信息，则此字段返回的是当前位置信息所在的内存偏移地址</param>
        /// <returns>如果能找到，则返回位置信息，否则返回null</returns>
        public PositionData? GetPositionDataByFileId(byte fileId, out int currentOffset)
        {
            int offset = 1, count = Count;
            fixed (byte* pByte = _data)
            {
                for (int i = 0; i < count; i++)
                {
                    PositionData* data = (PositionData*)(pByte + offset);
                    if (data->FileId == fileId)
                    {
                        currentOffset = offset;
                        return *data;
                    }
                    offset += sizeof(PositionData);
                }
                currentOffset = -1;
                return null;
            }
        }

        /// <summary>
        ///     更新指定的位置信息
        /// </summary>
        /// <param name="data">要更新的位置信息</param>
        /// <returns>返回更新后的结果</returns>
        public bool Update(PositionData data)
        {
            int offset;
            PositionData? positionData = GetPositionDataByFileId(data.FileId, out offset);
            if (positionData == null) return false;
            fixed (byte* pByte = _data)
                *(PositionData*)(pByte + offset) = data;
            return true;
        }

        /// <summary>
        ///     添加一个位置信息
        /// </summary>
        /// <param name="data">位置信息</param>
        /// <returns>返回添加后的结果</returns>
        public bool Add(PositionData data)
        {
            int count = Count;
            if (count >= Global.MaxDataStoreVolume) return false;
            fixed (byte* pByte = _data)
            {
                //set value.
                *(PositionData*) (pByte + 1 + count*sizeof (PositionData)) = data;
                //increment count.
                *pByte = (byte)(*pByte + 0x01);
                return true;
            }
        }

        #endregion
    }
}