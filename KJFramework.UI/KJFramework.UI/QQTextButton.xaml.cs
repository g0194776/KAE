using System;
using System.ComponentModel;
using System.Windows.Media;

namespace KJFramework.UI
{
    [Description("实现了QQ2009中纯文字按钮的效果。")]
	public partial class QQTextButton
	{
		public QQTextButton()
		{
			InitializeComponent();

			// 在此点之下插入创建对象所需的代码。
		}

	    private String _text;
        /// <summary>
        ///     获取或设置按钮的文字
        /// </summary>
        [Description("获取或设置按钮的文字")]
        [Category("控件自定义属性")]
	    public string Text
	    {
	        get { return _text = tb_Text.Text; }
            set { _text = tb_Text.Text = value; }
	    }

        private Brush _outBorderBrush;
        /// <summary>
        ///     获取或设置按钮外层边框笔刷
        /// </summary>
        [Description(" 获取或设置按钮外层边框笔刷")]
        [Category("控件自定义属性")]
	    public Brush OutBorderBrush
	    {
	        get { return _outBorderBrush = Bd_out.BorderBrush; }
            set { _outBorderBrush = Bd_out.BorderBrush = value; }
	    }

        private Brush _inBorderBrush;
        /// <summary>
        ///     获取或设置按钮外层边框笔刷
        /// </summary>
        [Description(" 获取或设置按钮内层边框笔刷")]
        [Category("控件自定义属性")]
        public Brush InBorderBrush
        {
            get { return _inBorderBrush = Bd_in.BorderBrush; }
            set { _inBorderBrush = Bd_in.BorderBrush = value; }
        }

        private Brush _borderBackground;
        /// <summary>
        ///     获取或设置按钮外层边框笔刷
        /// </summary>
        [Description(" 获取或设置边框背景填充颜色")]
        [Category("控件自定义属性")]
        public Brush BorderBackground
        {
            get { return _borderBackground = Bd_out.Background; }
            set { _borderBackground = Bd_out.Background = value; }
        }

	}
}