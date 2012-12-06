using System;
using System.Collections.Generic;
using KJFramework.Mono.Game.Components.Controls;
using XnaTouch.Framework;

namespace KJFramework.Mono.Game.Components.Scenarios
{
    /// <summary>
    ///     场景抽象父类，提供了相关的基本操作。
    /// </summary>
    public abstract class Scenario : DrawableGameComponent, IScenario
    {
        #region Constructor

        /// <summary>
        ///     场景抽象父类，提供了相关的基本操作。
        /// </summary>
        protected Scenario(XnaTouch.Framework.Game game)
            : base(game)
        {
            
        }

        #endregion

        #region IScenario Member

        protected int _id;

        /// <summary>
        ///     获取场景唯一编号
        /// </summary>
        public int Id
        {
            get { return _id; }
        }

        /// <summary>
        ///     添加一个游戏组件
        /// </summary>
        /// <param name="component">游戏组件</param>
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
        ///     获取一个值，该值标示了当前长久是否为默认场景。
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
        ///     绘制
        /// </summary>
        /// <param name="gameTime">游戏时间</param>
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
        ///     更新
        /// </summary>
        /// <param name="gameTime">游戏时间</param>
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