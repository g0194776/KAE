using System;

namespace KJFramework.Configurations
{
    /// <summary>
    ///     第三方自定义配置节父类，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="T">配置节类型</typeparam>
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
        ///     获取当前配置节
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