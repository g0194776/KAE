using System;
using KJFramework.Configurations.Loaders;

namespace KJFramework.Configurations
{
    /// <summary>
    ///     ���ýڹ��������ṩ����صĻ�������
    /// </summary>
    public static class Configurations
    {
        #region Members

        /// <summary>
        ///     Զ�����ü�����
        ///     <para>* ������ֶ�ֵΪ�գ����ʹ�ñ��������ļ�������</para>
        /// </summary>
        public static IConfigurationLoader RemoteConfigLoader { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     ��ȡ�Զ������ý�
        ///            * ��ǰĬ�ϼ��ط�ʽΪ�����ؼ���
        /// </summary>
        /// <typeparam name="T">�Զ������ý�����</typeparam>
        /// <param name="action">��ֵ�Զ������ýڵĶ���</param>
        public static void GetConfiguration<T>(Action<T> action) where T : class, new()
        {
            if (RemoteConfigLoader == null)
                using (LocalConfigurationLoader loader = new LocalConfigurationLoader()) loader.Load(action);
            else RemoteConfigLoader.Load(action);
        }

        #endregion
    }
}