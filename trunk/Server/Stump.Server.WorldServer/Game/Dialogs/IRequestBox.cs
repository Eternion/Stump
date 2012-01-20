using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Dialogs
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