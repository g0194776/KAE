﻿using System;
using System.Collections.Generic;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Loggers;
using KJFramework.Messages.Contracts;
using KJFramework.Net.Identities;
using KJFramework.Results;
using KJFramework.Tracing;

namespace KJFramework.ApplicationEngine.Commands
{
    /// <summary>
    ///     KAE宿主所支持的系统命令执行器
    /// </summary>
    internal static class KAECommandsExector
    {
        #region Members.

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (KAECommandsExector));
        private static readonly Dictionary<MessageIdentity, IKAESystemCommand> _commands = new Dictionary<MessageIdentity, IKAESystemCommand>(); 

        #endregion

        #region Methods.

        /// <summary>
        ///    初始化
        /// </summary>
        public static void Initialize()
        {
        }

        /// <summary>
        ///    执行相应的KAE系统命令
        /// </summary>
        /// <param name="msg">系统请求消息</param>
        /// <param name="logger">状态记录器实例</param>
        /// <returns>返回执行后的结果</returns>
        public static IExecuteResult Execute(MetadataContainer msg, IKAEStateLogger logger)
        {
            MessageIdentity messageIdentity = msg.GetAttributeAsType<MessageIdentity>(0x00);
            IKAESystemCommand command;
            if (!_commands.TryGetValue(messageIdentity, out command))
                return ExecuteResult.Fail((byte)KAEErrorCodes.NotSupportedCommand, string.Format("#Not supported system command protocol: {0}", messageIdentity));
            try { return command.Execute(msg, logger); }
            catch (Exception ex)
            {
                logger.Log(string.Format("#Occured an unhandled exception while executing a system level KAE command. #Command msg struct: {0}. \r\n#Error: {1}", msg, ex.Message));
                _tracing.Error(ex, null);
                return ExecuteResult.Fail((byte) KAEErrorCodes.Unknown, ex.Message);
            }
        }

        private static void RegisterCommand(IKAESystemCommand command)
        {
            _commands[command.SupportedCommand] = command;
        }

        #endregion
    }
}