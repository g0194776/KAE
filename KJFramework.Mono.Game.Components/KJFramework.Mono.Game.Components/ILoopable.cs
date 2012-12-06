using XnaTouch.Framework;

namespace KJFramework.Mono.Game.Components
{
    /// <summary>
    ///     可循环的游戏元接口，提供了相关的基本操作。
    /// </summary>
    public interface ILoopable
    {
        /// <summary>
        ///     绘制
        /// </summary>
        /// <param name="gameTime">游戏时间</param>
        void Draw(GameTime gameTime);
        /// <summary>
        ///     更新
        /// </summary>
        /// <param name="gameTime">游戏时间</param>
        void Update(GameTime gameTime);
    }
}