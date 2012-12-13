using KJFramework.Dynamic.Components;
using KJFramework.Dynamic.Structs;
using KJFramework.Tracing;
using System;
using System.IO;
using System.Reflection;

namespace KJFramework.Dynamic.Finders
{
    /// <summary>
    ///     基础的动态程序域服务查找器，提供了相关的基本操作。
    /// </summary>
    public class BasicDynamicDomainServiceFinder : IDynamicDomainServiceFinder
    {
        #region Members
        
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(BasicDynamicDomainServiceFinder));

	    #endregion

        #region Implementation of IDisposable

        /// <summary>
        ///     查找一个路径下所有的动态程序域组件
        /// </summary>
        /// <param name="path">查找路径</param>
        /// <returns>返回程序域组件入口点信息集合</returns>
        public DynamicDomainServiceInfo Execute(string path)
        {
            if (String.IsNullOrEmpty(path)) throw new ArgumentNullException("path");
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException("path");
            String[] files = Directory.GetFiles(path, "*.exe", SearchOption.AllDirectories);
            if (files.Length > 0)
            {
                foreach (string file in files)
                {
                    try
                    {
                        Assembly assembly = Assembly.LoadFile(file);
                        Type[] types = assembly.GetTypes();
                        if (types.Length == 0) continue;
                        foreach (Type type in types)
                        {
                            try
                            {
                                //找到入口点
                                if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(DynamicDomainService)))
                                {
                                    DynamicDomainServiceInfo info = new DynamicDomainServiceInfo();
                                    info.FilePath = file;
                                    info.DirectoryPath = file.Substring(0, file.LastIndexOf("\\") + 1);
                                    info.Service = (IDynamicDomainService)Activator.CreateInstance(type);
                                    return info;
                                }
                            }
                            catch (System.Exception ex)
                            {
                                _tracing.Error(ex, null);
                                continue;
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        _tracing.Error(ex, null);
                        continue;
                    }
                }
            }
            return new DynamicDomainServiceInfo();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}