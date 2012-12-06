using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.ComponentModel;

namespace KJFramework.UI
{
    [Description("重新设计了TextBox的样式，使之适配一个描述文本。样式模仿了Vista")]
	public partial class VistaTextBox
	{
		public VistaTextBox()
		{
			this.InitializeComponent();

			// 在此点之下插入创建对象所需的代码。
		}

        private String _title;
        /// <summary>
        ///     获取或设置标签描述内容
        /// </summary>
        [Category("控件自定义属性")]
        [Description("对于描述文字的支持")]
        public String Title
        {
            get
            {
                _title = this.Tb_Title.Text;
                return _title; 
            }
            set
            { 
                _title = value;
                this.Tb_Title.Text = _title;
            }
        }

        private String _text;
        /// <summary>
        ///     获取或设置文本框中的文字内容
        /// </summary>
        [Category("控件自定义属性")]
        [Description("对于文本框内容的支持")]
        public String Text
        {
            get
            {
                _text = this.txt_Content.Text;
                return _text;
            }
            set
            {
                _text = value;
                this.txt_Content.Text = _text;
            }
        }

        private Brush _titlebrush;
        /// <summary>
        ///     获取或设置标签描述的前景色
        /// </summary>
        [Category("控件自定义属性")]
        [Description("对于标签描述的前景色的支持")]
        public Brush TitleBrush
        {
            get 
            {
                _titlebrush = this.Tb_Title.Foreground;
                return _titlebrush;
            }
            set
            {
                _titlebrush = value;
                this.Tb_Title.Foreground = _titlebrush;
            }
        }

        private Brush _textboxbackground;
        /// <summary>
        ///     获取或设置文本框背景颜色
        /// </summary>
        [Category("控件自定义属性")]
        [Description("对于文本框背景颜色的支持")]
        public Brush TextBoxBackground
        {
            get
            {
                _textboxbackground = this.txt_Content.Background;
                return _textboxbackground;
            }
            set
            {
                _textboxbackground = value;
                this.txt_Content.Background = _textboxbackground;
            }
        }

        private Brush _layerbackground;
        /// <summary>
        ///     获取或设置动画浮动层背景色
        /// </summary>
        [Category("控件自定义属性")]
        [Description("对于动画浮动层背景色的支持")]
        public Brush LayerBackground
        {
            get
            {
                _layerbackground = Bd_Fore.Background;
                return _layerbackground; 
            }
            set 
            {
                _layerbackground = value;
                Bd_Fore.Background = _layerbackground;
            }
        }

        private double _width;
        [Description("对于按钮上宽度的支持")]
        [Category("控件自定义属性")]
        public double Width
        {
            get
            {
                _width = this.Width;
                return _width;
            }
            set
            {
                _width = value;
                this.Width = _width;
            }
        }

        private double _height;
        [Description("对于按钮上高度的支持")]
        [Category("控件自定义属性")]
        public double Height
        {
            get
            {
                _height = this.Height;
                return _height;
            }
            set
            {
                _height = value;
                this.Height = _height;
            }
        }
	}
}