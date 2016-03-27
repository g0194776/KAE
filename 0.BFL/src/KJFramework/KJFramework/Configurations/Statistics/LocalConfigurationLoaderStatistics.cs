using System;
using KJFramework.Configurations.Loaders;
using KJFramework.Statistics;
using System.Diagnostics;

namespace KJFramework.Configurations.Statistics
{
    /// <summary>
    ///     ���������ļ�������ͳ����
    /// </summary>
    public class LocalConfigurationLoaderStatistics : Statistic
    {
        #region ��Ա

        private LocalConfigurationLoader _loader;

        #endregion

        #region Overrides of Statistic

        /// <summary>
        ///     ��ʼ��
        /// </summary>
        /// <param name="element">ͳ������</param>
        /// <typeparam name="T">ͳ������</typeparam>
        public override void Initialize<T>(T element)
        {
            _loader = (LocalConfigurationLoader) ((Object)element);
            _loader.LoadSuccessfully += LoadSuccessfully;
            _loader.LoadFailed += LoadFailed;
        }

        /// <summary>
        ///     �ر�ͳ��
        /// </summary>
        public override void Close()
        {
            if (_loader != null)
            {
                _loader.LoadSuccessfully -= LoadSuccessfully;
                _loader.LoadFailed -= LoadFailed;
            }
        }

        #endregion

        #region �¼�

        void LoadFailed(object sender, System.EventArgs e)
        {
            Debug.WriteLine("���ؼ��������ļ�ʧ�ܡ�");
        }

        void LoadSuccessfully(object sender, System.EventArgs e)
        {
            Debug.WriteLine("���ؼ��������ļ��ɹ���");
        }


        #endregion
    }
}