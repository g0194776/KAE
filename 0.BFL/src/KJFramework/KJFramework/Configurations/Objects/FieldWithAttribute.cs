using System;
using System.Reflection;
namespace KJFramework.Configurations.Objects
{
    /// <summary>
    ///     ��ȡ�ֶεı������ʱ����ʱ���ݽṹ
    /// </summary>
    public class FieldWithAttribute<T> : IDisposable
        where T : System.Attribute
    {
        #region ��������

        ~FieldWithAttribute()
        {
            Dispose();
        }

        #endregion

        #region IDisposable ��Ա

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region ��Ա

        /// <summary>
        ///     ��ȡ�������ֶ���Ϣ
        /// </summary>
        public FieldInfo FieldInfo { get; set; }
        /// <summary>
        ///     ��ȡ�������ֶα������
        /// </summary>
        public T Attribute { get; set; }

        #endregion
    }
}