using System;
using KJFramework.Attribute;
using KJFramework.Configurations.Loaders;
using KJFramework.Helpers;

namespace KJFramework.Configurations
{
    /// <summary>
    ///     ���ýڹ��������ṩ����صĻ�������
    /// </summary>
    public class Configurations
    {
        #region ����

        /// <summary>
        ///     ��ȡ�Զ������ý�
        ///            * ��ǰĬ�ϼ��ط�ʽΪ�����ؼ���
        /// </summary>
        /// <typeparam name="T">�Զ������ý�����</typeparam>
        /// <param name="action">��ֵ�Զ������ýڵĶ���</param>
        public static void GetConfiguration<T>(Action<T> action) where T : class, new()
        {
            using (LocalConfigurationLoader loader = new LocalConfigurationLoader())
            {
                loader.Load(action);
            }
        }

        #endregion
    }
}