using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using KJFramework.UI.Enums;

namespace KJFramework.UI
{
	/// <summary>
	///    提供了类似于QQ2009中可扩展菜单项按钮的控件
	/// </summary>
	public partial class QQDropableMenuButton
    {
        #region 成员

	    private bool _isOpen;
        /// <summary>
        ///     获取一个值，该值标示了当前按钮右侧扩展菜单是否展开了
        /// </summary>
        public bool IsOpen
        {
            get { return _isOpen; }
            private set
            {
                _isOpen = value;
                bd_Right_Normal.Visibility = _isOpen ? Visibility.Hidden : Visibility.Visible;
                bd_Right_Clicked.Visibility = _isOpen ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private ImageSource _imageSource;
        /// <summary>
        ///     获取或设置左侧显示的图片资源
        /// </summary>
        public ImageSource ImageSource
        {
            get { return _imageSource = img.Source; }
            set { _imageSource = img.Source = value; }
        }

	    private String _tooltip;
        /// <summary>
        ///     获取或设置左侧按钮的Tooltip内容
        /// </summary>
        [Description("获取或设置左侧按钮的Tooltip内容")]
        [Category("控件自定义属性")]
	    public string Tooltip
	    {
	        get { return _tooltip = (string) gd_Left.ToolTip; }
            set
            {
                _tooltip = value;
                gd_Left.ToolTip = _tooltip;
            }
	    }

	    private ContextMenu _menu = new ContextMenu();
        /// <summary>
        ///     获取上下文菜单
        /// </summary>
	    public ContextMenu Menu
	    {
	        get { return _menu; }
        }

        private bool _allowTransparent;
        /// <summary>
        ///     获取或设置一个值，该值标示了当前是否可以将左右两侧的背景设置为透明
        /// </summary>
        [Description("获取或设置一个值，该值标示了当前是否可以将左右两侧的背景设置为透明")]
        [Category("控件自定义属性")]
	    public bool AllowTransparent
	    {
	        get { return _allowTransparent; }
	        set
	        {
	            _allowTransparent = value;
	            bd_Left_Normal.Visibility =
	                bd_Right_Normal.Visibility =
	                _allowTransparent ? Visibility.Hidden : Visibility.Visible;
	        }
	    }

        private DropableMenuButtonTypes _menuButtonTypes = DropableMenuButtonTypes.Ext;

        /// <summary>
        ///     获取或设置按钮布局
        /// </summary>
        [Description("获取或设置按钮布局")]
        [Category("控件自定义属性")]
	    public DropableMenuButtonTypes MenuButtonTypes
	    {
	        get { return _menuButtonTypes; }
	        set
	        {
	            _menuButtonTypes = value;
	            gd_Right.Visibility = _menuButtonTypes == DropableMenuButtonTypes.Ext
	                                      ? Visibility.Visible
	                                      : Visibility.Collapsed;
	        }
	    }

	    #endregion

        #region 构造函数

        /// <summary>
        ///    提供了类似于QQ2009中可扩展菜单项按钮的控件
        /// </summary>
        public QQDropableMenuButton()
        {
            InitializeComponent();
            Initialize();
        }

        #endregion

        #region 方法

        /// <summary>
        ///     初始化
        /// </summary>
        private void Initialize()
        {
            _menu.Height = 100;
            _menu.Width = 100;
            //item1.Style = (Style)FindResource("Normal_Style_QQMenuItem");
        }

        #endregion

        #region 事件

        /// <summary>
        ///     左侧按钮单击事件
        /// </summary>
	    public event EventHandler Clicked;
	    private void ClickedHandler(System.EventArgs e)
	    {
	        EventHandler clicked = Clicked;
	        if (clicked != null) clicked(this, e);
	    }

	    //右侧扩展菜单按钮单击事件
        private void GridRightClick(Object sender, MouseButtonEventArgs e)
        {
            IsOpen = !_isOpen;
            _menu.IsOpen = _isOpen;
            _menu.Closed += MenuClosed;
        }

        //上下文菜单关闭事件
        void MenuClosed(object sender, RoutedEventArgs e)
        {
            if(IsOpen)
            {
                IsOpen = !_isOpen;
            }
        }

        //左侧按钮单击事件
        private void GridLeftClick(Object sender, MouseButtonEventArgs e)
        {
        	ClickedHandler(new System.EventArgs());
        }

        #endregion
    }
}