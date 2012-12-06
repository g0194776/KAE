using KJFramework.Logger;
using KJFramework.Logger.LogObject;

namespace KJFramework
{
    /// <summary>
    ///     �ɼ�¼��Ԫ�ӿڣ��ṩ�����¼��صĻ���������
    /// </summary>
    /// <typeparam name="TLogger">��¼������</typeparam>
    /// <typeparam name="TLog">��¼������</typeparam>
    public interface IRecordable<TLogger, TLog>
        where TLog : ILog
        where TLogger : ILogger<TLog>
    {
        /// <summary>
        ///     ��ȡ�����ü�¼��
        /// </summary>
        TLogger Logger { get; set; }
    }
}