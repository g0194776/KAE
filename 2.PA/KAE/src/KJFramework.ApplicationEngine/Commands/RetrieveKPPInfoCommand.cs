using System;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Loggers;
using KJFramework.Messages.Contracts;
using KJFramework.Net.Identities;
using KJFramework.Results;

namespace KJFramework.ApplicationEngine.Commands
{
    /// <summary>
    ///    获取已上架的KPP实例相关信息的命令
    /// </summary>
    internal class RetrieveKPPInfoCommand : IKAESystemCommand
    {
        #region Members.

        /// <summary>
        ///     获取当前KAE系统命令所分配的网络信令P/S/D
        /// </summary>
        public MessageIdentity SupportedCommand
        {
            get
            {
                return new MessageIdentity {ProtocolId = 0xFF, ServiceId = 0x01, DetailsId = 0x00};
            }
        }

        #endregion

        #region Methods.

        /// <summary>
        ///     执行一个KAE系统命令
        /// </summary>
        /// <param name="msg">执行命令的请求消息</param>
        /// <param name="stateLogger">KAE宿主状态记录器实例</param>
        /// <returns>返回操作的结果</returns>
        public IExecuteResult Execute(MetadataContainer msg, IKAEStateLogger stateLogger)
        {
            Guid kppUniqueId = msg.GetAttributeAsType<Guid>(0x03);
            if (kppUniqueId == Guid.Empty) return ExecuteResult.Fail((byte)KAEErrorCodes.IllegalArgument, string.Empty);
            //stateLogger.Log(string.Format("#[Retrieving KPP Info] #KAE application: {0}, Internal un-rsp count: {1}", kppUniqueId, APPExecuter.Application.UnRspCount));
            //return ExecuteResult.Succeed(APPExecuter.Application.UnRspCount.ToString());
            throw new NotImplementedException();
        }

        #endregion
    }
}