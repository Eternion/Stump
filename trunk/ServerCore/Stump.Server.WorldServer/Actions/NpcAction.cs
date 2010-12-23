using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Actions
{
    public abstract class NpcAction : CharacterAction
    {
        public NpcSpawn Npc
        {
            get;
            set;
        }

        protected NpcAction(NpcActionArgument argument)
            : base(argument)
        {
            Npc = argument.Npc;
        }
    }
}