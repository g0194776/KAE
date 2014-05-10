using System.Management.Instrumentation;

namespace KJFramework.Datas
{
    /// <summary>
    /// 用来管理数据库类实例与Repository的注入关系
    /// </summary>
    public sealed class DatabaseManager
    {
        #region Members

        /// <summary>
        /// 静态实例
        /// </summary>
        public static DatabaseManager Instance = new DatabaseManager();
        private static Database _masterDB;
        private static Database _slaveDB;

        #endregion

        #region Constructor

        private DatabaseManager()
        {
        }

        #endregion

        /// <summary>
        ///     注入db实例
        /// </summary>
        /// <param name="masterDB">主DB实例</param>
        /// <param name="slaveDB">从DB实例</param>
        public static void Inject(Database masterDB, Database slaveDB)
        {
            _masterDB = masterDB;
            _slaveDB = slaveDB;
        }

        /// <summary>
        ///     获取主DB实例
        /// </summary>
        /// <exception cref="InstanceNotFoundException">数据库实例设置不正确</exception>
        public Database MasterDB {
            get
            {
                if (null == _masterDB) throw new InstanceNotFoundException("#You havn't specific any *MASTER* db instance!!");
                return _masterDB;
            }
        }

        /// <summary>
        ///     获取从DB实例
        /// </summary>
        /// <exception cref="InstanceNotFoundException">数据库实例设置不正确</exception>
        public Database SlaveDB
        {
            get
            {
                if (null == _slaveDB) throw new InstanceNotFoundException("#You havn't specific any *SLAVE* db instance!!");
                return _slaveDB;
            }
        }
    }
}
