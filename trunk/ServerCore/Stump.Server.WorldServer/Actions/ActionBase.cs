using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Actions
{
    public abstract class ActionBase
    {
        public static void ExecuteAction(ActionBase action, NpcSpawn npc, Character executer)
        {
            if (action is CharacterAction)
                ( action as CharacterAction ).Execute(executer);
            else if (action is NpcAction)
                (action as NpcAction).Execute(npc, executer);
        }
    }
}