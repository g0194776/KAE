using System;
using System.Windows;
using System.Windows.Input;

namespace KJFramework.UI.Calendar
{
    /// <summary>
    ///    日期选择控件
    /// </summary>
    public partial class CalendarSelector
    {
        private DateTime _selectDate;
        /// <summary>
        ///     获取或设置选中的日期
        /// </summary>
        public DateTime SelectDate
        {
            set { _selectDate = value;
                tb_data.Text = _selectDate.ToShortDateString(); }
            get { return _selectDate; }
        }

        #region Constructor

        /// <summary>
        ///     日期选择控件
        /// </summary>
        public CalendarSelector()
        {
            InitializeComponent();
            LostFocus += CalendarSelectorLostFocus;
            calendar.EntrySelectedDate = GetDate;
            // 在此点之下插入创建对象所需的代码。
        }

        #endregion

        #region Event

        //控件失去焦点事件
        

        void CalendarSelectorLostFocus(object sender, RoutedEventArgs e)
        {
            pop.IsOpen = false;
        }

        //用户单击了日历图片按钮事件
        private void img_select_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!pop.IsOpen)
            {
                pop.HorizontalOffset = -200 + ActualWidth;
                pop.IsOpen = true;
                tb_data.Focus();
                return;
            }
            pop.IsOpen = false;
        }

        #endregion

        #region Function

        /// <summary>
        ///    获取用户选择的日期
        /// </summary>
        /// <returns></returns>
        public void GetDate(DateTime? date)
        {
            tb_data.Text = (date == null) ? DateTime.Now.ToShortDateString() : ((DateTime)date).ToShortDateString();
            pop.IsOpen = false;
            if (date != null) _selectDate = (DateTime)date;
        }

        #endregion
    }
}