
using System.Net;
using System.Net.Sockets;

namespace Stump.Tools.Proxy.Network
{
    public class AuthClient : ProxyClient
    {
        public AuthClient(Socket socket, IPEndPoint ipEndPoint)
            : base(socket)
        {
            IsInCriticalZone = true;

            BindToServer(ipEndPoint);
        }
    }
}