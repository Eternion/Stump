using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights.Results.Data;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Guilds;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Handlers.Characters;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Fights.Results
{
    public class FightPlayerResult : FightResult<CharacterFighter>, IExperienceResult, IPvpResult
    {
        public FightPlayerResult(CharacterFighter fighter, FightOutcomeEnum outcome, FightLoot loot)
            : base(fighter, outcome, loot)
        {
        }

        public Character Character => Fighter.Character;

        public new byte Level => Character.Level;

        public override bool CanLoot(FightTeam team) => Fighter.Team == team && (!Fighter.HasLeft() || Fighter.IsDisconnected);

        public FightExperienceData ExperienceData
        {
            get;
            private set;
        }

        public FightPvpData PvpData
        {
            get;
            private set;
        }

        public override FightResultListEntry GetFightResultListEntry()
        {
            var additionalDatas = new List<DofusProtocol.Types.FightResultAdditionalData>();

            if (ExperienceData != null)
                additionalDatas.Add(ExperienceData.GetFightResultAdditionalData());

            if (PvpData != null)
                additionalDatas.Add(PvpData.GetFightResultAdditionalData());

            return new FightResultPlayerListEntry((short) Outcome, 0, Loot.GetFightLoot(), Id, Alive, Level,
                additionalDatas);
        }

        public override void Apply()
        {
            Character.Inventory.AddKamas(Loot.Kamas);

            foreach (var drop in Loot.Items.Values)
            {
                // just diplay purpose
                if (drop.IgnoreGeneration)
                    continue;

                var template = ItemManager.Instance.TryGetTemplate(drop.ItemId);

                if (template.Effects.Count > 0)
                    for (var i = 0; i < drop.Amount; i++)
                    {
                        var item = ItemManager.Instance.CreatePlayerItem(Character, drop.ItemId, 1);
                        Character.Inventory.AddItem(item, false);
                    }
                else
                {
                    var item = ItemManager.Instance.CreatePlayerItem(Character, drop.ItemId, (int)drop.Amount);
                    Character.Inventory.AddItem(item, false);
                }
            }

            if (ExperienceData != null)
                ExperienceData.Apply();

            if (PvpData != null)
                PvpData.Apply();

            CharacterHandler.SendCharacterStatsListMessage(Character.Client);
            InventoryHandler.SendInventoryContentMessage(Character.Client);
        }

        public void AddEarnedExperience(int experience)
        {
            if (Fighter.HasLeft() && !Fighter.IsDisconnected)
                return;

            if (ExperienceData == null)
                ExperienceData = new FightExperienceData(Character);

            if (Character.IsRiding && Character.EquippedMount.GivenExperience > 0)
            {
                var xp = (int)(experience * (Character.EquippedMount.GivenExperience * 0.01));
                var mountXp = (int)Character.EquippedMount.AdjustGivenExperience(Character, xp);

                experience -= xp;

                if (mountXp > 0)
                {
                    ExperienceData.ShowExperienceForMount = true;
                    ExperienceData.ExperienceForMount += mountXp;
                }
            }

            if (Character.GuildMember != null && Character.GuildMember.GivenPercent > 0)
            {
                var xp = (int)(experience*(Character.GuildMember.GivenPercent*0.01));
                var guildXp = (int)Character.Guild.AdjustGivenExperience(Character, xp);

                experience -= xp;
                guildXp = guildXp > Guild.MaxGuildXP ? Guild.MaxGuildXP : guildXp;

                if (guildXp > 0)
                {
                    ExperienceData.ShowExperienceForGuild = true;
                    ExperienceData.ExperienceForGuild += guildXp;
                }
            }

            ExperienceData.ShowExperienceFightDelta = true;
            ExperienceData.ShowExperience = true;
            ExperienceData.ShowExperienceLevelFloor = true;
            ExperienceData.ShowExperienceNextLevelFloor = true;
            ExperienceData.ExperienceFightDelta += experience;
        }

        public void SetEarnedHonor(short honor, short dishonor)
        {
            if (PvpData == null)
                PvpData = new FightPvpData(Character);

            PvpData.HonorDelta = honor;
            PvpData.DishonorDelta = dishonor;
            PvpData.Honor = Character.Honor;
            PvpData.Dishonor = Character.Dishonor;
            PvpData.Grade = (byte) Character.AlignmentGrade;
            PvpData.MinHonorForGrade = Character.LowerBoundHonor;
            PvpData.MaxHonorForGrade = Character.UpperBoundHonor;
        }
    }
}