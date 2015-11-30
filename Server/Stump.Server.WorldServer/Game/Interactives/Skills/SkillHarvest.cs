using System;
using System.Web.UI;
using Stump.Core.Attributes;
using Stump.Core.Collections;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Handlers.Inventory;
using Stump.Core.Mathematics;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    public class SkillHarvest : Skill
    {
        [Variable]
        public static int HarvestTime = 3000;

        [Variable]
        public static int RegrowTime = 60000;

        private ItemTemplate m_harvestedItem;

        public SkillHarvest(int id, InteractiveSkillTemplate skillTemplate, InteractiveObject interactiveObject)
            : base(id, skillTemplate, interactiveObject)
        {
            m_harvestedItem = ItemManager.Instance.TryGetTemplate(SkillTemplate.GatheredRessourceItem);

            if (m_harvestedItem == null)
                throw new Exception($"Harvested item {SkillTemplate.GatheredRessourceItem} doesn't exist");
        }

        public bool Harvested
        {
            get;
            private set;
        }

        public DateTime? HarvestedSince
        {
            get;
            private set;
        }

        public override int GetDuration(Character character)
        {
            return HarvestTime;
        }

        public override bool IsEnabled(Character character)
        {
            return !Harvested && character.Jobs[SkillTemplate.ParentJobId].Level >= SkillTemplate.LevelMin;
        }

        public override int StartExecute(Character character)
        {
            InteractiveObject.SetInteractiveState(InteractiveStateEnum.STATE_COLLECTING);
            SetHarvestedState(true);

            return GetDuration(character);
        }

        public override void EndExecute(Character character)
        {
            InteractiveObject.SetInteractiveState(InteractiveStateEnum.STATE_CUT);
            var count = RollHarvestedItemCount(character);

            character.Inventory.AddItem(m_harvestedItem, count);
            InventoryHandler.SendObtainedItemMessage(character.Client, m_harvestedItem, count);
        }

        public void SetHarvestedState(bool state)
        {
            Harvested = state;
            HarvestedSince = state ? (DateTime?)DateTime.Now : null;
            InteractiveObject.Map.Refresh(InteractiveObject);

            if (!state)
                InteractiveObject.SetInteractiveState(InteractiveStateEnum.STATE_NORMAL);
        }

        int RollHarvestedItemCount(Character character)
        {
            var max = (int)(7 + Math.Floor(character.Jobs[SkillTemplate.ParentJobId].Level / 5.0));

            return new CryptoRandom().Next(1, max);
        }
    }
}