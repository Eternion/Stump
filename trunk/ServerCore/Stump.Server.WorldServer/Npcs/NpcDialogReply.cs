using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Actions;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Npcs
{
    public class NpcDialogReply
    {
        public uint Id
        {
            get;
            private set;
        }

        public ActionsEnum ActionType
        {
            get;
            private set;
        }

        public object[] ActionArgs
        {
            get;
            private set;
        }

        public void CallAction(NpcSpawn npc, Character dialoger)
        {
            ActionManager.ExecuteAction(ActionType, npc, dialoger, ActionArgs);
        }
    }
}