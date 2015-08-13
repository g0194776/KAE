using System.Collections.Generic;
using KJFramework.Net.Channels.Identities;

namespace KJFramework.ApplicationEngine.Commands
{
    /// <summary>
    ///     KAE宿主所支持的系统命令执行器
    /// </summary>
    internal static class KAECommandsExector
    {
        #region Members.

        private static readonly Dictionary<MessageIdentity, IKAESystemCommand> _commands = new Dictionary<MessageIdentity, IKAESystemCommand>(); 

        #endregion

        #region Methods.

        /// <summary>
        ///    初始化
        /// </summary>
        public static void Initialize()
        {
            RegisterCommand(new InstallKPPCommand());
            RegisterCommand(new UninstallKPPCommand());
        }

        private static void RegisterCommand(IKAESystemCommand command)
        {
            _commands[command.SupportedCommand] = command;
        }

        #endregion
    }
}