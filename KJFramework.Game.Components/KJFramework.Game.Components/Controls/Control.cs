using System;
using KJFramework.EventArgs;
using KJFramework.Game.Components.EventArgs;
using KJFramework.Game.Components.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KJFramework.Game.Components.Controls
{
    /// <summary>
    ///     XNA元控件，提供了相关的基本属性结构，以及相关的基本操作。
    /// </summary>
    public abstract class Control : DrawableGameComponent
    {
        #region 构造函数

        /// <summary>
        ///     XNA元控件，提供了相关的基本属性结构，以及相关的基本操作。
        /// </summary>
        public Control(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            Initialize();
        }

        #endregion

        #region 父类方法

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn.  Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime).</param>
        public abstract override void Draw(GameTime gameTime);

        /// <summary>
        /// Called when graphics resources need to be loaded. Override this method to load any component-specific graphics resources. 
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            if (SpriteBatch == null) 
                SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// Initializes the component.  Override this method to load any non-graphics resources and query for any required services.
        /// </summary>
        public override void Initialize()
        {
            _eventChain = new EventChain(this)
                .Regist(new KeyDownEventHook())
                .Regist(new MouseIntersectEventHook())
                .Regist(new MouseLeftButtonEventHook())
                .Regist(new MouseRightButtonEventHook());
            LoadedHandler(new System.EventArgs());
            base.Initialize();
            LoadContent();
        }

        /// <summary>
        /// Called when graphics resources need to be unloaded. Override this method to unload any component-specific graphics resources. 
        /// </summary>
        protected override void UnloadContent()
        {
            UnLoadedHandler(new System.EventArgs());
            base.UnloadContent();
        }

        /// <summary>
        /// Called when the GameComponent needs to be updated.  Override this method with component-specific update code.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to Microsoft.Xna.Framework.GameComponent.Update(Microsoft.Xna.Framework.GameTime)</param>
        public override void Update(GameTime gameTime)
        {
            if (!Enabled) return;
            _eventChain.Execute(gameTime);
        }

        #endregion

        #region 成员

        #region 私有属性

        internal Keys PreKeys;
        private String _name;
        private Object _tag;
        /// <summary>
        /// The amount of _time in seconds that the current frame has been shown for.
        /// </summary>
        internal float WaitTime;
        internal float PreTime;

        #endregion

        #region 受保护属性

        protected float _width;
        protected float _height;
        internal bool IsCapsLock;
        protected Rectangle _area;
        protected Vector2 _position;
        internal bool IsLeftMouseButtonPressed;
        internal bool IsRightMouseButtonPressed;
        internal bool IsMouseEnter;
        internal bool Getfocus;
        protected SpriteBatch _spriteBatch;
        internal MouseState CurrentMouseState;
        internal Vector2 PreMouseDragPosition = Vector2.Zero;
        protected IEventChain _eventChain;

        #endregion

        #region 公共属性

        /// <summary>
        ///     获取或设置控件定位
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                _area = new Rectangle((int) _position.X, (int) _position.Y, (int) _width, (int) _height);
            }
        }

        /// <summary>
        ///     获取或设置控件宽度
        /// </summary>
        public float Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        ///     获取或设置控件高度
        /// </summary>
        public float Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        ///     获取控件面积范围
        /// </summary>
        public Rectangle Area
        {
            get { return _area; }
        }

        /// <summary>
        ///     获取或设置精灵组
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
            set { _spriteBatch = value; }
        }

        /// <summary>
        ///     获取或设置名称
        /// </summary>
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        ///     获取或设置附属属性
        /// </summary>
        public Object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        #endregion

        #endregion

        #region 事件

        /// <summary>
        ///     鼠标左键单击事件
        /// </summary>
        public event EventHandler LeftButtonClicked;
        /// <summary>
        ///     鼠标右键单击事件
        /// </summary>
        public event EventHandler RightButtonClicked;
        /// <summary>
        ///     鼠标左键松开事件
        /// </summary>
        public event EventHandler LeftButtonRelease;
        /// <summary>
        ///     鼠标右键松开事件
        /// </summary>
        public event EventHandler RightButtonRelease;
        /// <summary>
        ///     鼠标移入控件事件
        /// </summary>
        public event EventHandler MouseEnter;
        /// <summary>
        ///     鼠标移出控件事件
        /// </summary>
        public event EventHandler MouseLeave;
        /// <summary>
        ///     鼠标拖拽事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<Vector2>> MouseDrag;
        /// <summary>
        ///     加载事件
        /// </summary>
        public event EventHandler Loaded;
        /// <summary>
        ///     卸载事件
        /// </summary>
        public event EventHandler UnLoaded;
        /// <summary>
        ///     获取焦点事件
        /// </summary>
        public event EventHandler GetFocused;
        /// <summary>
        ///     按键事件
        /// </summary>
        public event EventHandler<KeyDownEventArgs> KeyDown;
        /// <summary>
        ///     失去焦点事件
        /// </summary>
        public event EventHandler LostForused;

        internal void KeyDownHandler(KeyDownEventArgs e)
        {
            EventHandler<KeyDownEventArgs> down = KeyDown;
            if (down != null) down(this, e);
        }

        internal void GetFocusedHandler(System.EventArgs e)
        {
            EventHandler focused = GetFocused;
            if (focused != null) focused(this, e);
        }

        internal void LostForusedHandler(System.EventArgs e)
        {
            EventHandler forused = LostForused;
            if (forused != null) forused(this, e);
        }

        internal void UnLoadedHandler(System.EventArgs e)
        {
            EventHandler loaded = UnLoaded;
            if (loaded != null) loaded(this, e);
        }

        internal void LoadedHandler(System.EventArgs e)
        {
            EventHandler loaded = Loaded;
            if (loaded != null) loaded(this, e);
        }

        internal void MouseLeaveHandler(System.EventArgs e)
        {
            EventHandler leave = MouseLeave;
            if (leave != null) leave(this, e);
        }

        internal void MouseEnterHandler(System.EventArgs e)
        {
            EventHandler enter = MouseEnter;
            if (enter != null) enter(this, e);
        }

        internal void LeftClickedHandler(System.EventArgs e)
        {
            EventHandler clicked = LeftButtonClicked;
            if (clicked != null) clicked(this, e);    
        }

        internal void RightClickedHandler(System.EventArgs e)
        {
            EventHandler clicked = RightButtonClicked;
            if (clicked != null) clicked(this, e);
        }

        internal void LeftButtonReleaseHandler(System.EventArgs e)
        {
            EventHandler release = LeftButtonRelease;
            if (release != null) release(this, e);
        }

        internal void RightButtonReleaseHandler(System.EventArgs e)
        {
            EventHandler release = RightButtonRelease;
            if (release != null) release(this, e);
        }

        internal void MouseDragHandler(LightSingleArgEventArgs<Vector2> e)
        {
            EventHandler<LightSingleArgEventArgs<Vector2>> handler = MouseDrag;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}