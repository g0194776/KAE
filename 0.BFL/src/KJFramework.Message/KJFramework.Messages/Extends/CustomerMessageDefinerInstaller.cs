using System;
using System.Collections.Generic;
using System.Linq;

namespace KJFramework.Messages.Extends
{
    /// <summary>
    ///     ��������Ϣ���尲װ�����ṩ����صĻ���������
    /// </summary>
    public static class CustomerMessageDefinerInstaller
    {
        #region ��Ա

        private static Dictionary<Guid, ICustomerMessageDefiner> _definer = new Dictionary<Guid, ICustomerMessageDefiner>();

        #endregion

        #region ����

        /// <summary>
        ///     ��װһ����������Ϣ������
        /// </summary>
        /// <param name="definer">��������Ϣ������</param>
        public static void Install(ICustomerMessageDefiner definer)
        {
            if (definer == null)
            {
                return;
            }
            //�Լ��
            definer.Check();
            lock (typeof(CustomerMessageDefinerInstaller))
            {
                if (!_definer.ContainsKey(definer.Id))
                {
                    //������������Ƶ�����õ��ַ�����
                    String.Intern(definer.Key);
                    _definer.Add(definer.Id, definer);
                }
            }
        }

        /// <summary>
        ///     ж��һ������ָ����ŵĵ�������Ϣ������
        /// </summary>
        /// <param name="id">���</param>
        public static void UnInstall(Guid id)
        {
            lock (typeof(CustomerMessageDefinerInstaller))
            {
                if (_definer.ContainsKey(id))
                {
                    _definer.Remove(id);
                }
            }
        }

        /// <summary>
        ///     ͨ��ָ�����û���ֵ����ȡһ����������Ϣ������
        /// </summary>
        /// <param name="key">�û���ֵ</param>
        /// <returns>����ָ���ĵ�������Ϣ������</returns>
        public static ICustomerMessageDefiner GetDefiner(String key)
        {
            if (String.IsNullOrEmpty(key))
            {
                return null;
            }
            lock (typeof(CustomerMessageDefinerInstaller))
            {
                var result = _definer.Values.Where(definer => definer.Key == key);
                if (result != null && result.Count() > 0)
                {
                    return result.First();
                }
                return null;
            }
        }

        #endregion
    }
}