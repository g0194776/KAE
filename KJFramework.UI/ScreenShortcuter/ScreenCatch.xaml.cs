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

namespace KJFramework.UI.ScreenShorter
{
    /// <summary>
    /// ScreenCatch.xaml 的交互逻辑
    /// </summary>
    public partial class ScreenCatch : UserControl
    {
        public ScreenCatch()
        {
            InitializeComponent();
            AppendEvent();
        }

        /// <summary>
        ///     加载相应事件
        /// </summary>
        private void AppendEvent()
        {
            //左上点
            this.btn_LeftTop.MouseEnter += new MouseEventHandler(btn_LeftTop_MouseEnter);
            this.btn_LeftTop.MouseLeave += new MouseEventHandler(btn_LeftTop_MouseLeave);
            //右上点
            this.btn_RightTop.MouseEnter += new MouseEventHandler(btn_RightTop_MouseEnter);
            this.btn_RightTop.MouseLeave +=new MouseEventHandler(btn_RightTop_MouseLeave);
            //左下点
            this.btn_LeftBottom.MouseEnter += new MouseEventHandler(btn_LeftBottom_MouseEnter);
            this.btn_LeftBottom.MouseLeave += new MouseEventHandler(btn_LeftBottom_MouseLeave);
            btn_LeftBottom.MouseUp += new MouseButtonEventHandler(btn_LeftBottom_MouseUp);
            //右下点
            this.btn_RightBottom.MouseEnter += new MouseEventHandler(btn_RightBottom_MouseEnter);
            this.btn_RightBottom.MouseLeave += new MouseEventHandler(btn_RightBottom_MouseLeave);
            //上中点
            this.btn_TopCenter.MouseEnter += new MouseEventHandler(btn_TopCenter_MouseEnter);
            this.btn_TopCenter.MouseLeave += new MouseEventHandler(btn_TopCenter_MouseLeave);
            //下中点
            this.btn_BottomCenter.MouseEnter += new MouseEventHandler(btn_BottomCenter_MouseEnter);
            this.btn_BottomCenter.MouseLeave += new MouseEventHandler(btn_BottomCenter_MouseLeave);
            //左中点
            this.btn_LeftCenter.MouseEnter += new MouseEventHandler(btn_LeftCenter_MouseEnter);
            this.btn_LeftCenter.MouseLeave += new MouseEventHandler(btn_LeftCenter_MouseLeave);
            //右中点
            this.btn_RightCenter.MouseEnter += new MouseEventHandler(btn_RightCenter_MouseEnter);
            this.btn_RightCenter.MouseLeave += new MouseEventHandler(btn_RightCenter_MouseLeave);
        }

        void btn_LeftBottom_MouseUp(object sender, MouseButtonEventArgs e)
        {
            OnOnLeftBottomButtonUp(new EventArgs());
        }

        #region 鼠标指针效果

        void btn_RightCenter_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        void btn_RightCenter_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.SizeWE;
        }

