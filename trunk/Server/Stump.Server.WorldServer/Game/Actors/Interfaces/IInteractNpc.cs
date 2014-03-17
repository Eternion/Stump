using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Actors.Interfaces
{
    public interface IInteractNpc
    {
        void InteractWith(NpcActionTypeEnum actionType, Character dialoguer);
        bool CanInteractWith(NpcActionTypeEnum action, Character dialoguer);
    }
}