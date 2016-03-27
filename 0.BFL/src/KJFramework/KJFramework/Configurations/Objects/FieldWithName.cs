using System;
using System.Reflection;
namespace KJFramework.Configurations.Objects
{
    /// <summary>
    ///     �ֶ�������������ʱ�ṹ��
    /// </summary>
    public class FieldWithName : IDisposable
    {
        #region ��������

        ~FieldWithName()
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
        ///     ��ȡ�������ֶ�����
        /// </summary>
        public FieldInfo FieldInfo { get; set; }
        /// <summary>
        ///     ��ȡ�������ֶ�����
        /// </summary>
        public String Name { get; set; }

        #endregion
    }
}