using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using KJFramework.Dynamic.Components;
using KJFramework.Dynamic.Finders;
using KJFramework.Dynamic.Structs;
using KJFramework.Tracing;

namespace KJFramework.ApplicationEngine
{
    /// <summary>
    ///    KAE宿主
    /// </summary>
    public class KAEHost : IKAEHost
    {
        #region Constructor

        /// <summary>
        ///     动态程序域服务，提供了相关的基本操作。
        ///     <para>* 使用此构造将会从配置文件中读取服务相关信息</para>
        /// </summary>
        public KAEHost()
            : this(Process.GetCurrentProcess().MainModule.FileName.Substring(0, Process.GetCurrentProcess().MainModule.FileName.LastIndexOf('\\') + 1))
        {
        }

        /// <summary>
        ///     动态程序域服务，提供了相关的基本操作。
        /// </summary>
        /// <param name="workRoot">工作目录</param>
        /// <exception cref="System.ArgumentNullException">参数错误</exception>
        /// <exception cref="DirectoryNotFoundException">工作目录错误</exception>
        public KAEHost(String workRoot)
        {
            if (workRoot == null) throw new ArgumentNullException("workRoot");
            if (!Directory.Exists(workRoot)) throw new DirectoryNotFoundException("Current work root don't existed. #dir: " + workRoot);
            _workRoot = workRoot;
        }

        #endregion

        #region Members.

        private readonly string _workRoot;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (KAEHost));
        /// <summary>
        ///    获取内部运行的应用数量
        /// </summary>
        public ushort ApplicationCount { get; private set; }
        /// <summary>
        ///    获取工作目录
        /// </summary>
        public string WorkRoot { get; private set; }

        #endregion

        #region Methods.

        /// <summary>
        ///    KAE宿主初始化函数
        /// </summary>
        protected virtual IList<DynamicDomainObject> Initialize()
        {
            return null;
        }

        /// <summary>
        ///    开启KAE宿主
        /// </summary>
        public void Start()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        ///    开启KAE宿主
        /// </summary>
        public void Stop()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}