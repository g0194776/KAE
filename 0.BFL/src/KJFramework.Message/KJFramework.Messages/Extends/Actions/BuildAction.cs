using System;

namespace KJFramework.Messages.Extends.Actions
{
    /// <summary>
    ///     构造动作基类，提供了相关的基本操作。
    /// </summary>
    public class BuildAction : IBuildAction
    {
        #region Implementation of IDisposable

        protected int _buildId;
        protected bool _isBuilding;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IBuildAction

        /// <summary>
        ///     获取构造动作编号
        /// </summary>
        public int BuildId
        {
            get { return _buildId; }
            internal set { _buildId = value; }
        }

        /// <summary>
        ///     获取一个值，该值标示了当前构造动作是否正在执行指定操作。
        /// </summary>
        public bool IsBuilding
        {
            get { return _isBuilding; }
            internal set { _isBuilding = value; }
        }

        #endregion
    }
}