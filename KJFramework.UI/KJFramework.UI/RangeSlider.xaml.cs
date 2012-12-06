using System;
using System.Windows;
using System.Windows.Controls;

namespace KJFramework.UI
{
    /// <summary>
    /// Interaction logic for RangeSlider.xaml
    /// </summary>
    public partial class RangeSlider : UserControl
    {
        public RangeSlider()
        {
            InitializeComponent();
            
            Loaded += SliderLoaded;
        }

        void SliderLoaded(object sender, RoutedEventArgs e)
        {
            LowerSlider.ValueChanged += LowerSliderValueChanged;
            UpperSlider.ValueChanged += UpperSliderValueChanged;
        }

        private void LowerSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //UpperSlider.Value = Math.Max((double) ((sbyte) UpperSlider.Value), (sbyte) LowerSlider.Value);
        }

        private void UpperSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //LowerSlider.Value = Math.Min((double) ((sbyte) UpperSlider.Value), (sbyte) LowerSlider.Value);
        }

        /// <summary>
        ///     获取或设置最小值
        /// </summary>
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(RangeSlider), new UIPropertyMetadata(0d));

        public double LowerValue
        {
            get { return (double)GetValue(LowerValueProperty); }
            set { SetValue(LowerValueProperty, value); }
        }

        public static readonly DependencyProperty LowerValueProperty =
            DependencyProperty.Register("LowerValue", typeof(double), typeof(RangeSlider), new UIPropertyMetadata(0d));

        public double UpperValue
        {
            get { return (double)GetValue(UpperValueProperty); }
            set { SetValue(UpperValueProperty, value); }
        }

        public static readonly DependencyProperty UpperValueProperty =
            DependencyProperty.Register("UpperValue", typeof(double), typeof(RangeSlider), new UIPropertyMetadata(0d));

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(RangeSlider), new UIPropertyMetadata(1d));

        


    }
}