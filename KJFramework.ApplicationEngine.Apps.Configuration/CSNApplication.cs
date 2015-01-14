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
            Global.Database = new Database(Global.ConfigDB, 120);
        }

        #endregion
    }
}