using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Stump.Server.BaseServer.IPC;
using Stump.Server.WorldServer.Core.Network;

namespace Stump.Server.WorldServer.Core.IPC
{
   [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, IncludeExceptionDetailInFaults = true)]
    public class IpcOperations : IRemoteWorldOperations
    {
       public IpcOperations()
       {
           IContextChannel channel = OperationContext.Current.Channel;

           channel.Closed += OnDisconnected;
           channel.Faulted += OnDisconnected;
       }

       private void OnDisconnected(object sender, EventArgs args)
       {
           
       }

        public bool DisconnectClient(uint accountId)
        {
            IEnumerable<WorldClient> clients = WorldServer.Instance.FindClients(client => client.Account != null && client.Account.Id == accountId);

            foreach (WorldClient client in clients)
            {
                client.Disconnect();
            }

            return clients.Any();
        }
    }
}