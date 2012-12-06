using System;
using System.Collections.Concurrent;
using KJFramework.Game.Components.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KJFramework.Game.Components.ControlGroups
{
    /// <summary>
    ///     控件组抽象父类，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="TControl">控件类型</typeparam>
    public class ControlGroup<TControl> : Control, IControlGroup<TControl>
        where TControl : Control
    {
        #region 成员

        /// <summary>
        ///     控件集合
        /// </summary>
        protected ConcurrentDictionary<String, TControl> _controls = new ConcurrentDictionary<String, TControl>();

        #endregion

        #region 构造函数

        /// <summary>
        ///     控件组抽象父类，提供了相关的基本操作。
        /// </summary>
        /// <param name="game">游戏对象</param>
        protected ControlGroup(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
        }

        #endregion

        #region IControlGroup 成员

        private Texture2D _background;
        /// <summary>
        ///     获取或设置控件组背景图
        /// </summary>
        public Texture2D Background
        {
            get { return _background; }
            set { _background = value; }
        }

        /// <summary>
        ///     获取控件组中具有指定名称的控件
        /// </summary>
        /// <typeparam name="T">控件类型</typeparam>
        /// <param name="name">控件名称</param>
        /// <returns>返回具有指定名称的控件</returns>
        public TControl GetControl(string name)
        {
            if (String.IsNullOrEmpty(name) || !_controls.ContainsKey(name))
            {
                return default(TControl);
            }
            return _controls[name];
        }

        /// <summary>
        ///     将一个控件加入控件组
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="position">
        ///     被加入控件的显示偏移
        ///     <para>* 此偏移量是相对于控件组偏移的。</para>
        /// </param>
        public void AddControl(TControl control, Vector2 position)
        {
            if (control == null)
            {
                throw new System.Exception("要添加的控件为空 !");
            }
            if (_controls.ContainsKey(control.Name))
            {
                throw new System.Exception("无法添加控件，当前集合中已经拥有该控件的名称。");
            }
            control.Position = new Vector2(_position.X + control.Position.X, _position.Y + control.Position.Y);
            _controls.TryAdd(control.Name, control);
        }

        #endregion

        #region 父类方法

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn.  Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime).</param>
        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            //绘制背景
            if (_background != null)
            {
                _spriteBatch.Draw(_background, _position, Color.White);
            }
            foreach (TControl control in _controls.Values)
            {
                control.Draw(gameTime);
            }
            _spriteBatch.End();
        }

        /// <summary>
        /// Called when the GameComponent needs to be updated.  Override this method with component-specific update code.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to Microsoft.Xna.Framework.GameComponent.Update(Microsoft.Xna.Framework.GameTime)</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (TControl control in _controls.Values)
            {
                control.Update(gameTime);
            }
        }

        #endregion
    }
}