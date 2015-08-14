using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Fights
{
    public class FightAgression : Fight<FightPlayerTeam, FightPlayerTeam>
    {
        public FightAgression(int id, Map fightMap, FightPlayerTeam defendersTeam, FightPlayerTeam challengersTeam)
            : base(id, fightMap, defendersTeam, challengersTeam)
        {
            m_placementTimer = Map.Area.CallDelayed(FightConfiguration.PlacementPhaseTime, StartFighting);
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

        public override FightTypeEnum FightType
        {
            get { return FightTypeEnum.FIGHT_TYPE_AGRESSION; }
        }

        public override bool IsPvP
        {
            get { return true; }
        }
        public override bool IsMultiAccountRestricted
        {
            get { return true; }
        }

        protected override IEnumerable<IFightResult> GenerateResults()
        {
            var results = GetFightersAndLeavers().Where(entry => !(entry is SummonedFighter) && !(entry is SummonedBomb) && !(entry is SlaveFighter)).
                Select(fighter => fighter.GetFightResult()).ToArray();

            foreach (var playerResult in results.OfType<FightPlayerResult>())
            {
                playerResult.SetEarnedHonor(CalculateEarnedHonor(playerResult.Fighter),
                    CalculateEarnedDishonor(playerResult.Fighter));
            }

            return results;
        }

        protected override void SendGameFightJoinMessage(CharacterFighter fighter)
        {
            ContextHandler.SendGameFightJoinMessage(fighter.Character.Client, CanCancelFight(), !IsStarted, false, IsStarted, GetPlacementTimeLeft(), FightType);
        }

        protected override void SendGameFightJoinMessage(FightSpectator spectator)
        {
            ContextHandler.SendGameFightJoinMessage(spectator.Character.Client, false, false, true, IsStarted, GetPlacementTimeLeft(), FightType);
        }

        public override int GetPlacementTimeLeft()
        {
            var timeleft = FightConfiguration.PlacementPhaseTime - ( DateTime.Now - CreationTime ).TotalMilliseconds;

            if (timeleft < 0)
                timeleft = 0;

            return (int)timeleft;
        }

        protected override bool CanCancelFight()
        {
            return false;
        }

        public short CalculateEarnedHonor(CharacterFighter character)
        {
            if (Draw)
                return 0;

            if (character.OpposedTeam.AlignmentSide == AlignmentSideEnum.ALIGNMENT_NEUTRAL)
                return 0;

            var winnersLevel = (double)Winners.GetAllFightersWithLeavers<CharacterFighter>().Sum(entry => entry.Level);
            var losersLevel = (double)Losers.GetAllFightersWithLeavers<CharacterFighter>().Sum(entry => entry.Level);

            var delta = Math.Floor(Math.Sqrt(character.Level) * 10 * ( losersLevel / winnersLevel ));

            if (Losers == character.Team)
                delta = -delta;

            return (short) delta;
        }


        public short CalculateEarnedDishonor(CharacterFighter character)
        {
            if (Draw)
                return 0;

            return character.OpposedTeam.AlignmentSide != AlignmentSideEnum.ALIGNMENT_NEUTRAL ? (short) 0 : (short) 1;
        }
    }
}