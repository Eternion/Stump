using System;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Handlers.Inventory;
using Stump.Core.Mathematics;
using Stump.Server.WorldServer.Game.Jobs;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    public class SkillHarvest : Skill
    {
        [Variable]
        public static int HarvestTime = 3000;

        [Variable]
        public static int RegrowTime = 60000;

        ItemTemplate m_harvestedItem;

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

        public override int GetDuration(Character character, bool forNetwork = false) => HarvestTime;

        public override bool IsEnabled(Character character)
            => base.IsEnabled(character) && !Harvested && character.Jobs[SkillTemplate.ParentJobId].Level >= SkillTemplate.LevelMin;

        public override int StartExecute(Character character)
        {
            InteractiveObject.SetInteractiveState(InteractiveStateEnum.STATE_ANIMATED);
            SetHarvestedState(true);

            base.StartExecute(character);

            return GetDuration(character);
        }

        public override void EndExecute(Character character)
        {
            InteractiveObject.SetInteractiveState(InteractiveStateEnum.STATE_ACTIVATED);
            var count = RollHarvestedItemCount(character);

            if (character.Inventory.IsFull(m_harvestedItem, count))
            {
                //Votre inventaire est plein. Votre récolte est perdue...
                character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 144);

                base.EndExecute(character);
                return;
            }

            character.Inventory.AddItem(m_harvestedItem, count);
            InventoryHandler.SendObtainedItemMessage(character.Client, m_harvestedItem, count);

            if (SkillTemplate.ParentJobId != 1)
            {
                var xp = JobManager.Instance.GetHarvestJobXp((int)SkillTemplate.LevelMin);
                character.Jobs[SkillTemplate.ParentJobId].Experience += xp;
            }

            base.EndExecute(character);
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
            var job = character.Jobs[SkillTemplate.ParentJobId];
            var minMax = JobManager.Instance.GetHarvestItemMinMax(job.Template, job.Level, SkillTemplate);
            return new CryptoRandom().Next(minMax.First, minMax.Second + 1);
        }
    }
}