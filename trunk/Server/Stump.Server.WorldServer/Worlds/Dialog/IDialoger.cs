using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Worlds.Dialog
{
    public interface IDialoger
    {
        IDialog Dialog { get; }
        Character Character { get; }
    }
}