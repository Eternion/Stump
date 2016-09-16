using System;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Database.Npcs.Actions;
using Stump.Server.WorldServer.Database.Quests;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Game.Quests.Objectives
{
    public class BringItemsToObjective : QuestObjective
    {
        public BringItemsToObjective(Character character, QuestObjectiveTemplate template, bool finished, int npcId, int amount, int itemId)
            : base(character, template, finished)
        {
            Npc = NpcManager.Instance.GetNpcTemplate(npcId);
            Amount = amount;
            Item = ItemManager.Instance.TryGetTemplate(itemId);
        }

        public NpcTemplate Npc
        {
            get;
            set;
        }

        public int Amount
        {
            get;
            set;
        }

        public ItemTemplate Item
        {
            get;
            set;
        }

        public override void EnableObjective()
        {
            Character.InteractingWith += CharacterOnInteractingWith;
        }

        public override void DisableObjective()
        {

        }

        public bool 

        private void CharacterOnInteractingWith(Character character, Npc npc, NpcActionTypeEnum actionType, NpcAction action)
        {
            if (npc.Template != Npc || actionType != NpcActionTypeEnum.ACTION_TALK)
        }

        public override QuestObjectiveInformations GetQuestObjectiveInformations()
        {
            throw new NotImplementedException();
        }
    }
}