        void btn_LeftCenter_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        void btn_LeftCenter_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.SizeWE;
        }

        void btn_BottomCenter_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        void btn_BottomCenter_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.SizeNS;
        }

        void btn_TopCenter_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        void btn_TopCenter_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.SizeNS;
        }

        void btn_RightBottom_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        void btn_RightBottom_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.SizeNWSE;
        }

        void btn_LeftBottom_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        void btn_LeftBottom_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.SizeNESW;
        }

        void btn_RightTop_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        void btn_RightTop_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.SizeNESW;
        }

        void btn_LeftTop_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        void btn_LeftTop_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.SizeNWSE;
        }

        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //this.ReSet();
        }

        /// <summary>
        ///     重新摆放外轮廓原点坐标
        /// </summary>
        public void ReSet()
        {
            double Width = this.Width;
            double Height = this.Height;
            #region 开始摆放定点

            double _Width = Width - (Width * 2);
            double _Height = Height - (Height * 2);
            //左上点
            this.btn_LeftTop.Margin = new Thickness(_Width, _Height, 0, 0);
            //右上点
            this.btn_RightTop.Margin = new Thickness(0, _Height, _Width, 0);
            //左下点
            this.btn_LeftBottom.Margin = new Thickness(_Width, 0, 0, _Height);
            //右下点
            this.btn_RightBottom.Margin = new Thickness(0, 0, _Width, _Height);
            //上中点
            this.btn_TopCenter.Margin = new Thickness(_Width / 2.00, _Height, _Width / 2.00, 0);
            //下中点
            this.btn_BottomCenter.Margin = new Thickness(_Width / 2.00, 0, _Width / 2.00, _Height);
            //左中点
            this.btn_LeftCenter.Margin = new Thickness(_Width, _Height / 2.00, 0, _Height / 2.00);
            //右中点
            this.btn_RightCenter.Margin = new Thickness(0, _Height / 2.00, _Width, _Height / 2.00);

            #endregion
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.ReSet();
        }

        #region 定义按钮鼠标事件

        #region 'OnLeftTopButtonDown' event definition code

        // Private delegate linked list (explicitly defined)
        private EventHandler<System.EventArgs> OnLeftTopButtonDownEventHandlerDelegate;

        /// <summary>
        ///     TODO: Describe the purpose of this event here
        /// </summary>
        public event EventHandler<System.EventArgs> OnLeftTopButtonDown
        {
            // Explicit event definition with accessor methods
            add
            {
                OnLeftTopButtonDownEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Combine(OnLeftTopButtonDownEventHandlerDelegate, value);
            }
            remove
            {
                OnLeftTopButtonDownEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Remove(OnLeftTopButtonDownEventHandlerDelegate, value);
            }
        }

        /// <summary>
        ///     This is the method that is responsible for notifying
        ///     receivers that the event occurred
        /// </summary>
        protected virtual void OnOnLeftTopButtonDown(System.EventArgs e)
        {
            if (OnLeftTopButtonDownEventHandlerDelegate != null)
            {
                OnLeftTopButtonDownEventHandlerDelegate(this, e);
            }
        }

        #endregion //('OnLeftTopButtonDown' event definition code)


        #region 'OnTopCenterButtonDown' event definition code

        // Private delegate linked list (explicitly defined)
        private EventHandler<System.EventArgs> OnTopCenterButtonDownEventHandlerDelegate;

        /// <summary>
        ///     TODO: Describe the purpose of this event here
        /// </summary>
        public event EventHandler<System.EventArgs> OnTopCenterButtonDown
        {
            // Explicit event definition with accessor methods
            add
            {
                OnTopCenterButtonDownEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Combine(OnTopCenterButtonDownEventHandlerDelegate, value);
            }
            remove
            {
                OnTopCenterButtonDownEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Remove(OnTopCenterButtonDownEventHandlerDelegate, value);
            }
        }

        /// <summary>
        ///     This is the method that is responsible for notifying
        ///     receivers that the event occurred
        /// </summary>
        protected virtual void OnOnTopCenterButtonDown(System.EventArgs e)
        {
            if (OnTopCenterButtonDownEventHandlerDelegate != null)
            {
                OnTopCenterButtonDownEventHandlerDelegate(this, e);
            }
        }

        #endregion //('OnTopCenterButtonDown' event definition code)


        #region 'OnRightTopButtonDown' event definition code

        // Private delegate linked list (explicitly defined)
        private EventHandler<System.EventArgs> OnRightTopButtonDownEventHandlerDelegate;

        /// <summary>
        ///     TODO: Describe the purpose of this event here
        /// </summary>
        public event EventHandler<System.EventArgs> OnRightTopButtonDown
        {
            // Explicit event definition with accessor methods
            add
            {
                OnRightTopButtonDownEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Combine(OnRightTopButtonDownEventHandlerDelegate, value);
            }
            remove
            {
                OnRightTopButtonDownEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Remove(OnRightTopButtonDownEventHandlerDelegate, value);
            }
        }

        /// <summary>
        ///     This is the method that is responsible for notifying
        ///     receivers that the event occurred
        /// </summary>
        protected virtual void OnOnRightTopButtonDown(System.EventArgs e)
        {
            if (OnRightTopButtonDownEventHandlerDelegate != null)
            {
                OnRightTopButtonDownEventHandlerDelegate(this, e);
            }
        }

        #endregion //('OnRightTopButtonDown' event definition code)


        #region 'OnLeftBottomButtonDown' event definition code

        // Private delegate linked list (explicitly defined)
        private EventHandler<System.EventArgs> OnLeftBottomButtonDownEventHandlerDelegate;

        /// <summary>
        ///     TODO: Describe the purpose of this event here
        /// </summary>
        public event EventHandler<System.EventArgs> OnLeftBottomButtonDown
        {
            // Explicit event definition with accessor methods
            add
            {
                OnLeftBottomButtonDownEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Combine(OnLeftBottomButtonDownEventHandlerDelegate, value);
            }
            remove
            {
                OnLeftBottomButtonDownEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Remove(OnLeftBottomButtonDownEventHandlerDelegate, value);
            }
        }

        /// <summary>
        ///     This is the method that is responsible for notifying
        ///     receivers that the event occurred
        /// </summary>
        protected virtual void OnOnLeftBottomButtonDown(System.EventArgs e)
        {
            if (OnLeftBottomButtonDownEventHandlerDelegate != null)
            {
                OnLeftBottomButtonDownEventHandlerDelegate(this, e);
            }
        }

        #endregion //('OnLeftBottomButtonDown' event definition code)


        #region 'OnBottomCenterButtonDown' event definition code

        // Private delegate linked list (explicitly defined)
        private EventHandler<System.EventArgs> OnBottomCenterButtonDownEventHandlerDelegate;

        /// <summary>
        ///     TODO: Describe the purpose of this event here
        /// </summary>
        public event EventHandler<System.EventArgs> OnBottomCenterButtonDown
        {
            // Explicit event definition with accessor methods
            add
            {
                OnBottomCenterButtonDownEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Combine(OnBottomCenterButtonDownEventHandlerDelegate, value);
            }
            remove
            {
                OnBottomCenterButtonDownEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Remove(OnBottomCenterButtonDownEventHandlerDelegate, value);
            }
        }

        /// <summary>
        ///     This is the method that is responsible for notifying
        ///     receivers that the event occurred
        /// </summary>
        protected virtual void OnOnBottomCenterButtonDown(System.EventArgs e)
        {
            if (OnBottomCenterButtonDownEventHandlerDelegate != null)
            {
                OnBottomCenterButtonDownEventHandlerDelegate(this, e);
            }
        }

        #endregion //('OnBottomCenterButtonDown' event definition code)


        #region 'OnRightBottomButtonDown' event definition code

        // Private delegate linked list (explicitly defined)
        private EventHandler<System.EventArgs> OnRightBottomButtonDownEventHandlerDelegate;

        /// <summary>
        ///     TODO: Describe the purpose of this event here
        /// </summary>
        public event EventHandler<System.EventArgs> OnRightBottomButtonDown
        {
            // Explicit event definition with accessor methods
            add
            {
                OnRightBottomButtonDownEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Combine(OnRightBottomButtonDownEventHandlerDelegate, value);
            }
            remove
            {
                OnRightBottomButtonDownEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Remove(OnRightBottomButtonDownEventHandlerDelegate, value);
            }
        }

        /// <summary>
        ///     This is the method that is responsible for notifying
        ///     receivers that the event occurred
        /// </summary>
        protected virtual void OnOnRightBottomButtonDown(System.EventArgs e)
        {
            if (OnRightBottomButtonDownEventHandlerDelegate != null)
            {
                OnRightBottomButtonDownEventHandlerDelegate(this, e);
            }
        }

        #endregion //('OnRightBottomButtonDown' event definition code)


        #region 'OnLeftCenterButtonDown' event definition code

        // Private delegate linked list (explicitly defined)
        private EventHandler<System.EventArgs> OnLeftCenterButtonDownEventHandlerDelegate;

        /// <summary>
        ///     TODO: Describe the purpose of this event here
        /// </summary>
        public event EventHandler<System.EventArgs> OnLeftCenterButtonDown
        {
            // Explicit event definition with accessor methods
            add
            {
                OnLeftCenterButtonDownEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Combine(OnLeftCenterButtonDownEventHandlerDelegate, value);
            }
            remove
            {
                OnLeftCenterButtonDownEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Remove(OnLeftCenterButtonDownEventHandlerDelegate, value);
            }
        }

        /// <summary>
        ///     This is the method that is responsible for notifying
        ///     receivers that the event occurred
        /// </summary>
        protected virtual void OnOnLeftCenterButtonDown(System.EventArgs e)
        {
            if (OnLeftCenterButtonDownEventHandlerDelegate != null)
            {
                OnLeftCenterButtonDownEventHandlerDelegate(this, e);
            }
        }

        #endregion //('OnLeftCenterButtonDown' event definition code)


        #region 'OnRightCenterButtonDown' event definition code

        // Private delegate linked list (explicitly defined)
        private EventHandler<System.EventArgs> OnRightCenterButtonDownEventHandlerDelegate;

        /// <summary>
        ///     TODO: Describe the purpose of this event here
        /// </summary>
        public event EventHandler<System.EventArgs> OnRightCenterButtonDown
        {
            // Explicit event definition with accessor methods
            add
            {
                OnRightCenterButtonDownEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Combine(OnRightCenterButtonDownEventHandlerDelegate, value);
            }
            remove
            {
                OnRightCenterButtonDownEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Remove(OnRightCenterButtonDownEventHandlerDelegate, value);
            }
        }

        /// <summary>
        ///     This is the method that is responsible for notifying
        ///     receivers that the event occurred
        /// </summary>
        protected virtual void OnOnRightCenterButtonDown(System.EventArgs e)
        {
            if (OnRightCenterButtonDownEventHandlerDelegate != null)
            {
                OnRightCenterButtonDownEventHandlerDelegate(this, e);
            }
        }

        #endregion //('OnRightCenterButtonDown' event definition code)


        #region 'OnLeftTopButtonUp' event definition code

        // Private delegate linked list (explicitly defined)
        private EventHandler<System.EventArgs> OnLeftTopButtonUpEventHandlerDelegate;

        /// <summary>
        ///     TODO: Describe the purpose of this event here
        /// </summary>
        public event EventHandler<System.EventArgs> OnLeftTopButtonUp
        {
            // Explicit event definition with accessor methods
            add
            {
                OnLeftTopButtonUpEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Combine(OnLeftTopButtonUpEventHandlerDelegate, value);
            }
            remove
            {
                OnLeftTopButtonUpEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Remove(OnLeftTopButtonUpEventHandlerDelegate, value);
            }
        }

        /// <summary>
        ///     This is the method that is responsible for notifying
        ///     receivers that the event occurred
        /// </summary>
        protected virtual void OnOnLeftTopButtonUp(System.EventArgs e)
        {
            if (OnLeftTopButtonUpEventHandlerDelegate != null)
            {
                OnLeftTopButtonUpEventHandlerDelegate(this, e);
            }
        }

        #endregion //('OnLeftTopButtonUp' event definition code)


        #region 'OnTopCenterButtonUp' event definition code

        // Private delegate linked list (explicitly defined)
        private EventHandler<System.EventArgs> OnTopCenterButtonUpEventHandlerDelegate;

        /// <summary>
        ///     TODO: Describe the purpose of this event here
        /// </summary>
        public event EventHandler<System.EventArgs> OnTopCenterButtonUp
        {
            // Explicit event definition with accessor methods
            add
            {
                OnTopCenterButtonUpEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Combine(OnTopCenterButtonUpEventHandlerDelegate, value);
            }
            remove
            {
                OnTopCenterButtonUpEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Remove(OnTopCenterButtonUpEventHandlerDelegate, value);
            }
        }

        /// <summary>
        ///     This is the method that is responsible for notifying
        ///     receivers that the event occurred
        /// </summary>
        protected virtual void OnOnTopCenterButtonUp(System.EventArgs e)
        {
            if (OnTopCenterButtonUpEventHandlerDelegate != null)
            {
                OnTopCenterButtonUpEventHandlerDelegate(this, e);
            }
        }

        #endregion //('OnTopCenterButtonUp' event definition code)


        #region 'OnRightTopButtonUp' event definition code

        // Private delegate linked list (explicitly defined)
        private EventHandler<System.EventArgs> OnRightTopButtonUpEventHandlerDelegate;

        /// <summary>
        ///     TODO: Describe the purpose of this event here
        /// </summary>
        public event EventHandler<System.EventArgs> OnRightTopButtonUp
        {
            // Explicit event definition with accessor methods
            add
            {
                OnRightTopButtonUpEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Combine(OnRightTopButtonUpEventHandlerDelegate, value);
            }
            remove
            {
                OnRightTopButtonUpEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Remove(OnRightTopButtonUpEventHandlerDelegate, value);
            }
        }

        /// <summary>
        ///     This is the method that is responsible for notifying
        ///     receivers that the event occurred
        /// </summary>
        protected virtual void OnOnRightTopButtonUp(System.EventArgs e)
        {
            if (OnRightTopButtonUpEventHandlerDelegate != null)
            {
                OnRightTopButtonUpEventHandlerDelegate(this, e);
            }
        }

        #endregion //('OnRightTopButtonUp' event definition code)


        #region 'OnLeftBottomButtonUp' event definition code

        // Private delegate linked list (explicitly defined)
        private EventHandler<System.EventArgs> OnLeftBottomButtonUpEventHandlerDelegate;

        /// <summary>
        ///     TODO: Describe the purpose of this event here
        /// </summary>
        public event EventHandler<System.EventArgs> OnLeftBottomButtonUp
        {
            // Explicit event definition with accessor methods
            add
            {
                OnLeftBottomButtonUpEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Combine(OnLeftBottomButtonUpEventHandlerDelegate, value);
            }
            remove
            {
                OnLeftBottomButtonUpEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Remove(OnLeftBottomButtonUpEventHandlerDelegate, value);
            }
        }

        /// <summary>
        ///     This is the method that is responsible for notifying
        ///     receivers that the event occurred
        /// </summary>
        protected virtual void OnOnLeftBottomButtonUp(System.EventArgs e)
        {
            if (OnLeftBottomButtonUpEventHandlerDelegate != null)
            {
                OnLeftBottomButtonUpEventHandlerDelegate(this, e);
            }
        }

        #endregion //('OnLeftBottomButtonUp' event definition code)


        #region 'OnBottomCenterButtonUp' event definition code

        // Private delegate linked list (explicitly defined)
        private EventHandler<System.EventArgs> OnBottomCenterButtonUpEventHandlerDelegate;

        /// <summary>
        ///     TODO: Describe the purpose of this event here
        /// </summary>
        public event EventHandler<System.EventArgs> OnBottomCenterButtonUp
        {
            // Explicit event definition with accessor methods
            add
            {
                OnBottomCenterButtonUpEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Combine(OnBottomCenterButtonUpEventHandlerDelegate, value);
            }
            remove
            {
                OnBottomCenterButtonUpEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Remove(OnBottomCenterButtonUpEventHandlerDelegate, value);
            }
        }

        /// <summary>
        ///     This is the method that is responsible for notifying
        ///     receivers that the event occurred
        /// </summary>
        protected virtual void OnOnBottomCenterButtonUp(System.EventArgs e)
        {
            if (OnBottomCenterButtonUpEventHandlerDelegate != null)
            {
                OnBottomCenterButtonUpEventHandlerDelegate(this, e);
            }
        }

        #endregion //('OnBottomCenterButtonUp' event definition code)


        #region 'OnRightBottomButtonUp' event definition code

        // Private delegate linked list (explicitly defined)
        private EventHandler<System.EventArgs> OnRightBottomButtonUpEventHandlerDelegate;

        /// <summary>
        ///     TODO: Describe the purpose of this event here
        /// </summary>
        public event EventHandler<System.EventArgs> OnRightBottomButtonUp
        {
            // Explicit event definition with accessor methods
            add
            {
                OnRightBottomButtonUpEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Combine(OnRightBottomButtonUpEventHandlerDelegate, value);
            }
            remove
            {
                OnRightBottomButtonUpEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Remove(OnRightBottomButtonUpEventHandlerDelegate, value);
            }
        }

        /// <summary>
        ///     This is the method that is responsible for notifying
        ///     receivers that the event occurred
        /// </summary>
        protected virtual void OnOnRightBottomButtonUp(System.EventArgs e)
        {
            if (OnRightBottomButtonUpEventHandlerDelegate != null)
            {
                OnRightBottomButtonUpEventHandlerDelegate(this, e);
            }
        }

        #endregion //('OnRightBottomButtonUp' event definition code)


        #region 'OnLeftCenterButtonUp' event definition code

        // Private delegate linked list (explicitly defined)
        private EventHandler<System.EventArgs> OnLeftCenterButtonUpEventHandlerDelegate;

        /// <summary>
        ///     TODO: Describe the purpose of this event here
        /// </summary>
        public event EventHandler<System.EventArgs> OnLeftCenterButtonUp
        {
            // Explicit event definition with accessor methods
            add
            {
                OnLeftCenterButtonUpEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Combine(OnLeftCenterButtonUpEventHandlerDelegate, value);
            }
            remove
            {
                OnLeftCenterButtonUpEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Remove(OnLeftCenterButtonUpEventHandlerDelegate, value);
            }
        }

        /// <summary>
        ///     This is the method that is responsible for notifying
        ///     receivers that the event occurred
        /// </summary>
        protected virtual void OnOnLeftCenterButtonUp(System.EventArgs e)
        {
            if (OnLeftCenterButtonUpEventHandlerDelegate != null)
            {
                OnLeftCenterButtonUpEventHandlerDelegate(this, e);
            }
        }

        #endregion //('OnLeftCenterButtonUp' event definition code)


        #region 'OnRightCenterButtonUp' event definition code

        // Private delegate linked list (explicitly defined)
        private EventHandler<System.EventArgs> OnRightCenterButtonUpEventHandlerDelegate;

        /// <summary>
        ///     TODO: Describe the purpose of this event here
        /// </summary>
        public event EventHandler<System.EventArgs> OnRightCenterButtonUp
        {
            // Explicit event definition with accessor methods
            add
            {
                OnRightCenterButtonUpEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Combine(OnRightCenterButtonUpEventHandlerDelegate, value);
            }
            remove
            {
                OnRightCenterButtonUpEventHandlerDelegate = (EventHandler<System.EventArgs>)Delegate.Remove(OnRightCenterButtonUpEventHandlerDelegate, value);
            }
        }

        /// <summary>
        ///     This is the method that is responsible for notifying
        ///     receivers that the event occurred
        /// </summary>
        protected virtual void OnOnRightCenterButtonUp(System.EventArgs e)
        {
            if (OnRightCenterButtonUpEventHandlerDelegate != null)
            {
                OnRightCenterButtonUpEventHandlerDelegate(this, e);
            }
        }

        #endregion //('OnRightCenterButtonUp' event definition code)

        #endregion

        #region 鼠标响应事件

        private void btn_LeftTop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnOnLeftTopButtonDown(new EventArgs());
        }

        private void btn_LeftTop_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OnOnLeftTopButtonUp(new EventArgs());
        }

        private void btn_TopCenter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnOnTopCenterButtonDown(new EventArgs());
        }

        private void btn_TopCenter_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OnOnTopCenterButtonUp(new EventArgs());
        }

        private void btn_RightTop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnOnRightTopButtonDown(new EventArgs());
        }

        private void btn_RightTop_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OnOnRightTopButtonUp(new EventArgs());
        }

        private void btn_LeftBottom_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnOnLeftBottomButtonDown(new EventArgs());
        }

        private void btn_LeftBottom_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OnOnLeftBottomButtonUp(new EventArgs());
        }

        private void btn_BottomCenter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnOnBottomCenterButtonDown(new EventArgs());
        }

        private void btn_BottomCenter_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OnOnBottomCenterButtonUp(new EventArgs());
        }

        private void btn_RightBottom_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnOnRightBottomButtonDown(new EventArgs());
        }

        private void btn_RightBottom_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OnOnRightBottomButtonUp(new EventArgs());
        }

        private void btn_LeftCenter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnOnLeftCenterButtonDown(new EventArgs());
        }

        private void btn_LeftCenter_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OnOnLeftCenterButtonUp(new EventArgs());
        }

        private void btn_RightCenter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnOnRightCenterButtonDown(new EventArgs());
        }

        private void btn_RightCenter_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OnOnRightCenterButtonUp(new EventArgs());
        }

        #endregion
    }
}
