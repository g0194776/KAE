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

        private static IList<string> _frameworkFiles = new[]
        {
            "kjframework.dll", 
            "kjframework.pdb", 
            "kjframework.xml", 
            "kjframework.cache.dll", 
            "kjframework.cache.pdb", 
            "kjframework.cache.xml", 
            "kjframework.data.objectdb.dll", 
            "kjframework.data.objectdb.pdb", 
            "kjframework.data.objectdb.xml", 
            "kjframework.dynamic.dll",
            "kjframework.dynamic.pdb",
            "kjframework.dynamic.xml",
            "kjframework.net.dll",
            "kjframework.net.pdb",
            "kjframework.net.xml",
            "kjframework.net.channels.dll", 
            "kjframework.net.channels.pdb", 
            "kjframework.net.channels.xml", 
            "kjframework.net.cloud.dll", 
            "kjframework.net.cloud.pdb", 
            "kjframework.net.cloud.xml", 
            "kjframework.messages.dll",
            "kjframework.messages.pdb",
            "kjframework.messages.xml",
            "kjframework.data.synchronization.dll", 
            "kjframework.data.synchronization.pdb", 
            "kjframework.data.synchronization.xml", 
            "kjframework.servicemodel.dll", 
            "kjframework.servicemodel.pdb", 
            "kjframework.servicemodel.xml", 
            "kjframework.test.loadrunner.dll",
            "kjframework.test.loadrunner.pdb",
            "kjframework.test.loadrunner.xml",
            "kjframework.net.transaction.dll", 
            "kjframework.net.transaction.pdb", 
            "kjframework.net.transaction.xml", 
            "kjframework.applicationengine.dll",
            "kjframework.applicationengine.pdb",
            "kjframework.applicationengine.xml",
            "icsharpcode.sharpziplib.dll",
            "icsharpcode.sharpziplib.pdb",
            "icsharpcode.sharpziplib.xml",
            "kjframework.platform.deploy.csn.networklayer.dll",
            "kjframework.platform.deploy.csn.networklayer.pdb",
            "kjframework.platform.deploy.csn.networklayer.xml",
            "kjframework.platform.deploy.csn.protocolstack.dll",
            "kjframework.platform.deploy.csn.protocolstack.pdb",
            "kjframework.platform.deploy.csn.protocolstack.xml",
            "kjframework.platform.deploy.metadata.dll",
            "kjframework.platform.deploy.metadata.pdb",
            "kjframework.platform.deploy.metadata.xml",
            "kjframework.servicemodel.bussiness.default.dll",
            "kjframework.servicemodel.bussiness.default.pdb",
            "kjframework.servicemodel.bussiness.default.xml",
            "kjframework.servicemodel.core.dll",
            "kjframework.servicemodel.core.pdb",
            "kjframework.servicemodel.core.xml"
        };

        #endregion

        #region Methods.

        /// <summary>
        ///    将一个目录下的所有文件打包为一个KPP资源包
        /// </summary>
        /// <param name="folderPath">资源路径</param>
        /// <param name="kppDestinationFilePath">要存放的KPP资源文件地址</param>
        /// <param name="head">KPP资源文件头</param>
        /// <param name="isCompletedEnvironment">一个值，标示了当前要封装的包裹是否按照完整运行时所需要的所有依赖文件来包装</param>
        /// <param name="sections">KPP数据节集合</param>
        /// <exception cref="FolderNotFoundException">目标文件夹不存在</exception>
        public static void Pack(string folderPath, string kppDestinationFilePath, KPPDataHead head, bool isCompletedEnvironment = true, params IKPPDataResource[] sections)
        {
            if (!Directory.Exists(folderPath)) throw new FolderNotFoundException("#Target folder path couldn't be find.");
            if (File.Exists(kppDestinationFilePath)) File.Delete(kppDestinationFilePath);
            #region Procedure of handling non completed runtime package.

            if (!isCompletedEnvironment)
            {
                string[] tempFiles = Directory.GetFiles(folderPath);
                foreach (string file in tempFiles)
                {
                    string lowerCaseFile = file.ToLower();
                    foreach (string globalFile in _frameworkFiles)
                    {
                        if (lowerCaseFile.Contains(globalFile))
                        {
                            File.Delete(file);
                            break;
                        }
                    }
                }
            }

            #endregion
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