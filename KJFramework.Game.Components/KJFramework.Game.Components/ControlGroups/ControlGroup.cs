using System;
using System.Collections.Concurrent;
using KJFramework.Game.Components.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KJFramework.Game.Components.ControlGroups
{
    /// <summary>
    ///     �ؼ�������࣬�ṩ����صĻ���������
    /// </summary>
    /// <typeparam name="TControl">�ؼ�����</typeparam>
    public class ControlGroup<TControl> : Control, IControlGroup<TControl>
        where TControl : Control
    {
        #region ��Ա

        /// <summary>
        ///     �ؼ�����
        /// </summary>
        protected ConcurrentDictionary<String, TControl> _controls = new ConcurrentDictionary<String, TControl>();

        #endregion

        #region ���캯��

        /// <summary>
        ///     �ؼ�������࣬�ṩ����صĻ���������
        /// </summary>
        /// <param name="game">��Ϸ����</param>
        protected ControlGroup(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
        }

        #endregion

        #region IControlGroup ��Ա

        private Texture2D _background;
        /// <summary>
        ///     ��ȡ�����ÿؼ��鱳��ͼ
        /// </summary>
        public Texture2D Background
        {
            get { return _background; }
            set { _background = value; }
        }

        /// <summary>
        ///     ��ȡ�ؼ����о���ָ�����ƵĿؼ�
        /// </summary>
        /// <typeparam name="T">�ؼ�����</typeparam>
        /// <param name="name">�ؼ�����</param>
        /// <returns>���ؾ���ָ�����ƵĿؼ�</returns>
        public TControl GetControl(string name)
        {
            if (String.IsNullOrEmpty(name) || !_controls.ContainsKey(name))
            {
                return default(TControl);
            }
            return _controls[name];
        }

        /// <summary>
        ///     ��һ���ؼ�����ؼ���
        /// </summary>
        /// <param name="control">�ؼ�</param>
        /// <param name="position">
        ///     ������ؼ�����ʾƫ��
        ///     <para>* ��ƫ����������ڿؼ���ƫ�Ƶġ�</para>
        /// </param>
        public void AddControl(TControl control, Vector2 position)
        {
            if (control == null)
            {
                throw new System.Exception("Ҫ��ӵĿؼ�Ϊ�� !");
            }
            if (_controls.ContainsKey(control.Name))
            {
                throw new System.Exception("�޷���ӿؼ�����ǰ�������Ѿ�ӵ�иÿؼ������ơ�");
            }
            control.Position = new Vector2(_position.X + control.Position.X, _position.Y + control.Position.Y);
            _controls.TryAdd(control.Name, control);
        }

        #endregion

        #region ���෽��

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn.  Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime).</param>
        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            //���Ʊ���
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