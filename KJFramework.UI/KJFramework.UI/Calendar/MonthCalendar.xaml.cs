using System;
using KJFramework.UI.EventArgs;

namespace KJFramework.UI.Calendar
{
    /// <summary>
    ///     月份日历项
    /// </summary>
    public partial class MonthCalendar
    {
        #region Property

        /// <summary>
        ///     日期选择入口
        /// </summary>
        /// <param name="date"></param>
        public delegate void DelegateSelectedDate(DateTime? date);
        private DateTime _selectedDate = DateTime.Today;

        private DelegateSelectedDate _entrySelectedDate;
        /// <summary>
        ///    获取或设置选定日期函数入口点
        /// </summary>
        public DelegateSelectedDate EntrySelectedDate
        {
            get { return _entrySelectedDate; }
            set { _entrySelectedDate = value; }
        }

        /// <summary>
        ///    获取或设置用户当前选定的日期
        /// </summary>
        public DateTime Date
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                MonthLabel.Content = _selectedDate.ToString("MMM - yyyy");
                SelectedMonthCalendar.Date = _selectedDate;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        ///     月份日历项
        /// </summary>
        public MonthCalendar()
        {
            InitializeComponent();
            MonthLabel.Content = _selectedDate.ToString("MMM - yyyy");
            TodayLabel.Content = string.Format("{0}/{1}/{2}", DateTime.Today.Month, DateTime.Today.Day, DateTime.Today.Year); 
            SelectedMonthCalendar.RootReference = this;
            SelectedMonthCalendar.SelectedDate += SelectedMonthCalendarSelectedDate;
            TodayLabel.MouseUp += TodayLabelMouseUp;
        }

        #endregion

        #region Event

        //用户选择日期后触发的事件
        void SelectedMonthCalendarSelectedDate(object sender, SelectedDateEventArgs e1)
        {
            Do();
        }

        private void TurnLeftButton_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Date = new DateTime(_selectedDate.Year, _selectedDate.Month, 1).AddMonths(-1);
        }

        private void TurnRightButton_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Date = new DateTime(_selectedDate.Year, _selectedDate.Month, 1).AddMonths(1);
        }

        private void TodayLabelMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Date = DateTime.Today;
            SelectedMonthCalendar.InitializeCalendar();
        }

        #endregion

        #region Function

        /// <summary>
        ///    告知调用者，用户已经选择了日期
        /// </summary>
        private void Do()
        {
            if (_entrySelectedDate != null)
            {
                _entrySelectedDate(Date);
            }
        }

        #endregion
    }
}