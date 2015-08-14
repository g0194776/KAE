using System.Configuration;
using KJFramework.Datas;

namespace KJFramework.ApplicationEngine.Apps.Configuration
{
    /// <summary>
    ///    CSN在KAE中的应用
    /// </summary>
    public class CSNApplication : Application
    {
        #region Methods.

        /// <summary>
        ///    初始化函数
        /// </summary>
        protected override void InnerInitialize()
        {
            Global.ConfigDB = ConfigurationManager.ConnectionStrings["CSNDB"].ConnectionString;
            Global.Database = new Database(Global.ConfigDB, 120);
        }

        #endregion
    }
}