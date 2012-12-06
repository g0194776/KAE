using System;
using KJFramework.Mono.Game.Components.Scenarios;
using XnaTouch.Framework;

namespace KJFramework.Mono.Game.Components.Controls
{
    /// <summary>
    ///     游戏控制元件，所有控制组建的抽象父类，提供了相关的基本操作。
    /// </summary>
    public class Control : DrawableGameComponent
    {
        #region Constructor

        /// <summary>
        ///     游戏控制元件，所有控制组建的抽象父类，提供了相关的基本操作。
        /// </summary>
        /// <param name="game">游戏对象</param>
        public Control(XnaTouch.Framework.Game game)
            : base(game)
        {
        }

        #endregion

        #region Base Methods

        /// <summary>
        ///     更新
        /// </summary>
        /// <param name="gameTime">游戏时间</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        ///     初始化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            InitializingHandler(null);
        }

        /// <summary>
        ///     加载
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            LoadingHandler(null);
        }

        /// <summary>
        ///     卸载
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();
            UnLoadingHandler(null);
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置组件的关键字
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        ///     获取或设置所属的场景
        /// </summary>
        public IScenario OwnScenario { get; set; } 

        #endregion

        #region Events

        /// <summary>
        ///     初始化
        /// </summary>
        public event EventHandler Initializing;
        protected void InitializingHandler(EventArgs e)
        {
            EventHandler handler = Initializing;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     加载
        /// </summary>
        public event EventHandler Loading;
        protected void LoadingHandler(EventArgs e)
        {
            EventHandler handler = Loading;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     卸载
        /// </summary>
        public event EventHandler UnLoading;
        protected void UnLoadingHandler(EventArgs e)
        {
            EventHandler handler = UnLoading;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}