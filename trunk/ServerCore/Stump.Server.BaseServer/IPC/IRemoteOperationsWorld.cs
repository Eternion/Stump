
using Stump.Database;
using Stump.Database.AuthServer;

namespace Stump.Server.BaseServer.IPC
{
    public interface IRemoteOperationsWorld
    {
        /// <summary>
        ///   Disconnect client who use the given account
        /// </summary>
        /// <param name = "account"></param>
        bool DisconnectConnectedAccount(AccountRecord account);
    }
}