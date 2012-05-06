using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Formulas;
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
                fighter.SetEarnedExperience(FightFormulas.CalculateWinExp(fighter));

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
                    looter.Loot.Kamas = FightFormulas.AdjustDroppedKamas(looter, teamPP, kamas);

                    foreach (FightActor dropper in droppers)
                    {
                        foreach (DroppedItem item in dropper.RollLoot(looter))
                            looter.Loot.AddItem(item);
                    }
                }
            }
        }
    }
}