using System;
using System.Windows;
using System.Windows.Controls;
using KJFramework.UI.EventArgs;

namespace KJFramework.UI.Calendar
{
    public partial class CalendarPanel
    {
        #region Event

        /// <summary>
        ///    用户选定日期事件
        /// </summary>
        public event DelegateSelectedDate SelectedDate;
        private void SelectedDateHandler(SelectedDateEventArgs e)
        {
            DelegateSelectedDate date = SelectedDate;
            if (date != null) date(this, e);
        }

        //用户左键选中日期时触发的事件
        private void ButtonMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DateButton button = sender as DateButton;
            if (selectedElement >= 0)
            {
                DateButton prev = DateGrid.Children[selectedElement] as DateButton;
                prev.IsOnSelected = false;
            }
            selectedElement = button.ElementID;
            if (RootReference != null) RootReference.Date = button.Date;
            SelectedDateHandler(new SelectedDateEventArgs());
        }

        #endregion

        #region Constructor

        public CalendarPanel()
        {
            InitializeComponent();
            InitializeCalendar();
        }

        #endregion

        #region Property

        private DateTime selectedDate = DateTime.Today;
        private int selectedElement = -1;
        public MonthCalendar RootReference { get; set; }

        /// <summary>
        ///     获取或设置当前用户选定的日期
        /// </summary>
        public DateTime Date
        {
            get { return selectedDate; }
            set
            {
                if (value != null)
                {
                    DateTime oldValue = selectedDate;
                    selectedDate = value;
                    if (selectedElement >= 0)
                    {
                        DateButton button = DateGrid.Children[selectedElement] as DateButton;
                        if (button != null) button.IsOnSelected = true;
                    }
                    if (selectedDate.Month != oldValue.Month || selectedDate.Year != oldValue.Year)
                    {
                        InitializeCalendar();
                    }
                }

            }
        }

        #endregion

        #region Function

        public void InitializeCalendar()
        {
            DateTime currentMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);
            int offset = (int)currentMonth.DayOfWeek;
            if (offset == 0) currentMonth = currentMonth.AddDays(-7);
            else currentMonth = currentMonth.AddDays(-offset);
            int row = 0;
            int col = 0;
            DateGrid.Children.Clear();
            GC.Collect();
            for (int i = 0; i < 42; i++, col++)
            {
                if (col > 6) { col = 0; row++; }
                DateButton button = new DateButton();
                button.ElementID = i;
                button.Date = currentMonth;
                button.CurrentMonth = selectedDate;
                button.VerticalAlignment = VerticalAlignment.Stretch;
                button.HorizontalAlignment = HorizontalAlignment.Stretch;
                button.MouseLeftButtonUp += ButtonMouseLeftButtonUp;
                if (currentMonth == selectedDate) { button.IsOnSelected = true; selectedElement = i; }
                DateGrid.Children.Add(button);
                Grid.SetRow(button, row);
                Grid.SetColumn(button, col);
                currentMonth = currentMonth.AddDays(1);
            }
        }

        #endregion
    }
}