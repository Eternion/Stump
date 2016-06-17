using System;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Handlers.Inventory;
using Stump.Core.Mathematics;
using Stump.Core.Timers;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Jobs;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    public class SkillHarvest : Skill, ISkillWithAgeBonus
    {
        [Variable(true)]
        public static int StarsBonusRate = 1800;

        [Variable(true)]
        public static short StarsBonusLimit = 200;

        public const short ClientStarsBonusLimit = 200;

        [Variable]
        public static int HarvestTime = 3000;

        [Variable]
        public static int RegrowTime = 60000;

        ItemTemplate m_harvestedItem;
        private TimedTimerEntry m_regrowTimer;

        public SkillHarvest(int id, InteractiveSkillTemplate skillTemplate, InteractiveObject interactiveObject)
            : base(id, skillTemplate, interactiveObject)
        {
            m_harvestedItem = ItemManager.Instance.TryGetTemplate(SkillTemplate.GatheredRessourceItem);
            CreationDate = DateTime.Now;

            if (m_harvestedItem == null)
                throw new Exception($"Harvested item {SkillTemplate.GatheredRessourceItem} doesn't exist");
        }

        public bool Harvested => HarvestedSince.HasValue && (DateTime.Now - HarvestedSince).Value.TotalMilliseconds < RegrowTime;

        public DateTime CreationDate
        {
            get;
            private set;
        }

        public DateTime EnabledSince => HarvestedSince + TimeSpan.FromMilliseconds(RegrowTime) ?? CreationDate;

        public DateTime? HarvestedSince
        {
            get;
            private set;
        }


        public short AgeBonus
        {
            get
            {
                var bonus = (DateTime.Now - EnabledSince).TotalSeconds / (StarsBonusRate);

                if (bonus > StarsBonusLimit)
                    bonus = StarsBonusLimit;

                return (short)bonus;
            }
            set { HarvestedSince = DateTime.Now -TimeSpan.FromMilliseconds(RegrowTime) - TimeSpan.FromSeconds(value * StarsBonusRate); }
        }


        public override int GetDuration(Character character, bool forNetwork = false) => HarvestTime;

        public override bool IsEnabled(Character character)
            => base.IsEnabled(character) && !Harvested && character.Jobs[SkillTemplate.ParentJobId].Level >= SkillTemplate.LevelMin;

        public override int StartExecute(Character character)
        {
            InteractiveObject.SetInteractiveState(InteractiveStateEnum.STATE_ANIMATED);
            SetHarvested();

            base.StartExecute(character);

            return GetDuration(character);
        }

        public override void EndExecute(Character character)
        {
            InteractiveObject.SetInteractiveState(InteractiveStateEnum.STATE_ACTIVATED);
            var count = RollHarvestedItemCount(character);
            var bonus = (int)Math.Floor(count*(AgeBonus/100d));

            if (character.Inventory.IsFull(m_harvestedItem, count))
            {
                //Votre inventaire est plein. Votre récolte est perdue...
                character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 144);

                base.EndExecute(character);
                return;
            }

            character.Inventory.AddItem(m_harvestedItem, count + bonus);
            InventoryHandler.SendObtainedItemWithBonusMessage(character.Client, m_harvestedItem, count, bonus);

            if (SkillTemplate.ParentJobId != 1)
            {
                var xp = JobManager.Instance.GetHarvestJobXp((int)SkillTemplate.LevelMin);
                character.Jobs[SkillTemplate.ParentJobId].Experience += xp;
            }

            base.EndExecute(character);
        }

        public void SetHarvested()
        {
            HarvestedSince = DateTime.Now;
            InteractiveObject.Map.Refresh(InteractiveObject);
            m_regrowTimer = InteractiveObject.Area.CallDelayed(RegrowTime, Regrow);
        }

        public void Regrow()
        {
            if (m_regrowTimer != null)
            {
                m_regrowTimer.Stop();
                m_regrowTimer = null;
            }

            InteractiveObject.Map.Refresh(InteractiveObject);
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