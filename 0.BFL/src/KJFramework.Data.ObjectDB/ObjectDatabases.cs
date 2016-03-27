using System;
using System.IO;

namespace KJFramework.Data.ObjectDB
{
    /// <summary>
    ///     对象数据库静态类
    /// </summary>
    public class ObjectDatabases
    {
        #region Methods

        /// <summary>
        ///     打开一个已存在的对象数据库文件
        ///     <para>* 如果目标文件不存在，将会自动创建一个新文件</para>
        /// </summary>
        /// <param name="filename">对象数据库全路径地址</param>
        /// <returns>返回对象数据库实例</returns>
        /// <exception cref="System.ArgumentNullException">参数不能为空</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录不存在</exception>
        public static IObjectDatabase Open(string filename)
        {
            if (string.IsNullOrEmpty(filename)) throw new ArgumentNullException("filename");
            if(!Directory.Exists(Path.GetFullPath(filename))) throw new DirectoryNotFoundException(Path.GetFullPath(filename));
            return new LocalObjectDatabase(filename);
        }

        #endregion
    }
}