using System;

namespace KJFramework.Net.Cloud.Virtuals.Processors
{
    /// <summary>
    ///     傀儡功能处理器抽象父类，提供了相关的基本操作
    /// </summary>
    public abstract class PuppetFunctionProcessor : IPuppetFunctionProcessor
    {
        #region Constructor

        /// <summary>
        ///     傀儡功能处理器抽象父类，提供了相关的基本操作
        /// </summary>
        protected PuppetFunctionProcessor()
        {
            _id = Guid.NewGuid();
        }

        #endregion

        #region Implementation of IPuppetFunctionProcessor

        protected Guid _id;

        /// <summary>
        ///     获取唯一编号
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        ///     初始化
        /// </summary>
        /// <typeparam name="T">宿主类型</typeparam>
        /// <param name="target">宿主对象</param>
        /// <returns>返回初始化的状态</returns>
        public abstract bool Initialize<T>(T target);

        /// <summary>
        ///     释放当前的傀儡功能处理
        /// </summary>
        public abstract void Release();

        #endregion
    }
}