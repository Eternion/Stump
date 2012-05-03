using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Fights
{
    public class FightPvM : Fight
    {
        private bool m_ageBonusDefined;

        public FightPvM(int id, Map fightMap, FightTeam blueTeam, FightTeam redTeam)
            : base(id, fightMap, blueTeam, redTeam)
        {
        }

        public override void StartPlacement()
        {
            base.StartPlacement();

            m_placementTimer = Map.Area.CallDelayed(PlacementPhaseTime, StartFighting);
        }

        public override void StartFighting()
        {
            m_placementTimer.Stop();

            base.StartFighting();
        }

        protected override void OnFighterAdded(FightTeam team, FightActor actor)
        {
            base.OnFighterAdded(team, actor);

            if (team.IsMonsterTeam() && !m_ageBonusDefined)
            {
                var monsterFighter = team.Leader as MonsterFighter;
                if (monsterFighter != null)
                    AgeBonus = monsterFighter.Monster.Group.AgeBonus;

                m_ageBonusDefined = true;
            }
        }

        public override FightTypeEnum FightType
        {
            get { return FightTypeEnum.FIGHT_TYPE_PvM; }
        }

        protected override IEnumerable<IFightResult> GenerateResults()
        {
            ShareLoots();

            foreach (CharacterFighter fighter in GetAllFighters<CharacterFighter>())
                fighter.SetEarnedExperience(CalculateWinExp(fighter));

            return GetFightersAndLeavers().Where(entry => !(entry is SummonedFighter)).Select(entry => entry.GetFightResult());
        }

        protected override void SendGameFightJoinMessage(CharacterFighter fighter)
        {
            ContextHandler.SendGameFightJoinMessage(fighter.Character.Client, true, !IsStarted, false, IsStarted, GetPlacementTimeLeft(), FightType);
        }

        protected override void SendGameFightJoinMessage(FightSpectator spectator)
        {
            ContextHandler.SendGameFightJoinMessage(spectator.Character.Client, false, !IsStarted, true, IsStarted, GetPlacementTimeLeft(), FightType);
        }

        protected override bool CanCancelFight()
        {
            return false;
        }

        public int GetPlacementTimeLeft()
        {
            double timeleft = PlacementPhaseTime - ( DateTime.Now - CreationTime ).TotalMilliseconds;

            if (timeleft < 0)
                timeleft = 0;

            return (int)timeleft;
        }

        #region Formulas
        public static readonly double[] GroupCoefficients =
            new[]
                {
                    1,
                    1.1,
                    1.5,
                    2.3,
                    3.1,
                    3.6,
                    4.2,
                    4.7
                };


        private int CalculateWinExp(CharacterFighter fighter)
        {
            if (fighter.HasLeft())
                return 0;

            IEnumerable<MonsterFighter> monsters = fighter.OpposedTeam.GetAllFighters<MonsterFighter>(entry => entry.IsDead()).ToList();
            IEnumerable<CharacterFighter> players = fighter.Team.GetAllFighters<CharacterFighter>().ToList();

            if (!monsters.Any() || !players.Any())
                return 0;

            int sumPlayersLevel = players.Sum(entry => entry.Level);
            byte maxPlayerLevel = players.Max(entry => entry.Level);
            int sumMonstersLevel = monsters.Sum(entry => entry.Level);
            byte maxMonsterLevel = monsters.Max(entry => entry.Level);
            int sumMonsterXp = monsters.Sum(entry => entry.Monster.Grade.GradeXp);

            double levelCoeff = 1;
            if (sumPlayersLevel - 5 > sumMonstersLevel)
                levelCoeff = (double)sumMonstersLevel / sumPlayersLevel;
            else if (sumPlayersLevel + 10 < sumMonstersLevel)
                levelCoeff = ( sumPlayersLevel + 10 ) / (double)sumMonstersLevel;

            double xpRatio = Math.Min(fighter.Level, Math.Truncate(2.5d * maxMonsterLevel)) / sumPlayersLevel * 100d;

            int regularGroupRatio = players.Where(entry => entry.Level >= maxPlayerLevel / 3).Sum(entry => 1);

            if (regularGroupRatio <= 0)
                regularGroupRatio = 1;

            double baseXp = Math.Truncate(xpRatio / 100 * Math.Truncate(sumMonsterXp * GroupCoefficients[regularGroupRatio - 1] * levelCoeff));
            double multiplicator = AgeBonus <= 0 ? 1 : 1 + AgeBonus / 100d;
            var xp = (int)Math.Truncate(Math.Truncate(baseXp * ( 100 + fighter.Stats[PlayerFields.Wisdom].Total ) / 100d) * multiplicator * Rates.XpRate);

            return xp;
        }

        private void ShareLoots()
        {
            foreach (FightTeam team in m_teams)
            {
                IEnumerable<FightActor> droppers = ( team == RedTeam ? BlueTeam : RedTeam ).GetAllFighters(entry => entry.IsDead()).ToList();
                IOrderedEnumerable<CharacterFighter> looters = team.GetAllFighters<CharacterFighter>().OrderByDescending(entry => entry.Stats[PlayerFields.Prospecting].Total);
                int teamPP = team.GetAllFighters().Sum(entry => entry.Stats[PlayerFields.Prospecting].Total);
                long kamas = droppers.Sum(entry => entry.GetDroppedKamas());

                foreach (CharacterFighter looter in looters)
                {
                    int looterPP = looter.Stats[PlayerFields.Prospecting].Total;

                    looter.Loot.Kamas = (int)( kamas * ( (double)looterPP / teamPP ) * Rates.KamasRate );

                    foreach (FightActor dropper in droppers)
                    {
                        foreach (DroppedItem item in dropper.RollLoot(looter))
                            looter.Loot.AddItem(item);
                    }
                }
            }
        }

        #endregion
    }
}