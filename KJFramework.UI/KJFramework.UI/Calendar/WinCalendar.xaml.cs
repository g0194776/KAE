using System;

namespace KJFramework.UI.Calendar
{
    /// <summary>
    ///    日期选择窗口
    /// </summary>
    public partial class WinCalendar
    {
        #region Property

        /// <summary>
        ///    获取用户选择的日期
        /// </summary>
        public DateTime? Date;

        #endregion

        #region Constructor
        
        /// <summary>
        ///     日期选择窗口
        /// </summary>
        public WinCalendar()
        {
            InitializeComponent();
            calendar.EntrySelectedDate = GetDate;
            // 在此点之下插入创建对象所需的代码。
        }

        #endregion

        #region Function

        /// <summary>
        ///    获取用户选择的日期
        /// </summary>
        /// <returns></returns>
        public void GetDate(DateTime? date)
        {
            Date = date;
            Close();
        }

        #endregion
    }
}