using System;
using System.Collections.Generic;
using System.Linq;

namespace KJFramework.Messages.Extends
{
    /// <summary>
    ///     第三方消息定义安装器，提供了相关的基本操作。
    /// </summary>
    public static class CustomerMessageDefinerInstaller
    {
        #region 成员

        private static Dictionary<Guid, ICustomerMessageDefiner> _definer = new Dictionary<Guid, ICustomerMessageDefiner>();

        #endregion

        #region 方法

        /// <summary>
        ///     安装一个第三方消息定义器
        /// </summary>
        /// <param name="definer">第三方消息定义器</param>
        public static void Install(ICustomerMessageDefiner definer)
        {
            if (definer == null)
            {
                return;
            }
            //自检查
            definer.Check();
            lock (typeof(CustomerMessageDefinerInstaller))
            {
                if (!_definer.ContainsKey(definer.Id))
                {
                    //向拘留池中添加频繁调用的字符串。
                    String.Intern(definer.Key);
                    _definer.Add(definer.Id, definer);
                }
            }
        }

        /// <summary>
        ///     卸载一个具有指定编号的第三方消息定义器
        /// </summary>
        /// <param name="id">编号</param>
        public static void UnInstall(Guid id)
        {
            lock (typeof(CustomerMessageDefinerInstaller))
            {
                if (_definer.ContainsKey(id))
                {
                    _definer.Remove(id);
                }
            }
        }

        /// <summary>
        ///     通过指定的用户键值，获取一个第三方消息定义器
        /// </summary>
        /// <param name="key">用户键值</param>
        /// <returns>返回指定的第三方消息定义器</returns>
        public static ICustomerMessageDefiner GetDefiner(String key)
        {
            if (String.IsNullOrEmpty(key))
            {
                return null;
            }
            lock (typeof(CustomerMessageDefinerInstaller))
            {
                var result = _definer.Values.Where(definer => definer.Key == key);
                if (result != null && result.Count() > 0)
                {
                    return result.First();
                }
                return null;
            }
        }

        #endregion
    }
}