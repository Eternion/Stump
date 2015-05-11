using System.Net.Sockets;
using Stump.Core.Pool;

namespace Stump.Server.BaseServer.Network
{
    public class PoolableSocketArgs : SocketAsyncEventArgs, IPooledObject
    {
        public void Cleanup()
        {
            UserToken = null;
        }
    }
}