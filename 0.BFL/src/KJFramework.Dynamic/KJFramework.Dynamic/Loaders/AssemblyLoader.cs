using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace KJFramework.Dynamic.Loaders
{
    /// <summary>
    ///     程序集加载器
    /// </summary>
    public class AssemblyLoader : IDynamicLoader
    {
        #region Constructor

        /// <summary>
        ///     程序集加载器
        /// </summary>
        /// <param name="currentDomain">要绑定的应用程序域</param>
        public AssemblyLoader(AppDomain currentDomain)
        {
            if (currentDomain == null) throw new ArgumentNullException("currentDomain");
            _currentDomain = currentDomain;
            _currentDomain.AssemblyResolve += AssemblyResolve;
        }

        #endregion

        #region Members

        private readonly Dictionary<string, string> _files = new Dictionary<string, string>();
        private readonly AppDomain _currentDomain;

        #endregion

        #region Events

        //event.
        Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string temp = args.Name.Substring(0, args.Name.IndexOf(','));
            string path;
            if (_files.TryGetValue(temp, out path)) return Assembly.LoadFile(path);
            #if (DEBUG)
            {
                Console.WriteLine("Cannot find assembly: " + args.Name);
            }
            #endif
            return null;
        }

        /// <summary>
        ///     加载成功事件
        /// </summary>
        public event EventHandler LoadSuccessfully;
        protected void LoadSuccessfullyHandler(System.EventArgs e)
        {
            EventHandler handler = LoadSuccessfully;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     加载失败事件
        /// </summary>
        public event EventHandler LoadFailed;
        protected void LoadFailedHandler(System.EventArgs e)
        {
            EventHandler handler = LoadFailed;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     动态加载
        ///     <para>* 此地方输入的路径应该为要加载全局Assembly的路径</para>
        /// </summary>
        /// <param name="args">加载参数</param>
        public void Load(params string[] args)
        {
            string path = args[0];
            foreach (string file in Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories))
            {
                string temp = Path.GetFileNameWithoutExtension(file);
                if (!_files.ContainsKey(temp)) _files.Add(temp, file);
            }
        }

        #endregion
    }
}