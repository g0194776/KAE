using System;
using System.Collections.Generic;
using KJFramework.Mono.Game.Components.Controls;
using XnaTouch.Framework;

namespace KJFramework.Mono.Game.Components.Scenarios
{
    /// <summary>
    ///     ���������࣬�ṩ����صĻ���������
    /// </summary>
    public abstract class Scenario : DrawableGameComponent, IScenario
    {
        #region Constructor

        /// <summary>
        ///     ���������࣬�ṩ����صĻ���������
        /// </summary>
        protected Scenario(XnaTouch.Framework.Game game)
            : base(game)
        {
            
        }

        #endregion

        #region IScenario Member

        protected int _id;

        /// <summary>
        ///     ��ȡ����Ψһ���
        /// </summary>
        public int Id
        {
            get { return _id; }
        }

        /// <summary>
        ///     ���һ����Ϸ���
        /// </summary>
        /// <param name="component">��Ϸ���</param>
        public void Add(Control component)
        {
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }
            component.OwnScenario = this;
             _components.Add(component);
        }

        protected bool _isDefault;

        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�����Ƿ�ΪĬ�ϳ�����
        /// </summary>
        public bool IsDefault
        {
            get { return _isDefault; }
        }

        #endregion

        #region Members

        private List<Control> _components = new List<Control>();

        #endregion

        #region Methods

        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="gameTime">��Ϸʱ��</param>
        public override void Draw(GameTime gameTime)
        {
            if (Enabled)
            {
                base.Draw(gameTime);
                foreach (Control component in _components)
                {
                    component.Draw(gameTime);
                }
            }
        }

        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="gameTime">��Ϸʱ��</param>
        public override void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                base.Update(gameTime);
                foreach (Control component in _components)
                {
                    component.Update(gameTime);
                }
            }
        }

        #endregion
    }
}