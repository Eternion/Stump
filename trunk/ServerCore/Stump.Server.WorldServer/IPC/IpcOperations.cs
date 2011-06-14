
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Database;
using Stump.Database.AuthServer;
using Stump.Server.BaseServer.IPC;

namespace Stump.Server.WorldServer.IPC
{
    public class IpcOperations : MarshalByRefObject, IRemoteOperationsWorld
    {
        #region IRemoteOperationsWorld Members

        public bool DisconnectConnectedAccount(AccountRecord account)
        {
            IEnumerable<WorldClient> clients = WorldServer.Instance.GetClientsUsingAccount(account);

            foreach (WorldClient client in clients)
            {
                client.Disconnect();
            }

            return clients.Count() > 0;
        }

        #endregion
    }
}