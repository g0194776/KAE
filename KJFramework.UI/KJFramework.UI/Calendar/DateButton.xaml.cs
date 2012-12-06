using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace KJFramework.UI.Calendar
{
    /// <summary>
    ///     日期按钮
    /// </summary>
    public partial class DateButton : INotifyPropertyChanged
    {
        private DateTime _selectedMonth = DateTime.Today;
        private DateTime _dateValue = DateTime.Today;
        private bool _hasDateValue;
        private bool _onSelected;

        /// <summary>
        ///     日期按钮
        /// </summary>
        public DateButton()
        {
            InitializeComponent();
        }        

        /// <summary>
        ///     获取或设置当前月
        /// </summary>
        public DateTime CurrentMonth
        {
            get { return _selectedMonth; }
            set
            {
                _selectedMonth = value;
                if (_dateValue.Year == _selectedMonth.Year && _dateValue.Month == _selectedMonth.Month)
                    TextLabel.Foreground = Brushes.Black;
                else TextLabel.Foreground = Brushes.Gray;
            }
        }

        /// <summary>
        ///     获取或设置现在时间
        /// </summary>
        public DateTime Date
        {
            get { return _dateValue; }
            set
            {
                _dateValue = value;
                DateTime today = DateTime.Today;
                if (_dateValue == today) ButtonBorder.Opacity = 1;
                else ButtonBorder.Opacity = 0;
                NotifyPropertyChanged("Date");
            }
        }

        /// <summary>
        ///     获取或设置一个值，该值表示了是否包含当前日期
        /// </summary>
        public bool ContainData
        {
            get { return _hasDateValue; }
            set
            {
                _hasDateValue = value;
                if (_hasDateValue) TextLabel.FontWeight = FontWeight.FromOpenTypeWeight(600);
                else TextLabel.FontWeight = FontWeight.FromOpenTypeWeight(400);
            }
        }

        /// <summary>
        ///     获取或设置当前是否被选中
        /// </summary>
        public bool IsOnSelected
        {
            get { return _onSelected; }
            set
            {
                _onSelected = value;
                if (_onSelected) ButtonSelectedFace.Opacity = 1;
                else ButtonSelectedFace.Opacity = 0;
            }
        }

        public int ElementID { get; set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

    [ValueConversion(typeof(DateTime), typeof(string))]
    public class DateTimeConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            return date.Day;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}