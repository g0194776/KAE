using KJFramework.Data.ObjectDB.Structures;

namespace KJFramework.Data.ObjectDB.Controllers
{
    /// <summary>
    ///     页面控制器接口
    /// </summary>
    internal interface IPageController
    {
        /// <summary>
        ///     创建一个新的页面
        /// </summary>
        /// <returns>如果创建成功，则返回一个新的页面实例</returns>
        IPage CreatePage();
    }
}