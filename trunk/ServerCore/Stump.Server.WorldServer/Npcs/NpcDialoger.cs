using Stump.Server.WorldServer.Dialog;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Npcs
{
    public class NpcDialoger : Dialoger
    {
        public NpcDialoger(Character character, NpcDialog dialog)
            : base(character, dialog)
        {
        }

        public new NpcDialog Dialog
        {
            get { return base.Dialog as NpcDialog; }
        }
    }
}