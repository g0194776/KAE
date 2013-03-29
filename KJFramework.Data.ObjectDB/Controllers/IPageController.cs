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
        /// <summary>
        ///     获取一个具有指定编号的页面
        /// </summary>
        /// <param name="pageId">页面编号</param>
        /// <returns>如果存在指定条件的页面，则返回，反之返回null.</returns>
        IPage GetPageById(uint pageId);
    }
}