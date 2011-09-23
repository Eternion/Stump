using System;
using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Worlds.Dialogs.Npcs;

namespace Stump.Server.WorldServer.Database.Npcs.Actions
{
    [ActiveRecord(DiscriminatorValue = "Talk")]
    public class NpcTalkAction : NpcAction
    {
        public override NpcActionTypeEnum ActionType
        {
            get
            {
                return NpcActionTypeEnum.ACTION_TALK;
            }
        }

        public override void Execute(Npc npc, Character character)
        {
            var dialog = new NpcDialog(character, npc);

            dialog.Open();
        }
    }
}