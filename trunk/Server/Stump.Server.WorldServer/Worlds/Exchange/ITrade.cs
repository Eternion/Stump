using Stump.Server.WorldServer.Worlds.Dialog;

namespace Stump.Server.WorldServer.Worlds.Exchange
{
    public interface ITrade : IDialog
    {
        int Id { get; }
    }
}