

using Stump.Server.BaseServer.IPC.Objects;

namespace Stump.Server.BaseServer.IPC
{
    public interface IRemoteOperationsWorld
    {
        /// <summary>
        ///   Disconnect client who use the given account
        /// </summary>
        bool DisconnectConnectedAccount(uint accountId);
    }
}