using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Worlds.Dialogs
{
    public interface IRequestBox
    {
        Character Source { get; }
        Character Target { get; }

        void Open();
        void Accept();
        void Deny();
        void Cancel();
    }
}