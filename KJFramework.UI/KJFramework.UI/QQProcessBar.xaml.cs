using System;
using System.ComponentModel;
using System.Windows;

namespace KJFramework.UI
{
    /// <summary>
    ///     QQ2009中的ProcessBar效果
    /// </summary>
    [Description("完美的呈现了QQ2009中的ProcessBar效果。")]
	public partial class QQProcessBar
	{
	    protected double _imgChunkWidth;
        protected double _borderWidth;

		public QQProcessBar()
		{
			this.InitializeComponent();
			// 在此点之下插入创建对象所需的代码。
            this.Loaded += new RoutedEventHandler(QQProcessBar_Loaded);
		}

        void QQProcessBar_Loaded(object sender, RoutedEventArgs e)
        {
            //得到总长
            _borderWidth = Ph_Border.ActualWidth;
            //得到分块刻度
            _imgChunkWidth = _borderWidth/_max;
        }

        private double _min;
        private double _max = 100;
        private double _value;
        /// <summary>
        ///     获取或设置最小值
        ///         * 最小值不能小于0
        /// </summary>
        [Description("获取或设置最小值, 最小值不能小于0")]
        [Category("控件自定义属性")]
        public double Min
	    {
	        get { return _min; }
	        set
	        {
	            _min = value;
	            if (_min < 0)
	            {
	                _min = 0;
	            }
	        }
	    }

        /// <summary>
        ///     获取或设置最大值
        ///         * 最大值不能小于最小值
        /// </summary>
        [Description("获取或设置最大值, 最大值不能小于最小值")]
        [Category("控件自定义属性")]
        public double Max
	    {
	        get { return _max; }
	        set
	        {
	            _max = value;
	            if (_max < _min)
	            {
	                _max = _min;
	            }
	        }
	    }

        /// <summary>
        ///     获取或设置当前值
        ///         * 当前值不能小于0
        /// </summary>
        [Description("获取或设置当前值, 当前值不能小于最小值, 也不能大于最大值")]
        [Category("控件自定义属性")]
        public double Value
	    {
	        get { return _value; }
	        set
            {
                _value = value;
                if (_value < _min)
                {
                    _value = _min;
                }
                if (_value > _max)
                {
                    _value = _max;
                }
                if (_value == _max)
                {
                    Img_Background.Width = _borderWidth;
                }
                else
                {
                    Img_Background.Width = _imgChunkWidth * _value;
                }
            }
	    }

        /// <summary>
        ///     进度条值更改事件
        /// </summary>
        public event DELEGATE_VALUE_CHANGED ValueChanged;
        private void InvokeValueChanged(ValueChangedEventArgs e)
        {
            DELEGATE_VALUE_CHANGED changed = ValueChanged;
            if (changed != null) changed(this, e);
        }
	}

    public delegate void DELEGATE_VALUE_CHANGED(Object sender, ValueChangedEventArgs e);
    /// <summary>
    ///     进度条值更改事件
    /// </summary>
    public class ValueChangedEventArgs : System.EventArgs
    {
        private int _value;
        /// <summary>
        ///     更改值
        /// </summary>
        public int Value
        {
            get { return _value; }
        }

        /// <summary>
        ///     进度条值更改事件
        /// </summary>
        /// <param name="value">更改值</param>
        public ValueChangedEventArgs(int value)
        {
            _value = value;
        }
    }
}