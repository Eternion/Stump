using Stump.Server.WorldServer.Worlds.Dialogs;

namespace Stump.Server.WorldServer.Worlds.Exchanges
{
    public interface ITrade : IDialog
    {
        int Id { get; }
    }
}