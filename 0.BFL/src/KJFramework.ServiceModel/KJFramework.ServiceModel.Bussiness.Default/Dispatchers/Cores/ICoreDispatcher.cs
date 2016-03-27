using KJFramework.Net.Channels;
using KJFramework.ServiceModel.Bussiness.Default.Messages;
using KJFramework.ServiceModel.Bussiness.Default.Services;
using KJFramework.ServiceModel.Bussiness.Default.Transactions;
using KJFramework.Statistics;

namespace KJFramework.ServiceModel.Bussiness.Default.Dispatchers.Cores
{
    /// <summary>
    ///     ���ķַ������ṩ����صĻ���������
    /// </summary>
    internal interface ICoreDispatcher : IStatisticable<IStatistic>
    {
        /// <summary>
        ///     �ַ�����
        /// </summary>
        /// <param name="serviceHandle">������</param>
        /// <param name="message">������Ϣ</param>
        /// <param name="channel">�ײ㴫��ͨ��</param>
        void Dispatch(IServiceHandle serviceHandle, RPCTransaction rpcTrans);
    }
}