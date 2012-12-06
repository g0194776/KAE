using KJFramework.Game.Components.Controls;

namespace KJFramework.Game.Components.Scenarios
{
    /// <summary>
    ///     场景抽象父类，提供了相关的基本操作。
    /// </summary>
    public abstract class Scenario : Control, IScenario
    {
        #region 构造函数

        /// <summary>
        ///     场景抽象父类，提供了相关的基本操作。
        /// </summary>
        /// <param name="game">游戏对象</param>
        protected Scenario(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            LoadContent();
        }
        #endregion

        #region IScenario 成员

        protected int _id;

        /// <summary>
        ///     获取场景唯一编号
        /// </summary>
        public int Id
        {
            get { return _id; }
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

        #region 方法

        /// <summary>
        ///     加载场景所需要的资料
        /// </summary>
        protected abstract override void LoadContent();

        #endregion
    }
}