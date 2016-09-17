using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;

namespace Stump.Server.WorldServer.Database.Npcs.Actions
{
    public abstract class NpcAction
    {

        public abstract NpcActionTypeEnum[] ActionType
        {
            get;
        }

        public virtual int Priority => 0;

        public abstract void Execute(Npc npc, Character character);

        public virtual bool CanExecute(Npc npc, Character character)
        {
            return true;
        }
    }
}