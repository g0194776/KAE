using System;
namespace KJFramework.EventArgs
{
    public delegate void DelegateCpuPerformanceValueChanged(Object sender, CpuPerformanceValueChangedEventArgs e);
    /// <summary>
    ///     CPU����ֵ�����¼�
    /// </summary>
    public class CpuPerformanceValueChangedEventArgs : System.EventArgs
    {
        #region ��Ա

        private double _cpuUsage;
        /// <summary>
        ///     ��ȡCPUʹ����
        /// </summary>
        public double CpuUsage
        {
            get { return _cpuUsage; }
        }

        #endregion

        #region ���캯��

        /// <summary>
        ///     CPU����ֵ�����¼�
        /// </summary>
        /// <param name="cpuUsage">CPU����ֵ</param>
        public CpuPerformanceValueChangedEventArgs(double cpuUsage)
        {
            _cpuUsage = cpuUsage;
        }

        #endregion

    }
}