using System;
using System.Collections.Generic;
using KJFramework.Game.Components.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace KJFramework.Game.Components.Events
{
    /// <summary>
    ///     事件链，提供了相关的基本操作
    /// </summary>
    class EventChain : IEventChain
    {
        #region Constructor

        /// <summary>
        ///     事件链，提供了相关的基本操作
        /// </summary>
        /// <param name="control">内部控件</param>
        public EventChain(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            _control = control;
        }

        #endregion

        #region Members

        private readonly IList<IEventHook> _hooks = new List<IEventHook>();

        #endregion

        #region Implementation of IEventChain

        private readonly Control _control;

        /// <summary>
        ///     获取内部的控件
        /// </summary>
        public Control Control
        {
            get { return _control; }
        }

        /// <summary>
        ///     注册一个事件钩子
        /// </summary>
        /// <param name="eventHook">事件钩子</param>
        /// <returns>返回自身</returns>
        public IEventChain Regist(IEventHook eventHook)
        {
            if (eventHook == null) throw new ArgumentNullException("eventHook");
            eventHook.Control = _control;
            _hooks.Add(eventHook);
            return this;
        }

        /// <summary>
        ///     执行
        /// </summary>
        /// <param name="gameTime">游戏时间</param>
        public void Execute(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            GameTime gt = gameTime;
            foreach (IEventHook eventHook in _hooks)
            {
                if (!eventHook.Execute(keyboardState, mouseState, gt)) break;
            }
        }

        #endregion
    }
}