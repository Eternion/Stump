using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Fights
{
    public class FightAgression : Fight
    {
        public FightAgression(int id, Map fightMap, FightTeam blueTeam, FightTeam redTeam) : base(id, fightMap, blueTeam, redTeam)
        {
            m_placementTimer = Map.Area.CallDelayed(PlacementPhaseTime, StartFighting);
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

        public override FightTypeEnum FightType
        {
            get { return FightTypeEnum.FIGHT_TYPE_AGRESSION; }
        }

        protected override IEnumerable<IFightResult> GenerateResults()
        {
            foreach (var character in GetAllFighters<CharacterFighter>())
            {
                character.SetEarnedHonor(CalculateEarnedHonor(character));
                character.SetEarnedDishonor(CalculateEarnedDishonor(character));
            }

            return GetFightersAndLeavers().Where(entry => !( entry is SummonedFighter )).Select(fighter => fighter.GetFightResult());
        }

        protected override void SendGameFightJoinMessage(CharacterFighter fighter)
        {
            ContextHandler.SendGameFightJoinMessage(fighter.Character.Client, CanCancelFight(), !IsStarted, false, IsStarted, GetPlacementTimeLeft(), FightType);
        }

        protected override void SendGameFightJoinMessage(FightSpectator spectator)
        {
            ContextHandler.SendGameFightJoinMessage(spectator.Character.Client, false, false, true, IsStarted, GetPlacementTimeLeft(), FightType);
        }

        public int GetPlacementTimeLeft()
        {
            double timeleft = PlacementPhaseTime - ( DateTime.Now - CreationTime ).TotalMilliseconds;

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

            var winnersLevel = (double)Winners.GetAllFightersWithLeavers().Sum(entry => entry.Level);
            var losersLevel = (double)Losers.GetAllFightersWithLeavers().Sum(entry => entry.Level);

            var delta = Math.Floor(Math.Sqrt(character.Level) * 10 * ( losersLevel / winnersLevel ));

            if (Losers == character.Team)
                delta = -delta;

            return (short) delta;
        }


        public short CalculateEarnedDishonor(CharacterFighter character)
        {
            if (Draw)
                return 0;

            if (character.OpposedTeam.AlignmentSide != AlignmentSideEnum.ALIGNMENT_NEUTRAL)
                return 0;

            return 1;
        }
    }
}