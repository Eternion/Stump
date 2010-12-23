using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Actions
{
    public class NpcActionArgument : CharacterActionArgument
    {
        public NpcSpawn Npc
        {
            get;
            set;
        }

        public NpcActionArgument(NpcSpawn npc, Character character, params object[] arguments)
            : base(character, arguments)
        {
            Npc = npc;
        }
    }
}