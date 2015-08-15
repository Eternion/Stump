using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stump.Server.BaseServer.IPC
{
    public interface IIPCErrorMessage
    {
        Guid RequestGuid { get; set; }
        string Message { get; set; }
        string StackTrace { get; set; }
    }
}
