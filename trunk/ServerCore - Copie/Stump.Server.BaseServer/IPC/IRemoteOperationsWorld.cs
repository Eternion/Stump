

namespace Stump.Server.BaseServer.IPC
{
    public interface IRemoteOperationsWorld
    {
        /// <summary>
        ///   Disconnect client who use the given account
        /// </summary>
        /// <param name = "account"></param>
        bool DisconnectConnectedAccount(AccountData account);
    }
}