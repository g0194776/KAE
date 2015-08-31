using System;
using System.Collections.Generic;
using System.IO;
using KJFramework.Exceptions;

namespace KJFramework.Helpers
{
    /// <summary>
    ///     目录帮助器, 提供了相关的基本操作
    /// </summary>
    public class DirectoryHelper
    {
        /// <summary>
        ///     获取指定目录中具有指定后缀名的文件列表
        /// </summary>
        /// <param name="path" type="string">
        ///     <para>
        ///         寻找目录, 请在目录后面加上 "/"
        ///         比如: C:\Hook\
        ///     </para>
        /// </param>
        /// <param name="ext" type="string">
        ///     <para>
        ///         指定后缀名 : 比如 exe, dll 等等,请不要以"."开头
        ///         直接填写后缀名即可。
        ///         如果是 *.* 则等同返回该目录下的所有文件
        ///         其他非法后缀名会导致异常的抛出
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回null,表示当前目录中不存在具有指定后缀名的文件
        /// </returns>
        public static List<String> GetFiles(String path, String ext)
        {
            ext = ext.ToLower();
            //读取目录不存在
            if (!Directory.Exists(path))
            {
                throw new PluginPathNotFoundException();
            }
            String[] files = Directory.GetFiles(path);
            List<String> acceptFiles = null;
            foreach (String file in files)
            {
                String[] filestruct = file.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                if (filestruct.Length > 1)
                {
                    if (filestruct[filestruct.Length - 1].ToLower() == ext)
                    {
                        if (acceptFiles == null)
                        {
                            acceptFiles = new List<string>();
                        }
                        //添加检验合格的文件全路径
                        acceptFiles.Add(file);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
            }
            return acceptFiles == null ? null : acceptFiles;
        }
    }
}
