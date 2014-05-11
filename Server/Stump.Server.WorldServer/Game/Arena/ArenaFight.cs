using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Maps;

namespace Stump.Server.WorldServer.Game.Arena
{
    public class ArenaFight : Fight<ArenaTeam, ArenaTeam>
    {
        public event Action<ArenaFight, Character> FightDenied;

        protected virtual void OnFightDenied(Character character)
        {
            Action<ArenaFight, Character> handler = FightDenied;
            if (handler != null) handler(this, character);
        }

        public ArenaFight(int id, Map fightMap, ArenaTeam defendersTeam, ArenaTeam challengersTeam)
            : base(id, fightMap, defendersTeam, challengersTeam)
        {
             m_placementTimer = Map.Area.CallDelayed(PlacementPhaseTime, StartFighting);
        }

        public override FightTypeEnum FightType
        {
            get { return FightTypeEnum.FIGHT_TYPE_PVP_ARENA; }
        }

        public void DenyFight(Character character)
        {
            if (State != FightState.NotStarted)
                throw new Exception("DenyFight() : State != FightState.NotStarted");

            OnFightDenied(character);
        }
         public override void StartPlacement()
        {
            base.StartPlacement();

            m_placementTimer = Map.Area.CallDelayed(PlacementPhaseTime, StartFighting);
        }

        public override void StartFighting()
        {
            m_placementTimer.Dispose();

            base.StartFighting();
        }
        public override int GetPlacementTimeLeft()
        {
            var timeleft = PlacementPhaseTime - ( DateTime.Now - CreationTime ).TotalMilliseconds;

            if (timeleft < 0)
                timeleft = 0;

            return (int)timeleft;
        }

        protected override IEnumerable<IFightResult> GenerateResults()
        {
            var challengersRank =
                (int) ChallengersTeam.Fighters.OfType<CharacterFighter>().Average(x => x.Character.ArenaRank);
            var defendersRank =
                (int) DefendersTeam.Fighters.OfType<CharacterFighter>().Average(x => x.Character.ArenaRank);

            foreach (CharacterFighter fighter in Fighters.OfType<CharacterFighter>())
            {
                FightOutcomeEnum outcome = fighter.GetFighterOutcome();
                yield return new ArenaFightResult(fighter, outcome, fighter.Loot,
                    ArenaRankFormulas.AdjustRank(fighter.Character.ArenaRank,
                        fighter.Team == ChallengersTeam ? defendersRank : challengersRank,
                        outcome == FightOutcomeEnum.RESULT_VICTORY));
            }
        }

        protected override bool CanCancelFight()
        {
            return false;
        }
    }
}