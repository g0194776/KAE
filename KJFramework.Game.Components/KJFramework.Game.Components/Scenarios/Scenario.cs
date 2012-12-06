using KJFramework.Game.Components.Controls;

namespace KJFramework.Game.Components.Scenarios
{
    /// <summary>
    ///     ���������࣬�ṩ����صĻ���������
    /// </summary>
    public abstract class Scenario : Control, IScenario
    {
        #region ���캯��

        /// <summary>
        ///     ���������࣬�ṩ����صĻ���������
        /// </summary>
        /// <param name="game">��Ϸ����</param>
        protected Scenario(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            LoadContent();
        }
        #endregion

        #region IScenario ��Ա

        protected int _id;

        /// <summary>
        ///     ��ȡ����Ψһ���
        /// </summary>
        public int Id
        {
            get { return _id; }
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

        #region ����

        /// <summary>
        ///     ���س�������Ҫ������
        /// </summary>
        protected abstract override void LoadContent();

        #endregion
    }
}