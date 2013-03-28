using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using KJFramework.Data.ObjectDB.Structures;

namespace KJFramework.Data.ObjectDB.Helpers
{
    /// <summary>
    ///     数据库文件帮助器
    /// </summary>
    internal class DBFileHelper
    {
        #region Methods

        public static IndexTable CreateNew(string filename)
        {
            IndexTable indexTable = new IndexTable();
            using (FileStream stream = new FileStream(filename, FileMode.CreateNew))
            {
                byte[] data = GetDataWithFileHead(indexTable);
                stream.Write(data, 0, data.Length);
            }
            return indexTable;
        }

        public unsafe static byte[] GetDataWithFileHead(IIndexTable indexTable)
        {
            byte[] data = new byte[Global.HeaderBoundary];
            Buffer.BlockCopy(Global.FileHeadFlag, 0, data, 0, Global.FileHeadFlag.Length);
            int offset = 0;
            fixed (byte* pByte = &data[8])
            {
                *(ushort*) (pByte + offset)= indexTable.Flag.GetData();
                offset += 2;
                //Igrone authorization.
                *(uint*) (pByte + offset) = indexTable.UsedPageCounts;
                offset += 4;
                *(ushort*) (pByte + offset) = indexTable.UsedTokenCounts;
                offset += 2;
                //serialize token data.
                if(indexTable.UsedTokenCounts > 0)
                {
                    foreach (TypeToken token in indexTable.GetTokens())
                    {
                        *(TypeToken*) (pByte + offset) = token;
                        offset += sizeof(TypeToken);
                    }
                }
            }
            return data;
        }

        internal unsafe static IIndexTable ReadIndexTable(MemoryMappedFile mappedFile)
        {
            using (MemoryMappedViewAccessor accessor = mappedFile.CreateViewAccessor(0, Global.HeaderBoundary))
            {
                byte[] data = new byte[Global.HeaderBoundary];
                int readLen = accessor.ReadArray(0, data, 0, data.Length);
                int offset = 0;
                fixed (byte* pData = &data[8])
                {
                    FileFlag flags = new FileFlag(*(ushort*) pData);
                    offset += 2;
                    //Igrone authorization.
                    uint usedPageCounts = *(uint*)(pData + offset);
                    offset += 4;
                    ushort usedTokenCounts = *(ushort*)(pData + offset);
                    offset += 2;
                    IList<TypeToken> tokens = null;
                    if (usedTokenCounts > 0)
                    {
                        tokens = new List<TypeToken>(usedTokenCounts);
                        for (int i = 0; i < usedTokenCounts; i++)
                        {
                            tokens.Add(*(TypeToken*)(pData + offset));
                            offset += sizeof(TypeToken);
                        }
                    }
                    return new IndexTable(flags, tokens) {UsedPageCounts = usedPageCounts};
                }
            }
        }

        internal static void StoreIndexTable(MemoryMappedFile mappedFile, IIndexTable indexTable)
        {
            using (MemoryMappedViewStream stream = mappedFile.CreateViewStream(0, Global.HeaderBoundary))
            {
                byte[] data = GetDataWithFileHead(indexTable);
                stream.Write(data, 0, data.Length);
            }
        }

        #endregion
    }
}