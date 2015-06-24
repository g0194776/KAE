using ICSharpCode.SharpZipLib.Checksums;
using KJFramework.ApplicationEngine.Objects;
using KJFramework.ApplicationEngine.Resources;
using KJFramework.ApplicationEngine.Resources.Packs;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using KJFramework.ApplicationEngine.Eums;

namespace KJFramework.ApplicationEngine.Finders
{
    /// <summary>
    ///    KAE应用寻找器
    /// </summary>
    internal sealed class DefaultApplicationFinder : IApplicationFinder
    {
        #region Members

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(DefaultApplicationFinder));

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        ///    从一个指定的路径中寻找合法的KPP包
        ///     <para>* Dic.Key = PackName</para>
        /// </summary>
        /// <param name="path">寻找的路径</param>
        /// <returns>返回找到的KPP资源集合</returns>
        public IDictionary<string, IList<Tuple<ApplicationEntryInfo, KPPDataStructure>>> Search(string path)
        {
            IDictionary<string, IList<Tuple<ApplicationEntryInfo, KPPDataStructure>>> result = new Dictionary<string, IList<Tuple<ApplicationEntryInfo, KPPDataStructure>>>();
            string[] files = Directory.GetFiles(path, "*.kpp", SearchOption.AllDirectories);
            if (files.Length > 0)
            {
                foreach (string file in files)
                {
                    IList<Tuple<ApplicationEntryInfo, KPPDataStructure>> tuple;
                    Tuple<string, ApplicationEntryInfo, KPPDataStructure> tmpTuple = ReadKPPFrom(file);
                    if (tmpTuple == null) continue;
                    if (!result.TryGetValue(tmpTuple.Item1, out tuple))
                    {
                        tuple = new List<Tuple<ApplicationEntryInfo, KPPDataStructure>>();
                        result.Add(tmpTuple.Item1, tuple);
                    }
                    tuple.Add(new Tuple<ApplicationEntryInfo, KPPDataStructure>(tmpTuple.Item2, tmpTuple.Item3));
                }
                return result;
            }
            return result;
        }

        /// <summary>
        ///     读取通知KAE APP所在地址的文件，并将其进行解析
        /// </summary>
        /// <param name="fileFullPath">KAE APP文件的完整路径地址</param>
        /// <returns>返回解析后的KAE APP信息</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="FileNotFoundException">目标文件不存在</exception>
        public Tuple<string, ApplicationEntryInfo, KPPDataStructure> ReadKPPFrom(string fileFullPath)
        {
            if (string.IsNullOrEmpty(fileFullPath)) throw new ArgumentNullException("fileFullPath");
            if (!File.Exists(fileFullPath)) throw new FileNotFoundException(fileFullPath);
            try
            {
                //analyzes kpp package.
                KPPDataStructure dataStructure = KPPResource.UnPack(fileFullPath);
                if (dataStructure == null) return null;
                string targetPath = Path.Combine(Path.GetDirectoryName(fileFullPath), string.Format("..\\..\\temp-files\\apps\\tempfiles_{0}_{1}_{2}", Path.GetFileName(fileFullPath), dataStructure.GetSectionField<string>(0x00, "Version"), (ApplicationLevel) dataStructure.GetSectionField<byte>(0x00, "ApplicationLevel")));
                //releases files from kpp package.
                dataStructure.ReleaseFiles(targetPath);
                //picks up main file data.
                string mainFilePath = Path.Combine(targetPath, dataStructure.GetSectionField<string>(0x00, "ApplicationMainFileName"));
                //updating combined full path directory.
                dataStructure.SetSectionField(0x00, "ApplicationMainFileName", mainFilePath);
                Assembly assembly = Assembly.Load(File.ReadAllBytes(mainFilePath));
                Type[] types = assembly.GetTypes();
                if (types.Length == 0) return null;
                Type targetAppType = null;
                foreach (Type type in types)
                {
                    try
                    {
                        //找到入口点
                        if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof (Application)))
                        {
                            targetAppType = type;
                            break;
                        }
                    }
                    catch (ReflectionTypeLoadException) { }
                    catch (System.Exception ex) {  _tracing.Error(ex, null); }
                }
                //use default application type when current package had missed the main application class.
                targetAppType = targetAppType ?? typeof (Application);
                ApplicationEntryInfo info = new ApplicationEntryInfo
                {
                    FilePath = Path.GetFullPath(mainFilePath),
                    FolderPath = Path.GetFullPath(targetPath),
                    EntryPoint = targetAppType.FullName
                };
                byte[] data = File.ReadAllBytes(fileFullPath);
                Crc32 crc = new Crc32();
                crc.Reset();
                crc.Update(data);
                info.FileCRC = crc.Value;
                return new Tuple<string, ApplicationEntryInfo, KPPDataStructure>(dataStructure.GetSectionField<string>(0x00, "PackName"), info, dataStructure);
            }
            catch (ReflectionTypeLoadException) { return null; }
            catch (System.Exception ex)  
            { 
                _tracing.Error(ex, null); 
                return null;
            }
        }

        #endregion
    }
}