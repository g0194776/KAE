using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace KJFramework.UI
{
    /// <summary>
    /// SmallfontlineButton.xaml 的交互逻辑
    /// </summary>
    public partial class SmallfontlineButton : UserControl
    {
        public SmallfontlineButton()
        {
            InitializeComponent();
        }

        #region 控件属性

        private Brush _fontcolor;
        [Category("控件自定义属性")]
        [Description("字体颜色")]
        public Brush FontColor
        {
            get { return _fontcolor; }
            set
            {
                _fontcolor = value;
                this.Tb_Text.Foreground = _fontcolor;
            }
        }

        private ImageSource _ButtonImage;
        /// <summary>
        ///     按钮图像
        /// </summary>
        [Description("对于按钮上图像的支持")]
        [Category("控件自定义属性")]
        public ImageSource ButtonImage
        {
            get { return _ButtonImage; }
            set
            {
                _ButtonImage = value;
                this.Img_ButtonPicture.Source = _ButtonImage;
            }
        }

        private String _text;
        [Description("对于按钮上文字的支持")]
        [Category("控件自定义属性")]
        public String Text
        {
            get
            {
                _text = this.Tb_Text.Text;
                return _text; 
            }
            set
            {
                _text = value;
                this.Tb_Text.Text = _text;
            }
        }

        private Brush _layercolor;
        [Description("对于按钮上动画浮动层颜色的支持")]
        [Category("控件自定义属性")]
        public Brush LayerColor
        {
            get
            {
                _layercolor = Bd_Back.Background;
                return _layercolor;
            }
            set
            {
                _layercolor = value;

                Bd_Back.Background = _layercolor;
            }
        }

        private double _initopacity;
        [Description("对于按钮上图片初始透明度的支持")]
        [Category("控件自定义属性")]
        public double InitOpacity
        {
            get
            {
                _initopacity = this.Img_ButtonPicture.Opacity;
                return _initopacity;
            }
            set
            {
                _initopacity = value;
                this.Img_ButtonPicture.Opacity = _initopacity;
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

        #endregion
    }
}
