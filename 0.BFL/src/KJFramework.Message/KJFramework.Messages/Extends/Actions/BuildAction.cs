using System;

namespace KJFramework.Messages.Extends.Actions
{
    /// <summary>
    ///     ���춯�����࣬�ṩ����صĻ���������
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
        ///     ��ȡ���춯�����
        /// </summary>
        public int BuildId
        {
            get { return _buildId; }
            internal set { _buildId = value; }
        }

        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ���춯���Ƿ�����ִ��ָ��������
        /// </summary>
        public bool IsBuilding
        {
            get { return _isBuilding; }
            internal set { _isBuilding = value; }
        }

        #endregion
    }
}