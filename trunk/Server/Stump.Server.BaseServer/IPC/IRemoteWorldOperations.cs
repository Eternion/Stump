using System.ServiceModel;

namespace Stump.Server.BaseServer.IPC
{
    [ServiceContract]
    public interface IRemoteWorldOperations
    {
        [OperationContract]
        bool DisconnectClient(uint account);
    }
}