
using System;
using Stump.Server.AuthServer.Network;

namespace Stump.Server.AuthServer.Handlers
{
    public static class PredicatesDefinitions
    {
        public static readonly Predicate<AuthClient> HasChoosenAccount = entry => entry.Account != null;
        public static readonly Predicate<AuthClient> IsLookingOfServers = entry => entry.LookingOfServers;
    }
}