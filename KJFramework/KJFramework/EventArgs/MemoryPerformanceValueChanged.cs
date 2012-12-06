using System;
namespace KJFramework.EventArgs
{
    public delegate void DelegateMemoryPerformanceValueChanged(Object sender, MemoryPerformanceValueChangedEventArgs e);
    /// <summary>
    ///     �ڴ�����ֵ�����¼�
    /// </summary>
    public class MemoryPerformanceValueChangedEventArgs : System.EventArgs
    {
        #region ��Ա

        private double _memoryUsage;
        /// <summary>
        ///     ��ȡCPUʹ����
        /// </summary>
        public double MemoryUsage
        {
            get { return _memoryUsage; }
        }

        #endregion

        #region ���캯��

        /// <summary>
        ///     �ڴ�����ֵ�����¼�
        /// </summary>
        /// <param name="memoryUsage">�ڴ�����ֵ</param>
        public MemoryPerformanceValueChangedEventArgs(double memoryUsage)
        {
            _memoryUsage = memoryUsage;
        }

        #endregion

    }
}