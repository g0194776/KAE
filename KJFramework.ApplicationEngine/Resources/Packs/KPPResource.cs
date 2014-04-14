using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.Checksums;
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
        #region Constructor.

        /// <summary>
        ///    KPP资源打包/解包器
        /// </summary>
        static KPPResource()
        {
            _sections = new Dictionary<byte, Type>();
            _sections.Add(0x00, typeof(PackageAttributeDataSection));
        }

        #endregion

        #region Members.

        private static readonly Dictionary<byte, Type> _sections; 

        #endregion

        #region Methods.

        /// <summary>
        ///    将一个目录下的所有文件打包为一个KPP资源包
        /// </summary>
        /// <param name="folderPath">资源路径</param>
        /// <param name="kppDestinationFilePath">要存放的KPP资源文件地址</param>
        /// <param name="head">KPP资源文件头</param>
        /// <param name="sections">KPP数据节集合</param>
        /// <exception cref="FolderNotFoundException">目标文件夹不存在</exception>
        public static void Pack(string folderPath, string kppDestinationFilePath, KPPDataHead head, params IKPPDataResource[] sections)
        {
            if (!Directory.Exists(folderPath)) throw new FolderNotFoundException("#Target folder path couldn't be find.");
            if (File.Exists(kppDestinationFilePath)) File.Delete(kppDestinationFilePath);

            //handles ZIP stream.
            List<FileInfo> files = Directory.GetFiles(folderPath).Select(f => new FileInfo(f)).ToList();
            byte[] data = FileCompression.CompressFile(files, 9, false);
            Crc32 crc = new Crc32();
            crc.Reset();
            crc.Update(data);
            //sets ref value.
            head.SetField("TotalSize", (ulong)data.Length);
            head.SetField("SectionCount", (ushort)sections.Length);
            head.SetField("CRC", crc.Value);
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
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="System.IO.FileNotFoundException">目标文件不存在</exception>
        /// <exception cref="BadImageFormatException">错误的KPP资源包格式</exception>
        /// <exception cref="UnSupportedSectionTypeException">不支持的数据节类型</exception>
        public static KPPDataStructure UnPack(string kppDestinationFilePath)
        {
            if (string.IsNullOrEmpty(kppDestinationFilePath)) throw new ArgumentNullException("kppDestinationFilePath");
            if (!File.Exists(kppDestinationFilePath)) throw new System.IO.FileNotFoundException("#Current file cannot be find, file: " + kppDestinationFilePath);
            byte[] data = File.ReadAllBytes(kppDestinationFilePath);
            using (MemoryStream stream = new MemoryStream(data))
            {
                KPPDataHead head = new KPPDataHead();
                head.UnPack(stream);
                ushort sectionCount = head.GetField<ushort>("SectionCount");
                Dictionary<byte, IKPPDataResource> sections = new Dictionary<byte, IKPPDataResource>();
                for (int i = 0; i < sectionCount; i++)
                {
                    byte sectionId = (byte) stream.ReadByte();
                    //reset position.
                    stream.Position = stream.Position - 1;
                    Type sectionType;
                    if (!_sections.TryGetValue(sectionId, out sectionType)) throw new UnSupportedSectionTypeException("#Current data section type cannot be supported. #id: " + sectionId);
                    IKPPDataResource section = (IKPPDataResource) sectionType.Assembly.CreateInstance(sectionType.FullName);
                    section.UnPack(stream);
                    sections.Add(sectionId, section);
                }
                byte[] zipData = new byte[data.Length - stream.Position];
                Buffer.BlockCopy(data, (int) stream.Position, zipData, 0, zipData.Length);
                Crc32 crc = new Crc32();
                crc.Reset();
                crc.Update(zipData);
                if(head.GetField<long>("CRC") != crc.Value) throw new BadImageFormatException("#Bad image format of zipped stream CRC.");
                IDictionary<string, byte[]> files = FileCompression.Decompress(zipData);
                return new KPPDataStructure(head, sections, files);
            }
        }

        #endregion
    }
}