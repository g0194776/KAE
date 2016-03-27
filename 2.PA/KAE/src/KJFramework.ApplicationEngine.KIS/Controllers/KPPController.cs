using System.Data;
using System.Web.Http;

using KJFramework.ApplicationEngine.KIS.Models;
using KJFramework.ApplicationEngine.Objects;
using KJFramework.Tracing;

namespace KJFramework.ApplicationEngine.KIS.Controllers
{
    /// <summary>
    ///    KIS内部针对KPP的controller
    /// </summary>
    public class KPPController : ApiController
    {
        #region Members.

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(KPPController));

        #endregion

        #region Methods.

        /// <summary>
        ///    根据一个APP的完整包名和版本号信息来获取指定KPP的详细信息
        /// </summary>
        /// <param name="pkgname">完整包名</param>
        /// <param name="ver">版本号信息</param>
        /// <returns>返回KPP的详细信息</returns>
        [HttpGet]
        public object GetKPPInfo(string pkgname, string ver)
        {
            if(string.IsNullOrEmpty(pkgname) || string.IsNullOrEmpty(ver))
                return new ErrorModel{Desc = "#Missing some required arguments.", ErrorId = 500};
            try
            {
                DataTable table = Global.Database.SpExecuteTable("spGetKppInfoByNameAndVer", new[] { "Name", "Ver" }, new object[] { pkgname, ver });
                if (table == null || table.Rows.Count == 0) return null;
                DataRow row = table.Rows[0];
                return new PackageInfo
                {
                    Name = row["PackageName"].ToString(),
                    PackageName = row["PackageName"].ToString(),
                    Description = row["Description"].ToString(),
                    CRC = ulong.Parse(row["CRC"].ToString()),
                    Identity = row["Identity"].ToString(),
                    Level = row["Level"].ToString(),
                    Url = row["RemotingUri"].ToString()
                };
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex);
                return new ErrorModel { Desc = "#Internal Server Error.", ErrorId = 500 };
            }
        }

        #endregion
    }
}
