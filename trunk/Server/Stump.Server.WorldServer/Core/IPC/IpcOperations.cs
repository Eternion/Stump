using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Stump.Server.BaseServer.IPC;
using Stump.Server.WorldServer.Core.Network;

namespace Stump.Server.WorldServer.Core.IPC
{
    public class IpcOperations
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

        
    }
}