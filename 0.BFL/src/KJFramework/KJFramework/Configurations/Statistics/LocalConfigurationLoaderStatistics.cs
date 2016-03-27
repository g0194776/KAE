using System;
using KJFramework.Configurations.Loaders;
using KJFramework.Statistics;
using System.Diagnostics;

namespace KJFramework.Configurations.Statistics
{
    /// <summary>
    ///     本地配置文件加载器统计器
    /// </summary>
    public class LocalConfigurationLoaderStatistics : Statistic
    {
        #region 成员

        private LocalConfigurationLoader _loader;

        #endregion

        #region Overrides of Statistic

        /// <summary>
        ///     初始化
        /// </summary>
        /// <param name="element">统计类型</param>
        /// <typeparam name="T">统计类型</typeparam>
        public override void Initialize<T>(T element)
        {
            _loader = (LocalConfigurationLoader) ((Object)element);
            _loader.LoadSuccessfully += LoadSuccessfully;
            _loader.LoadFailed += LoadFailed;
        }

        /// <summary>
        ///     关闭统计
        /// </summary>
        public override void Close()
        {
            if (_loader != null)
            {
                _loader.LoadSuccessfully -= LoadSuccessfully;
                _loader.LoadFailed -= LoadFailed;
            }
        }

        #endregion

        #region 事件

        void LoadFailed(object sender, System.EventArgs e)
        {
            Debug.WriteLine("本地加载配置文件失败。");
        }

        void LoadSuccessfully(object sender, System.EventArgs e)
        {
            Debug.WriteLine("本地加载配置文件成功。");
        }


        #endregion
    }
}