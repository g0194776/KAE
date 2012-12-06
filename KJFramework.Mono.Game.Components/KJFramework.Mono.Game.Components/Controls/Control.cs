using System;
using KJFramework.Mono.Game.Components.Scenarios;
using XnaTouch.Framework;

namespace KJFramework.Mono.Game.Components.Controls
{
    /// <summary>
    ///     ��Ϸ����Ԫ�������п����齨�ĳ����࣬�ṩ����صĻ���������
    /// </summary>
    public class Control : DrawableGameComponent
    {
        #region Constructor

        /// <summary>
        ///     ��Ϸ����Ԫ�������п����齨�ĳ����࣬�ṩ����صĻ���������
        /// </summary>
        /// <param name="game">��Ϸ����</param>
        public Control(XnaTouch.Framework.Game game)
            : base(game)
        {
        }

        #endregion

        #region Base Methods

        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="gameTime">��Ϸʱ��</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        ///     ��ʼ��
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            InitializingHandler(null);
        }

        /// <summary>
        ///     ����
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            LoadingHandler(null);
        }

        /// <summary>
        ///     ж��
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();
            UnLoadingHandler(null);
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ����������Ĺؼ���
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        ///     ��ȡ�����������ĳ���
        /// </summary>
        public IScenario OwnScenario { get; set; } 

        #endregion

        #region Events

        /// <summary>
        ///     ��ʼ��
        /// </summary>
        public event EventHandler Initializing;
        protected void InitializingHandler(EventArgs e)
        {
            EventHandler handler = Initializing;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     ����
        /// </summary>
        public event EventHandler Loading;
        protected void LoadingHandler(EventArgs e)
        {
            EventHandler handler = Loading;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     ж��
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