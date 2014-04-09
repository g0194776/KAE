using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KJFramework.ApplicationEngine.Exceptions;
using KJFramework.ApplicationEngine.Helpers;
using KJFramework.ApplicationEngine.Resources.Packs.Sections;

namespace KJFramework.ApplicationEngine.Resources.Packs
{
    /// <summary>
    ///    KPP资源打包/解包器
    /// </summary>
    internal static class KPPResource
    {
        #region Methods.

        /// <summary>
        ///    将一个目录下的所有文件打包为一个KPP资源包
        /// </summary>
        /// <param name="folderPath">资源路径</param>
        /// <param name="kppDestinationFilePath">要存放的KPP资源文件地址</param>
        /// <param name="head">KPP资源文件头</param>
        /// <param name="sections">KPP数据节集合</param>
        /// <exception cref="FolderNotFoundException">目标文件夹不存在</exception>
        public static void Pack(string folderPath, string kppDestinationFilePath, KPPDataHead head, IKPPDataResource[] sections)
        {
            if (!Directory.Exists(folderPath)) throw new FolderNotFoundException("#Target folder path couldn't be find.");
            if (File.Exists(kppDestinationFilePath)) File.Delete(kppDestinationFilePath);

            //handles ZIP stream.
            List<FileInfo> files = Directory.GetFiles(folderPath).Select(f => new FileInfo(f)).ToList();
            byte[] data = FileCompression.CompressFile(files, 9, false);
            //sets ref value.
            head.SetField("TotalSize", (ulong)data.Length);
            head.SetField("SectionCount", (ushort)sections.Length);
            MemoryStream stream = new MemoryStream();
            head.Pack(stream);
            for (int i = 0; i < sections.Length; i++) sections[i].Pack(stream);
            stream.Write(data, 0, data.Length);
            using (FileStream fStream = new FileStream(kppDestinationFilePath, FileMode.CreateNew))
            {
                byte[] buffer = stream.GetBuffer();
                data = new byte[stream.Length];
                Buffer.BlockCopy(buffer, 0, data, 0, data.Length);
                fStream.Write(data, 0, data.Length);
            }
        }

        /// <summary>
        ///    将一个目录下的PP资源包进行解包
        /// </summary>
        /// <param name="kppDestinationFilePath">KPP资源文件路径</param>
        public static void UnPack(string kppDestinationFilePath)
        {
            
        }


        #endregion
    }
}