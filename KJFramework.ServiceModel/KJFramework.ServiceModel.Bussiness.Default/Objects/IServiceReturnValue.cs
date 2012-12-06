using KJFramework.ServiceModel.Enums;

namespace KJFramework.ServiceModel.Bussiness.Default.Objects
{
    /// <summary>
    ///     ���񷽷�����ֵԪ�ӿڣ��ṩ����صĻ������Խṹ
    /// </summary>
    internal interface IServiceReturnValue
    {
        /// <summary>
        ///     ��ȡ�����ô�����
        /// </summary>
        ServiceProcessResult ProcessResult { get; set; }
        /// <summary>
        ///     ��ȡ�������Ƿ���з���ֵ
        /// </summary>
        bool HasReturnValue { get; set; }
        /// <summary>
        ///     ����һ���ɷ��������ص��쳣����
        /// </summary>
        /// <returns>����һ���쳣</returns>
        System.Exception CreateException();
        /// <summary>
        ///     ���÷���ֵ����
        /// </summary>
        /// <typeparam name="T">����ֵ����</typeparam>
        /// <param name="obj">����ֵ����</param>
        void SetReturnValue<T>(T obj);
        /// <summary>
        ///     ��ȡ����ֵ
        /// </summary>
        /// <typeparam name="T">����ֵ����</typeparam>
        T GetReturnValue<T>();
    }
}