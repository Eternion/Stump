using System;
using System.Collections.Generic;

namespace Stump.Server.BaseServer.Network
{
    public static class ClientExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> clients, Action<T> action) where T : BaseClient
        {
            foreach (var client in clients)
            {
                action(client);
            }
        }
    }
}