using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Stump.Server.BaseServer.IPC
{
    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    public class WorldClientAdapter : ClientBase<IRemoteWorldOperations>, IRemoteWorldOperations
    {
        public WorldClientAdapter()
        {
        }

        public WorldClientAdapter(string endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }

        public WorldClientAdapter(string endpointConfigurationName, string remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        public WorldClientAdapter(string endpointConfigurationName, EndpointAddress remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        public WorldClientAdapter(Binding binding, EndpointAddress remoteAddress)
            : base(binding, remoteAddress)
        {
        }

        public event Action<Exception> Error;

        private void OnError(Exception ex)
        {
            Action<Exception> handler = Error;
            if (handler != null)
                handler(ex);
        }

        public bool DisconnectClient(uint accountId)
        {
            try
            {
                return Channel.DisconnectClient(accountId);
            }
            catch (Exception ex)
            {
                OnError(ex);
                return false;
            }
        }
    }
}