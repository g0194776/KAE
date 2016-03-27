using System;
using KJFramework.Dynamic.Components;
using KJFramework.Statistics;

namespace KJFramework.Dynamic.Statistics
{
    /// <summary>
    ///     ��̬���������ͳ�������ṩ����صĻ���������
    /// </summary>
    public class DynamicDomainObjectStatistic : Statistic
    {
        #region ��Ա

        private DynamicDomainObject _target;

        #endregion

        #region Overrides of Statistic

        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="element">ͳ������</param>
        /// <typeparam name="T">ͳ������</typeparam>
        public override void Initialize<T>(T element)
        {
            _target = (DynamicDomainObject) ((Object) element);
            _target.WorkProcessing += WorkProcessing;
        }

        /// <summary>
        /// �ر�ͳ��
        /// </summary>
        public override void Close()
        {
            if (_target != null)
            {
                _target.WorkProcessing -= WorkProcessing;
                _target = null;
            }
        }

        #endregion

        #region �¼�

        void WorkProcessing(object sender, EventArgs.LightSingleArgEventArgs<string> e)
        {
            Console.WriteLine(e.Target);
        }

        #endregion
    }
}