using KJFramework.ApplicationEngine.Resources;
using KJFramework.ApplicationEngine.Resources.Packs;
using KJFramework.Dynamic.Finders;
using KJFramework.Dynamic.Structs;
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
    internal class ApplicationFinder
    {
        #region Members

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(BasicDynamicDomainComponentFinder));

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        ///     查找一个路径下所有的动态程序域组件
        /// </summary>
        /// <param name="path">查找路径</param>
        /// <returns>返回程序域组件入口点信息集合</returns>
        public static IDictionary<string, IList<Tuple<DomainComponentEntryInfo, KPPDataStructure>>> Search(string path)
        {
            IDictionary<string, IList<Tuple<DomainComponentEntryInfo, KPPDataStructure>>> result = new Dictionary<string, IList<Tuple<DomainComponentEntryInfo, KPPDataStructure>>>();
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
                        string targetPath = Path.Combine(Path.GetDirectoryName(file), string.Format("temp_files_{0}", DateTime.Now.Date.ToString("yyyy-MM-dd")));
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
                                    DomainComponentEntryInfo info = new DomainComponentEntryInfo();
                                    info.FilePath = Path.GetFullPath(mainFilePath);
                                    info.FolderPath = Path.GetFullPath(targetPath);
                                    info.EntryPoint = type.FullName;
                                    IList<Tuple<DomainComponentEntryInfo, KPPDataStructure>> tuple;
                                    if(!result.TryGetValue(dataStructure.GetSectionField<string>(0x00, "PackName"), out tuple))
                                    {
                                        tuple = new List<Tuple<DomainComponentEntryInfo, KPPDataStructure>>();
                                        result.Add(dataStructure.GetSectionField<string>(0x00, "PackName"), tuple);
                                    }
                                    tuple.Add(new Tuple<DomainComponentEntryInfo, KPPDataStructure>(info, dataStructure));
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

        public void Dispose()
        {
        }

        #endregion
    }
}