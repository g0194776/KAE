using KJFramework.Dynamic.Components;
using KJFramework.Dynamic.Structs;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace KJFramework.Dynamic.Finders
{
    /// <summary>
    ///     基础的动态程序域组件查找器，提供了相关的基本操作。
    /// </summary>
    public class BasicDynamicDomainComponentFinder : IDynamicDomainComponentFinder
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
        public List<DomainComponentEntryInfo> Execute(string path)
        {
            if (String.IsNullOrEmpty(path) || !Directory.Exists(path))
            {
                throw new System.Exception("非法的加载路径。#path: " + path);
            }
            List<DomainComponentEntryInfo> result = new List<DomainComponentEntryInfo>();
            String[] files = Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories);
            if (files.Length > 0)
            {
                foreach (string file in files)
                {
                    try
                    {
                        Assembly assembly = Assembly.Load(File.ReadAllBytes(file));
                        Type[] types = assembly.GetTypes();
                        if (types.Length == 0) continue;
                        foreach (Type type in types)
                        {
                            try
                            {
                                //找到入口点
                                if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(DynamicDomainComponent)))
                                {
                                    DomainComponentEntryInfo info = new DomainComponentEntryInfo();
                                    info.FilePath = file;
                                    info.FolderPath = file.Substring(0, file.LastIndexOf("\\") + 1);
                                    info.EntryPoint = type.FullName;
                                    result.Add(info);
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