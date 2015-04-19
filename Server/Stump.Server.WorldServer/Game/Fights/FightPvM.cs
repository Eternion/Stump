using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Challenges;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Formulas;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Fights
{
    public class FightPvM : Fight<FightMonsterTeam, FightPlayerTeam>
    {
        private bool m_ageBonusDefined;

        public FightPvM(int id, Map fightMap, FightMonsterTeam defendersTeam, FightPlayerTeam challengersTeam)
            : base(id, fightMap, defendersTeam, challengersTeam)
        {
        }

        public override void StartPlacement()
        {
            base.StartPlacement();

            m_placementTimer = Map.Area.CallDelayed(FightConfiguration.PlacementPhaseTime, StartFighting);
        }

        public override void StartFighting()
        {
            m_placementTimer.Dispose();

            base.StartFighting();
        }

        protected override void OnFightStarted()
        {
            base.OnFightStarted();

            var challenge = ChallengeManager.Instance.GetRandomChallenge(this);
            challenge.Initialize();

            SetChallenge(challenge);
        }

        protected override void OnFighterAdded(FightTeam team, FightActor actor)
        {
            base.OnFighterAdded(team, actor);

            if (!(team is FightMonsterTeam) || m_ageBonusDefined)
                return;

            var monsterFighter = team.Leader as MonsterFighter;
            if (monsterFighter != null)
                AgeBonus = monsterFighter.Monster.Group.AgeBonus;

            m_ageBonusDefined = true;
        }

        public override FightTypeEnum FightType
        {
            get { return FightTypeEnum.FIGHT_TYPE_PvM; }
        }

        public override bool IsPvP
        {
            get { return false; }
        }

        protected override IEnumerable<IFightResult> GenerateResults()
        {
            base.GenerateResults();

            var results = new List<IFightResult>();
            results.AddRange(GetFightersAndLeavers().Where(entry => !(entry is SummonedFighter)
                && !(entry is SummonedBomb) && !(entry is SlaveFighter)).Select(entry => entry.GetFightResult()));

            if (Map.TaxCollector != null && Map.TaxCollector.CanGatherLoots())
                results.Add(new TaxCollectorProspectingResult(Map.TaxCollector, this));

            foreach (var team in m_teams)
            {
                IEnumerable<FightActor> droppers = team.OpposedTeam.GetAllFighters(entry => entry.IsDead()).ToList();
                var looters = results.Where(x => x.CanLoot(team)).OrderByDescending(entry => entry is TaxCollectorProspectingResult ? -1 : entry.Prospecting); // tax collector loots at the end
                var teamPP = team.GetAllFighters<CharacterFighter>().Sum(entry => entry.Stats[PlayerFields.Prospecting].Total);
                var kamas = droppers.Sum(entry => entry.GetDroppedKamas());

                foreach (var looter in looters)
                {
                    looter.Loot.Kamas = teamPP > 0 ? FightFormulas.AdjustDroppedKamas(looter, teamPP, kamas) : 0;

                    if (team == Winners)
                    {
                        foreach (var item in droppers.SelectMany(dropper => dropper.RollLoot(looter)))
                        {
                            looter.Loot.AddItem(item);
                        }
                    }

                    if (looter is IExperienceResult)
                    {
                        (looter as IExperienceResult).AddEarnedExperience(FightFormulas.CalculateWinExp(looter, team.GetAllFighters<CharacterFighter>(), droppers));
                    }
                }
            }


            return results;
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

        public override int GetPlacementTimeLeft()
        {
            var timeleft = FightConfiguration.PlacementPhaseTime - ( DateTime.Now - CreationTime ).TotalMilliseconds;

            if (timeleft < 0)
                timeleft = 0;

            return (int)timeleft;
        }

    }
}