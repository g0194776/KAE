using System;
namespace KJFramework.Configurations.Objects
{
    /// <summary>
    ///     内部的XML节点信息临时结构
    /// </summary>
    public class InnerXmlNodeInfomation : IDisposable
    {
        #region 析构函数

        ~InnerXmlNodeInfomation()
        {
            Dispose();
        }

        #endregion

        #region IDisposable 成员

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region 成员

        /// <summary>
        ///     获取或设置XML节点名称
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        ///     获取或设置该节点的XML元数据
        /// </summary>
        public String OutputXml { get; set; }

        #endregion
    }
}