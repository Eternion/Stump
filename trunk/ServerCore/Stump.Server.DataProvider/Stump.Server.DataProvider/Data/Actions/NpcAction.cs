using Stump.Server.DataProvider.Data.Actions;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Actions
{
    public abstract class NpcAction : ActionBase
    {
        public abstract void Execute(NpcSpawn npc, Character executer);
    }
}