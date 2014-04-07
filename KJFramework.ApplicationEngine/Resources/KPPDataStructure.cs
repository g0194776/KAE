using System;
using System.Collections.Generic;

namespace KJFramework.ApplicationEngine.Resources
{
    /// <summary>
    ///    KPP文件数据格式
    /// </summary>
    internal class KPPDataStructure : MarshalByRefObject
    {
        #region Constructor.

        public KPPDataStructure(byte[] data, int offset, int count)
        {
            Initialize(data, offset, count);
        }

        #endregion

        #region Members.

        private readonly Dictionary<string, object> _values = new Dictionary<string, object>(); 

        #endregion

        #region Methods.

        /// <summary>
        ///    内部信息初始化
        /// </summary>
        /// <param name="data">KPP文件元数据</param>
        /// <param name="offset">可用偏移</param>
        /// <param name="count">可用长度</param>
        private void Initialize(byte[] data, int offset, int count)
        {
            
        }

        /// <summary>
        ///    获取具有指定名称字段数据信息
        /// </summary>
        /// <typeparam name="T">字段数据信息类型</typeparam>
        /// <param name="name">字段名</param>
        /// <returns>返回指定名称所代表的数据信息</returns>
        /// <exception cref="KeyNotFoundException">指定的KEY不存在</exception>
        public T GetField<T>(string name)
        {
            return default(T);
        }


        #endregion
    }
}