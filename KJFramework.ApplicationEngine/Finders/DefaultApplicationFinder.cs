using ICSharpCode.SharpZipLib.Checksums;
using KJFramework.ApplicationEngine.Objects;
using KJFramework.ApplicationEngine.Resources;
using KJFramework.ApplicationEngine.Resources.Packs;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

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
            String[] files = Directory.GetFiles(path, "*.kpp", SearchOption.AllDirectories);
            if (files.Length > 0)
            {
                foreach (string file in files)
                {
                    try
                    {
                        //analyzes kpp package.
                        KPPDataStructure dataStructure = KPPResource.UnPack(file);
                        if(dataStructure == null) continue;
                        string targetPath = Path.Combine(Path.GetDirectoryName(file), string.Format("tempfiles_{0}_{1}_{2}", Path.GetFileName(file), dataStructure.GetSectionField<string>(0x00, "Version"), DateTime.Now.Date.ToString("yyyy-MM-dd")));
                        //releases files from kpp package.
                        dataStructure.ReleaseFiles(targetPath);
                        //picks up main file data.
                        string mainFilePath = Path.Combine(targetPath, dataStructure.GetSectionField<string>(0x00, "ApplicationMainFileName"));
                        Assembly assembly = Assembly.Load(File.ReadAllBytes(mainFilePath));
                        Type[] types = assembly.GetTypes();
                        if (types.Length == 0) continue;
                        foreach (Type type in types)
                        {
                            try
                            {
                                //找到入口点
                                if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(Application)))
                                {
                                    ApplicationEntryInfo info = new ApplicationEntryInfo();
                                    info.FilePath = Path.GetFullPath(mainFilePath);
                                    info.FolderPath = Path.GetFullPath(targetPath);
                                    info.EntryPoint = type.FullName;
                                    byte[] data = File.ReadAllBytes(file);
                                    Crc32 crc = new Crc32();
                                    crc.Reset();
                                    crc.Update(data);
                                    info.FileCRC = crc.Value;
                                    //info.FileCRC = 
                                    IList<Tuple<ApplicationEntryInfo, KPPDataStructure>> tuple;
                                    if(!result.TryGetValue(dataStructure.GetSectionField<string>(0x00, "PackName"), out tuple))
                                    {
                                        tuple = new List<Tuple<ApplicationEntryInfo, KPPDataStructure>>();
                                        result.Add(dataStructure.GetSectionField<string>(0x00, "PackName"), tuple);
                                    }
                                    tuple.Add(new Tuple<ApplicationEntryInfo, KPPDataStructure>(info, dataStructure));
                                }
                            }
                            catch (ReflectionTypeLoadException) { }
                            catch (System.Exception ex) { _tracing.Error(ex, null); }
                        }
                    }
                    catch (ReflectionTypeLoadException) { }
                    catch (System.Exception ex) { _tracing.Error(ex, null); }
                }
                return result;
            }
            return result;
        }

        #endregion
    }
}