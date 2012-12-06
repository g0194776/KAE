using System;
using System.Collections.Generic;
using LoadRunner;

namespace KJFramework.Test.LoadRunner
{
    /// <summary>
    ///     测试上下文
    /// </summary>
    public class TestContext : ITestContext
    {
        #region Members

        private object _lockObj = new object();
        private Dictionary<string, object> _dic = new Dictionary<string, object>();

        #endregion

        #region Constructor

        /// <summary>
        ///     测试上下文
        /// </summary>
        /// <param name="api">LoadRunner API</param>
        public TestContext(LrApi api)
        {
            if (api == null) throw new ArgumentNullException("api");
            _api = api;
        }

        #endregion

        #region Implementation of ITestContext

        private LrApi _api;

        /// <summary>
        ///     获取LoadRunner API
        /// </summary>
        public LrApi Api
        {
            get { return _api; }
        }

        /// <summary>
        ///     添加一个共享资源
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="value">VALUE</param>
        public void AddResource(string key, object value)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException("key");
            lock (_lockObj) { _dic[key] = value; }
        }

        /// <summary>
        ///     移除一个共享资源
        /// </summary>
        /// <param name="key">KEY</param>
        public void RemoveResource(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException("key");
            lock (_lockObj) { _dic.Remove(key); }
        }

        /// <summary>
        ///     获取一个共享资源
        /// </summary>
        /// <param name="key">KEY</param>
        /// <returns>返回获取到的共享资源</returns>
        public object GetResource(string key)
        {
            lock (_lockObj)
            {
                object value;
                return _dic.TryGetValue(key, out value) ? value : null;
            }
        }

        #endregion
    }
}