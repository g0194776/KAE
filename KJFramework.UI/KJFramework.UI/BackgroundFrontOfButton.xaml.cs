using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace KJFramework.UI
{
    [Description("支持了特定鼠标移动效果的按钮, 且效果滑块在内容之上。")]
	public partial class BackgroundFrontOfButton
	{
        public BackgroundFrontOfButton()
		{
			InitializeComponent();
			// 在此点之下插入创建对象所需的代码。
		}

        private ImageSource _buttonimage;
        /// <summary>
        ///     按钮图像
        /// </summary>
        [Description("对于按钮上图像的支持")]
        [Category("控件自定义属性")]
        public ImageSource ButtonImage
        {
            get
            {
                _buttonimage = Img.Source;
                return _buttonimage; 
            }
            set
            {
                _buttonimage = value;
                Img.Source = _buttonimage;
            }
        }

        private BtnType _buttonType;
        /// <summary>
        ///     按钮类型
        /// </summary>
        [Description("对于按钮的效果分类")]
        [Category("控件自定义属性")]
        [DefaultValue(typeof(BtnType), "ImageAndText")]
        public BtnType ButtonType
        {
            get { return _buttonType; }
            set
            {
                _buttonType = value;
                if (_buttonType == BtnType.ImageAndText)
                {
                    Img.Visibility = Visibility.Visible;
                    txt_Name.Visibility = Visibility.Visible;
                    txt_InnerName.Visibility = Visibility.Collapsed;
                }
                else if (_buttonType == BtnType.TextOnly)
                {
                    Img.Visibility = Visibility.Collapsed;
                    txt_Name.Visibility = Visibility.Collapsed;
                    txt_InnerName.Visibility = Visibility.Visible;
                }
                else
                {
                    Img.Visibility = Visibility.Visible;
                    txt_Name.Visibility = Visibility.Collapsed;
                    txt_InnerName.Visibility = Visibility.Collapsed;
                }
            }
        }

        private String _text;
        /// <summary>
        ///     按钮文字
        /// </summary>
        [Description("对于按钮的下标文字")]
        [Category("控件自定义属性")]
        [DefaultValue("Button")]
        public String Text
        {
            get { return _text; }
            set 
            { 
                _text = value;
                txt_Name.Text = _text;
                txt_InnerName.Text = _text;
                txt_InnerName.HorizontalAlignment = HorizontalAlignment.Center;
            }
        }

        private Brush _textcolor;
        /// <summary>
        ///     对于按钮文字颜色的支持
        /// </summary>
        [Description("对于按钮文字颜色的支持")]
        [Category("控件自定义属性")]
        public Brush TextColor
        {
            get
            {
                _textcolor = txt_InnerName.Foreground;
                return _textcolor;
            }
            set
            {
                _textcolor = value;
                txt_Name.Foreground = _textcolor;
                txt_InnerName.Foreground = _textcolor;
            }
        }

        private Brush _layercolor;
        /// <summary>
        ///     对于按钮上动画浮动层颜色的支持
        /// </summary>
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

        /// <summary>
        ///     按钮类型枚举
        /// </summary>
        public enum BtnType
        {
            /// <summary>
            ///     只有文字
            /// </summary>
            TextOnly,
            /// <summary>
            ///     图片和文字
            /// </summary>
            ImageAndText,
            /// <summary>
            ///     只有图片
            /// </summary>
            ImageOnly
        }
	}
}