using System;
using System.Collections.Concurrent;
using KJFramework.Game.Components.Scenarios;
using Microsoft.Xna.Framework;

namespace KJFramework.Game.Components.Actions
{
    /// <summary>
    ///     ���������࣬�ṩ����صĻ���������
    /// </summary>
    public abstract class Action<TScenario> : DrawableGameComponent ,IAction<TScenario>
        where TScenario : Scenario
    {
        #region ���캯��

        /// <summary>
        ///     ���������࣬�ṩ����صĻ���������
        /// </summary>
        protected Action(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
        }

        #endregion

        #region ��Ա

        protected readonly ConcurrentDictionary<String, TScenario> _scenarios = new ConcurrentDictionary<string, TScenario>();
        private TScenario _playScenario;

        #endregion

        #region IAction Members

        protected bool _isDefault;

        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�����Ƿ�ΪĬ�϶���
        /// </summary>
        public bool IsDefault
        {
            get { return _isDefault; }
        }

        protected String  _name;

        /// <summary>
        ///     ��ȡ����������
        /// </summary>
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        protected Object _tag;

        /// <summary>
        ///     ��ȡ�����ø�������
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
        ///     ���һ������
        /// </summary>
        /// <param name="scenario"></param>
        public void AddScenario(TScenario scenario)
        {
            if (scenario == null) throw new ArgumentNullException("scenario");
            _scenarios.TryAdd(scenario.Name, scenario);
        }

        /// <summary>
        ///     ��ȡָ������
        /// </summary>
        public TScenario GetScenario(String name)
        {
            if (name == null) throw new ArgumentNullException("name");
            TScenario scenario;
            return _scenarios.TryGetValue(name, out scenario) ? scenario : null;
        }

        /// <summary>
        ///     ���ž���ָ�����Ƶĳ���
        /// </summary>
        /// <param name="scenarioName">��������</param>
        public void Play(string scenarioName)
        {
            if (String.IsNullOrEmpty(scenarioName)) throw new ArgumentNullException("scenarioName");
            if (!_scenarios.ContainsKey("scenarioName")) throw new System.Exception("�޷�����һ����������Ϊ�ó��������� !");
            _playScenario = _scenarios[scenarioName];
        }

        /// <summary>
        ///     ��ʼ�����¼�
        /// </summary>
        public event EventHandler BeginAction;
        protected void BeginActionHandler(System.EventArgs e)
        {
            EventHandler action = BeginAction;
            if (action != null) action(this, e);
        }

        /// <summary>
        ///     ���������¼�
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
        ///     ��װ
        /// </summary>
        /// <returns>���ذ�װ��״̬</returns>
        public abstract bool Install();
        /// <summary>
        ///     ж��
        /// </summary>
        /// <returns>����ж�ص�״̬</returns>
        public abstract bool UnInstall();

        #endregion

        #region ���෽��

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