using System;
using System.Collections.Concurrent;
using KJFramework.Game.Components.Scenarios;
using Microsoft.Xna.Framework;

namespace KJFramework.Game.Components.Actions
{
    /// <summary>
    ///     动作抽象父类，提供了相关的基本操作。
    /// </summary>
    public abstract class Action<TScenario> : DrawableGameComponent ,IAction<TScenario>
        where TScenario : Scenario
    {
        #region 构造函数

        /// <summary>
        ///     动作抽象父类，提供了相关的基本操作。
        /// </summary>
        protected Action(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
        }

        #endregion

        #region 成员

        protected readonly ConcurrentDictionary<String, TScenario> _scenarios = new ConcurrentDictionary<string, TScenario>();
        private TScenario _playScenario;

        #endregion

        #region IAction Members

        protected bool _isDefault;

        /// <summary>
        ///     获取一个值，该值表示了当前动作是否为默认动作
        /// </summary>
        public bool IsDefault
        {
            get { return _isDefault; }
        }

        protected String  _name;

        /// <summary>
        ///     获取或设置名称
        /// </summary>
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        protected Object _tag;

        /// <summary>
        ///     获取或设置附属属性
        /// </summary>
        public Object Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
            }
        }

        /// <summary>
        ///     添加一个场景
        /// </summary>
        /// <param name="scenario"></param>
        public void AddScenario(TScenario scenario)
        {
            if (scenario == null) throw new ArgumentNullException("scenario");
            _scenarios.TryAdd(scenario.Name, scenario);
        }

        /// <summary>
        ///     获取指定场景
        /// </summary>
        public TScenario GetScenario(String name)
        {
            if (name == null) throw new ArgumentNullException("name");
            TScenario scenario;
            return _scenarios.TryGetValue(name, out scenario) ? scenario : null;
        }

        /// <summary>
        ///     播放具有指定名称的场景
        /// </summary>
        /// <param name="scenarioName">场景名称</param>
        public void Play(string scenarioName)
        {
            if (String.IsNullOrEmpty(scenarioName)) throw new ArgumentNullException("scenarioName");
            if (!_scenarios.ContainsKey("scenarioName")) throw new System.Exception("无法播放一个场景，因为该场景不存在 !");
            _playScenario = _scenarios[scenarioName];
        }

        /// <summary>
        ///     开始动作事件
        /// </summary>
        public event EventHandler BeginAction;
        protected void BeginActionHandler(System.EventArgs e)
        {
            EventHandler action = BeginAction;
            if (action != null) action(this, e);
        }

        /// <summary>
        ///     结束动作事件
        /// </summary>
        public event EventHandler EndAction;
        protected void EndActionHandler(System.EventArgs e)
        {
            EventHandler action = EndAction;
            if (action != null) action(this, e);
        }

        #endregion

        #region IInstallable Members

        /// <summary>
        ///     安装
        /// </summary>
        /// <returns>返回安装的状态</returns>
        public abstract bool Install();
        /// <summary>
        ///     卸载
        /// </summary>
        /// <returns>返回卸载的状态</returns>
        public abstract bool UnInstall();

        #endregion

        #region 父类方法

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn. Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Draw.</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (_playScenario != null) _playScenario.Draw(gameTime);
        }

        /// <summary>
        /// Called when the GameComponent needs to be updated. Override this method with component-specific update code.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to Update</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (_playScenario != null) _playScenario.Update(gameTime);
        }

        #endregion
    }
}