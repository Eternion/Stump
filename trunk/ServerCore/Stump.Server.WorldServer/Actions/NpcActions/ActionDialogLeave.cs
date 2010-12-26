using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Actions.NpcActions
{
    public class ActionDialogLeave : NpcAction
    {
        public ActionDialogLeave()
        {
        }

        public override void Execute(NpcSpawn npc, Character executer)
        {
            if (executer.Dialog != null)
                executer.Dialog.EndDialog();
        }
    }
}