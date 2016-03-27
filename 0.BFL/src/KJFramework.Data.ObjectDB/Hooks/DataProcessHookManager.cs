using System;
using System.Collections.Generic;
using KJFramework.Data.ObjectDB.Exceptions;

namespace KJFramework.Data.ObjectDB.Hooks
{
    /// <summary>
    ///     数据处理钩子管理器
    /// </summary>
    internal static class DataProcessHookManager
    {
        #region Members

        private static readonly IDictionary<string, IList<IDataProcessHook>> _hooks = new Dictionary<string, IList<IDataProcessHook>>();

        #endregion

        #region Methods

        /// <summary>
        ///     注册一个只针对指定类型的数据处理钩子
        /// </summary>
        /// <param name="type">针对的数据类型</param>
        /// <param name="hook">数据处理钩子</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public static void Regist(Type type, IDataProcessHook hook)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (hook == null) throw new ArgumentNullException("hook");
            IList<IDataProcessHook> hooks;
            if (!_hooks.TryGetValue(type.FullName, out hooks))
                _hooks.Add(type.FullName, new List<IDataProcessHook>(new[] { hook }));
            else hooks.Add(hook);
        }

        /// <summary>
        ///     使用一系列的数据钩子处理当前二进制数据
        /// </summary>
        /// <param name="type">要处理的数据原对象类型</param>
        /// <param name="data"></param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="HookProcessException">数据钩子处理失败</exception>
        public static void Process(Type type, byte[] data)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (data == null) throw new ArgumentNullException("data");
            IList<IDataProcessHook> hooks;
            if (!_hooks.TryGetValue(type.FullName, out hooks) && !_hooks.TryGetValue("Type.Any", out hooks)) return;
            foreach (IDataProcessHook hook in hooks)
            {
                data = hook.Process(data);
                if (data == null) throw new HookProcessException("#There's a *NULL* binary data from a specific hook processor. Type: " + hook);
            }
        }

        #endregion
    }
}