using System;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Caches
{
    /// <summary>
    ///     ���ݿ����ݻ������
    /// </summary>
    public class DBDataCache : IDataCache<DataTable>
    {
        #region Constructor

        /// <summary>
        ///     ���ݿ����ݻ������
        /// </summary>
        public DBDataCache()
        {
            _lastUpdateTime = _lastVisitTime =  DateTime.Now;
        }

        #endregion

        #region Implementation of IDataCache<DataTable>

        protected string _key;
        protected DataTable _item;
        protected DateTime _lastVisitTime;
        protected DateTime _lastUpdateTime;

        /// <summary>
        ///     ��ȡ�����û����Ψһ��ֵ
        /// </summary>
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        ///     ��ȡ������Ҫ�������
        /// </summary>
        public DataTable Item
        {
            get { return _item; }
            set { _item = value; }
        }

        /// <summary>
        ///     ��ȡ������������ʱ��
        /// </summary>
        public DateTime LastVisitTime
        {
            get { return _lastVisitTime; }
            set { _lastVisitTime = value; }
        }

        /// <summary>
        ///     ��ȡ������������ʱ��
        /// </summary>
        public DateTime LastUpdateTime
        {
            get { return _lastUpdateTime; }
            set { _lastUpdateTime = value; }
        }

        #endregion
    }
}