using System;
using System.Collections;
using System.Drawing;
using System.Resources;

namespace KJFramework.Helpers
{
    /// <summary>
    ///     资源帮助器, 提供了相关的基本操作。
    /// </summary>
    public class ResourceHelper
    {
        /// <summary>
        ///     将指定图形文件集合写入资源文件 - [不支持资源追加]
        /// </summary>
        /// <param name="filePath" type="string">
        ///     <para>
        ///         要写入的文件名称
        ///     </para>
        /// </param>
        /// <param name="resourceInfo" type="UMFramework.Resource.ResourceHelper.ImageResourceCotent[]">
        ///     <para>
        ///         要写入的图形资源集合
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回写入的状态
        /// </returns>
        public static bool WriterImageResourceToFile(String filePath, ImageResourceCotent[] resourceInfo)
        {
            try
            {
                using (ResXResourceWriter rw = new ResXResourceWriter(filePath))
                {
                    for (int i = 0; i < resourceInfo.Length; i++)
                    {
                        rw.AddResource(resourceInfo[i].ResourceKey, Image.FromFile(resourceInfo[i].ResourcePath));
                    }
                    rw.Generate();
                    return true;
                }
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     提取指定文件中的图形资源
        /// </summary>
        /// <param name="filePath" type="string">
        ///     <para>
        ///         要提取资源的文件全路径
        ///     </para>
        /// </param>
        /// <returns>
        ///    返回存有该文件图形资源的Hashtable
        /// </returns>
        public static Hashtable ReadImageResourceFormFile(String filePath)
        {
            Hashtable tempTable;
            using(ResXResourceReader rm = new ResXResourceReader(filePath))
            {
                tempTable = new Hashtable();
                foreach (DictionaryEntry d in rm)
                {
                    tempTable.Add(d.Key.ToString(), d.Value);
                }
            }
            return tempTable;
        }

        /// <summary>
        ///     资源文件相关信息类
        /// </summary>
        public class ImageResourceCotent
        {
            private String _resourceKey;
            private String _resourcePath;

            /// <summary>
            ///     资源标示
            /// </summary>
            public string ResourceKey
            {
                get { return _resourceKey; }
                set { _resourceKey = value; }
            }

            /// <summary>
            ///     资源路径
            /// </summary>
            public string ResourcePath
            {
                get { return _resourcePath; }
                set { _resourcePath = value; }
            }
        }
    }
}
