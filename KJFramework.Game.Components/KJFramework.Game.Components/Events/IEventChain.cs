using KJFramework.Game.Components.Controls;
using Microsoft.Xna.Framework;

namespace KJFramework.Game.Components.Events
{
    /// <summary>
    ///     事件链元接口，提供了相关的基本操作
    /// </summary>
    public interface IEventChain
    {
        /// <summary>
        ///     获取内部的控件
        /// </summary>
        Control Control { get; }
        /// <summary>
        ///     注册一个事件钩子
        /// </summary>
        /// <param name="eventHook">事件钩子</param>
        /// <returns>返回自身</returns>
        IEventChain Regist(IEventHook eventHook);
        /// <summary>
        ///     执行
        /// </summary>
        /// <param name="gameTime">游戏时间</param>
        void Execute(GameTime gameTime);
    }
}