using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using KJFramework.Dynamic.Components;
using KJFramework.Dynamic.Structs;
using KJFramework.Logger;

namespace KJFramework.Dynamic.Finders
{
    /// <summary>
    ///     �����Ķ�̬������������������ṩ����صĻ���������
    /// </summary>
    public class BasicDynamicDomainComponentFinder : IDynamicDomainComponentFinder
    {
        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     ����һ��·�������еĶ�̬���������
        /// </summary>
        /// <param name="path">����·��</param>
        /// <returns>���س����������ڵ���Ϣ����</returns>
        public List<DomainComponentEntryInfo> Execute(string path)
        {
            if (String.IsNullOrEmpty(path) || !Directory.Exists(path))
            {
                throw new System.Exception("�Ƿ��ļ���·����#path: " + path);
            }
            List<DomainComponentEntryInfo> result = new List<DomainComponentEntryInfo>();
            String[] files = Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories);
            if (files != null && files.Length > 0)
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
                                //�ҵ���ڵ�
                                if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(DynamicDomainComponent)))
                                {
                                    DomainComponentEntryInfo info = new DomainComponentEntryInfo();
                                    info.FilePath = file;
                                    info.FolderPath = file.Substring(0, file.LastIndexOf("\\") + 1);
                                    info.EntryPoint = type.FullName;
                                    result.Add(info);
                                }
                            }
                            catch (System.Exception ex)
                            {
                                Logs.Logger.Log(ex);
                                continue;
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Logs.Logger.Log(ex);
                        continue;
                    }
                }
                return result;
            }
            return result;
        }

        #endregion
    }
}