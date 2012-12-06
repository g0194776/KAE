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
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using KJFramework.UI.ScreenShorter;

namespace KJFramework.UI.ScreenShorter
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Win_Shortcut : Window
    {
        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);
        bool IsMouseDown = false;
        double AddWidth = 0;
        double AddHeight = 0;
        bool IsControlPointDown = false;
        bool IsMouseLeftButtonDown = false;
        System.Windows.Point MouseDownPoint;
        System.Windows.Point Localtion;
        Bitmap Desktop;

        /// <summary>
        ///     选取截图对象
        /// </summary>
        ScreenCatch win = null;
        bool IsLeftTopButtonDown = false;
        bool IsTopCenterButtonDown = false;
        bool IsRightTopButtonDown = false;
        bool IsLeftBottomButtonDown = false;
        bool IsBottomCenterButtonDown = false;
        bool IsRightBottomButtonDown = false;
        bool IsLeftCenterButtonDown = false;
        bool IsRightCenterButtonDown = false;
        TextBlock tb_Point;

        public Win_Shortcut()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.Bounds.Height;
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Topmost = true;
            this.Left = 0;
            this.Top = 0;
            this.Desktop = this.GetDesktop();
            IntPtr imgPtr = Desktop.GetHbitmap();
            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(imgPtr, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            DeleteObject(imgPtr);
            this.Background = new ImageBrush(bitmapSource);
            this.AppendEvent();
            tb_Point = new TextBlock();
            tb_Point.Foreground = System.Windows.Media.Brushes.Red;
            tb_Point.FontSize = 30;
            tb_Point.Text = "坐标";
            this.Cav_Back.Children.Add(tb_Point);
            Canvas.SetLeft(tb_Point, 5);
            Canvas.SetTop(tb_Point, 5);
        }

        /// <summary>
        ///     加载相应事件
        /// </summary>
        private void AppendEvent()
        {
            this.MouseMove += new System.Windows.Input.MouseEventHandler(Win_Shortcut_MouseMove);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(Win_Shortcut_MouseLeftButtonUp);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(Win_Shortcut_MouseLeftButtonDown);
            this.MouseRightButtonDown += new MouseButtonEventHandler(Win_Shortcut_MouseRightButtonDown);
        }

        void Win_Shortcut_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.win != null && !IsMouseDown && IsMouseLeftButtonDown)
            {
                Localtion = this.win.PointToScreen(new System.Windows.Point(0, 0));
            }
            IsMouseLeftButtonDown = false;
            IsControlPointDown = false;
        }

        private void AppendCatchWindowEvent()
        {
            if (this.win != null)
            {
                win.MouseDown += new MouseButtonEventHandler(win_MouseDown);
                win.OnRightBottomButtonDown += new EventHandler<EventArgs>(win_OnRightBottomButtonDown);
                win.OnRightBottomButtonUp += new EventHandler<EventArgs>(win_OnRightBottomButtonUp);
                win.OnLeftTopButtonDown += new EventHandler<EventArgs>(win_OnLeftTopButtonDown);
                win.OnLeftTopButtonUp += new EventHandler<EventArgs>(win_OnLeftTopButtonUp);
                win.OnTopCenterButtonDown += new EventHandler<EventArgs>(win_OnTopCenterButtonDown);
                win.OnTopCenterButtonUp += new EventHandler<EventArgs>(win_OnTopCenterButtonUp);
                win.OnRightTopButtonDown += new EventHandler<EventArgs>(win_OnRightTopButtonDown);
                win.OnRightTopButtonUp += new EventHandler<EventArgs>(win_OnRightTopButtonUp);
                win.OnLeftBottomButtonDown += new EventHandler<EventArgs>(win_OnLeftBottomButtonDown);
                win.OnLeftBottomButtonUp += new EventHandler<EventArgs>(win_OnLeftBottomButtonUp);
                win.OnBottomCenterButtonDown += new EventHandler<EventArgs>(win_OnBottomCenterButtonDown);
                win.OnBottomCenterButtonUp += new EventHandler<EventArgs>(win_OnBottomCenterButtonUp);
                win.OnLeftCenterButtonDown += new EventHandler<EventArgs>(win_OnLeftCenterButtonDown);
                win.OnLeftCenterButtonUp += new EventHandler<EventArgs>(win_OnLeftCenterButtonUp);
                win.OnRightCenterButtonDown += new EventHandler<EventArgs>(win_OnRightCenterButtonDown);
                win.OnRightCenterButtonUp += new EventHandler<EventArgs>(win_OnRightCenterButtonUp);
                win.MouseDoubleClick += new MouseButtonEventHandler(win_MouseDoubleClick);
            }
        }

        void win_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Bitmap Desktop_Bitmap = new Bitmap((int)win.Width, (int)win.Height);
            Graphics graphics = Graphics.FromImage(Desktop_Bitmap);
            System.Windows.Point pt = this.win.PointToScreen(new System.Windows.Point());
            graphics.CopyFromScreen(new System.Drawing.Point((int)pt.X,(int)pt.Y), new System.Drawing.Point(0, 0), new System.Drawing.Size((int)this.win.Width, (int)this.win.Height));
            OnOnGetImage(new OnGetImageEventArgs(Desktop_Bitmap));
            this.Close();
        }

        void Win_Shortcut_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (win == null)
            {
                win = new ScreenCatch();
                this.Cav_Back.Children.Add(win);
                win.Visibility = Visibility.Visible;
                win.Width = 0;
                win.Height = 0;
                Canvas.SetLeft(this.win, e.GetPosition(this.Cav_Back).X);
                Canvas.SetTop(this.win, e.GetPosition(this.Cav_Back).Y);
                MouseDownPoint = e.GetPosition(this.Cav_Back);
                IsMouseDown = false;
                IsMouseLeftButtonDown = true;
                AppendCatchWindowEvent();
                IsControlPointDown = false;
            }
            else
            {
                if(!IsControlPointDown)
                {
                    IsMouseLeftButtonDown = false;
                    IsMouseDown = true;
                }
            }
        }

        #region 接收控制点鼠标事件

        void win_OnRightCenterButtonUp(object sender, EventArgs e)
        {
            IsRightCenterButtonDown = false;
            IsControlPointDown = false;
            IsMouseLeftButtonDown = false;
            this.AddWidth = 0;
            this.AddHeight = 0;
        }

        void win_OnRightCenterButtonDown(object sender, EventArgs e)
        {
            IsRightCenterButtonDown = true;
            IsControlPointDown = true;
            IsMouseLeftButtonDown = true;
            this.AddWidth = 0;
            this.AddHeight = 0;
        }

        void win_OnLeftCenterButtonUp(object sender, EventArgs e)
        {
            IsLeftCenterButtonDown = false;
            IsControlPointDown = false;
            IsMouseLeftButtonDown = false;
            this.AddWidth = 0;
            this.AddHeight = 0;
        }

        void win_OnLeftCenterButtonDown(object sender, EventArgs e)
        {
            IsLeftCenterButtonDown = true;
            IsControlPointDown = true;
            IsMouseLeftButtonDown = true;
            this.AddWidth = 0;
            this.AddHeight = 0;
        }

        void win_OnBottomCenterButtonUp(object sender, EventArgs e)
        {
            IsBottomCenterButtonDown = false;
            IsControlPointDown = false;
            IsMouseLeftButtonDown = false;
            this.AddWidth = 0;
            this.AddHeight = 0;
        }

        void win_OnBottomCenterButtonDown(object sender, EventArgs e)
        {
            IsControlPointDown = true;
            IsBottomCenterButtonDown = true;
            IsMouseLeftButtonDown = true;
            this.AddWidth = 0;
            this.AddHeight = 0;
        }

        void win_OnLeftBottomButtonUp(object sender, EventArgs e)
        {
            IsLeftBottomButtonDown = false;
            IsControlPointDown = false;
            IsMouseLeftButtonDown = false;
            this.AddWidth = 0;
            this.AddHeight = 0;
        }

        void win_OnLeftBottomButtonDown(object sender, EventArgs e)
        {
            IsLeftBottomButtonDown = true;
            IsControlPointDown = true;
            IsMouseLeftButtonDown = true;
            this.AddWidth = 0;
            this.AddHeight = 0;
        }

        void win_OnRightTopButtonUp(object sender, EventArgs e)
        {
            IsRightTopButtonDown = false;
            IsControlPointDown = false;
            IsMouseLeftButtonDown = false;
            this.AddWidth = 0;
            this.AddHeight = 0;
        }

        void win_OnRightTopButtonDown(object sender, EventArgs e)
        {
            IsRightTopButtonDown = true;
            IsControlPointDown = true;
            IsMouseLeftButtonDown = true;
            this.AddWidth = 0;
            this.AddHeight = 0;
        }

        void win_OnTopCenterButtonUp(object sender, EventArgs e)
        {
            IsTopCenterButtonDown = false;
            IsControlPointDown = false;
            IsMouseLeftButtonDown = false;
            this.AddWidth = 0;
            this.AddHeight = 0;
        }

        void win_OnTopCenterButtonDown(object sender, EventArgs e)
        {
            IsTopCenterButtonDown = true;
            IsControlPointDown = true;
            IsMouseLeftButtonDown = true;
            this.AddWidth = 0;
            this.AddHeight = 0;
        }

        void win_OnLeftTopButtonUp(object sender, EventArgs e)
        {
            IsLeftTopButtonDown = false;
            IsControlPointDown = false;
            IsMouseLeftButtonDown = false;
            this.AddWidth = 0;
            this.AddHeight = 0;
        }

        void win_OnLeftTopButtonDown(object sender, EventArgs e)
        {
            IsLeftTopButtonDown = true;
            IsControlPointDown = true;
            IsMouseLeftButtonDown = true;
        }

        void win_OnRightBottomButtonUp(object sender, EventArgs e)
        {
            IsRightBottomButtonDown = false;
            IsControlPointDown = false;
            IsMouseLeftButtonDown = false;
            this.AddWidth = 0;
            this.AddHeight = 0;
        }

        void win_OnRightBottomButtonDown(object sender, EventArgs e)
        {
            IsRightBottomButtonDown = true;
            IsControlPointDown = true;
            IsMouseLeftButtonDown = true;
            this.AddWidth = 0;
            this.AddHeight = 0;
        }

        #endregion

        void Win_Shortcut_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.win != null)
            {
                this.win.Visibility = Visibility.Hidden;
                this.win = null;
                IsLeftTopButtonDown = false;
                IsTopCenterButtonDown = false;
                IsRightTopButtonDown = false;
                IsLeftBottomButtonDown = false;
                IsBottomCenterButtonDown = false;
                IsRightBottomButtonDown = false;
                IsLeftCenterButtonDown = false;
                IsRightCenterButtonDown = false;
                this.AddWidth = 0;
                this.AddHeight = 0;
            }
            else
            {
                //触发退出事件
                OnOnClosed(new EventArgs());
                this.Close();
            }
        }

        void win_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.IsMouseDown = true;
            }
        }

        void Win_Shortcut_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.tb_Point.Text = e.GetPosition(this.Cav_Back).X + " , " + e.GetPosition(this.Cav_Back).Y;
            //移动捕获窗口
            if (this.IsMouseDown && e.LeftButton == MouseButtonState.Pressed && this.win != null && !IsControlPointDown)
            {
                Canvas.SetLeft(this.win, e.GetPosition(this.Cav_Back).X - this.win.Width / 2.00);
                Canvas.SetTop(this.win, e.GetPosition(this.Cav_Back).Y - this.win.Height / 2.00);
                Localtion = this.win.PointToScreen(new System.Windows.Point(0, 0));
            }
            //改变捕获窗口大小
            else if (this.IsMouseLeftButtonDown && win != null && e.LeftButton == MouseButtonState.Pressed && !IsControlPointDown)
            {
                this.win.Width = e.GetPosition(this.Cav_Back).X - this.MouseDownPoint.X;
                this.win.Height = e.GetPosition(this.Cav_Back).Y - this.MouseDownPoint.Y;
            }
            //缩放捕获窗口
            else if (this.IsMouseLeftButtonDown && win != null && e.LeftButton == MouseButtonState.Pressed && IsControlPointDown)
            {
                //////////////////////////////////////////////////////////////////////////
                ///                        扩展
                //////////////////////////////////////////////////////////////////////////
            }
        }

        /// <summary>
        ///     获得当前桌面截图拷贝
        /// </summary>
        /// <returns>
        ///     返回当前桌面截图Bitmap对象
        /// </returns>
        public Bitmap GetDesktop()
        {
            Bitmap Desktop_Bitmap = new Bitmap((int)this.Width, (int)this.Height);
            Graphics graphics = Graphics.FromImage(Desktop_Bitmap);
            graphics.CopyFromScreen(new System.Drawing.Point(0, 0), new System.Drawing.Point(0, 0), new System.Drawing.Size((int)this.Width, (int)this.Height));
            return Desktop_Bitmap;
        }





        #region 'OnClosed' event definition code

        // Private delegate linked list (explicitly defined)
        private EventHandler<System.EventArgs> OnClosedEventHandlerDelegate;

        /// <summary>
        ///     TODO: Describe the purpose of this event here
        /// </summary>
        public event EventHandler<System.EventArgs> OnClosed
        {
            // Explicit event definition with accessor methods
            add
            {
                OnClosedEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Combine(OnClosedEventHandlerDelegate, value);
            }
            remove
            {
                OnClosedEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Remove(OnClosedEventHandlerDelegate, value);
            }
        }

        /// <summary>
        ///     This is the method that is responsible for notifying
        ///     receivers that the event occurred
        /// </summary>
        protected virtual void OnOnClosed(System.EventArgs e)
        {
            if (OnClosedEventHandlerDelegate != null)
            {
                OnClosedEventHandlerDelegate(this, e);
            }
        }

        #endregion //('OnClosed' event definition code)


        #region 'OnGetImage' event definition code

        /// <summary>
        ///     EventArgs derived type which holds the custom event fields
        /// </summary>
        public class OnGetImageEventArgs : System.EventArgs
        {
            /// <summary>
            ///     Use this constructor to initialize the event arguments
            ///     object with the custom event fields
            /// </summary>
            public OnGetImageEventArgs(Bitmap bitScreenImage)
            {
                this.ScreenImage = bitScreenImage;
            }

            /// <summary>
            ///     TODO: Describe the purpose of this event argument here
            /// </summary>
            public readonly Bitmap ScreenImage;

        }

        // Private delegate linked list (explicitly defined)
        private EventHandler<OnGetImageEventArgs> OnGetImageEventHandlerDelegate;

        /// <summary>
        ///     TODO: Describe the purpose of this event here
        /// </summary>
        public event EventHandler<OnGetImageEventArgs> OnGetImage
        {
            // Explicit event definition with accessor methods
            add
            {
                OnGetImageEventHandlerDelegate = (EventHandler<OnGetImageEventArgs>)Delegate.Combine(OnGetImageEventHandlerDelegate, value);
            }
            remove
            {
                OnGetImageEventHandlerDelegate = (EventHandler<OnGetImageEventArgs>)Delegate.Remove(OnGetImageEventHandlerDelegate, value);
            }
        }

        /// <summary>
        ///     This is the method that is responsible for notifying
        ///     receivers that the event occurred
        /// </summary>
        protected virtual void OnOnGetImage(OnGetImageEventArgs e)
        {
            if (OnGetImageEventHandlerDelegate != null)
            {
                OnGetImageEventHandlerDelegate(this, e);
            }
        }

        #endregion //('OnGetImage' event definition code)

    }
}
