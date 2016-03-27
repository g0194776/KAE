using System;

namespace KJFramework.Configurations
{
    /// <summary>
    ///     �������Զ������ýڸ��࣬�ṩ����صĻ���������
    /// </summary>
    /// <typeparam name="T">���ý�����</typeparam>
    public class CustomerSection<T> : ICustomerSection
        where T : class, new()
    {
        #region Implementation of IDisposable

        private static T _current;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of ICustomerSection<T>

        /// <summary>
        ///     ��ȡ��ǰ���ý�
        /// </summary>
        public static T Current
        {
            get
            {
                if (_current == null)
                {
                    try
                    {
                        Configurations.GetConfiguration(delegate(T section)
                        {
                            _current = section;
                        });
                    }
                    catch
                    {
                        return null;
                    }
                }
                return _current;
            }
            //support LoadRunner.
            set { _current = value; }
        }

        #endregion
    }
}