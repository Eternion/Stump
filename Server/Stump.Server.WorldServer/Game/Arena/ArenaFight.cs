using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Arena
{
    public class ArenaFight : Fight<ArenaTeam, ArenaTeam>
    {
        public ArenaFight(int id, Map fightMap, ArenaTeam defendersTeam, ArenaTeam challengersTeam)
            : base(id, fightMap, defendersTeam, challengersTeam)
        {
        }

        public override FightTypeEnum FightType
        {
            get { return FightTypeEnum.FIGHT_TYPE_PVP_ARENA; }
        }

        public override bool IsPvP
        {
            get { return false; } // don't know why
        }
        public override bool IsMultiAccountRestricted
        {
            get { return true; }
        }

        public override bool IsDeathTemporarily
        {
            get { return true; }
        }

        public override bool CanKickPlayer
        {
            get { return false; }
        }

        public override void StartPlacement()
        {            
             ContextHandler.SendGameRolePlayArenaRegistrationStatusMessage(Clients, false,
                            PvpArenaStepEnum.ARENA_STEP_STARTING_FIGHT, PvpArenaTypeEnum.ARENA_TYPE_3VS3);

            base.StartPlacement();

            m_placementTimer = Map.Area.CallDelayed(FightConfiguration.PlacementPhaseTime, StartFighting);
        }

        public override void StartFighting()
        {
            m_placementTimer.Dispose();

            base.StartFighting();
        }

        protected override void OnFightEnded()
        {
            ContextHandler.SendGameRolePlayArenaRegistrationStatusMessage(Clients, false,
                    PvpArenaStepEnum.ARENA_STEP_UNREGISTER, PvpArenaTypeEnum.ARENA_TYPE_3VS3);

            base.OnFightEnded();
        }

        public override int GetPlacementTimeLeft()
        {
            var timeleft = FightConfiguration.PlacementPhaseTime - ( DateTime.Now - CreationTime ).TotalMilliseconds;

            if (timeleft < 0)
                timeleft = 0;

            return (int)timeleft;
        }

        protected override IEnumerable<IFightResult> GenerateResults()
        {
            base.GenerateResults();

            var challengersRank =
                (int) ChallengersTeam.GetAllFightersWithLeavers().OfType<CharacterFighter>().Average(x => x.Character.ArenaRank);
            var defendersRank =
                (int) DefendersTeam.GetAllFightersWithLeavers().OfType<CharacterFighter>().Average(x => x.Character.ArenaRank);

            return (from fighter in GetFightersAndLeavers().OfType<CharacterFighter>() let outcome = fighter.GetFighterOutcome() select new ArenaFightResult(fighter, outcome, fighter.Loot,
                ArenaRankFormulas.AdjustRank(fighter.Character.ArenaRank,
                    fighter.Team == ChallengersTeam ? defendersRank : challengersRank,
                    outcome == FightOutcomeEnum.RESULT_VICTORY)));
        }

        protected override IEnumerable<IFightResult> GenerateLeaverResults(CharacterFighter leaver, out IFightResult leaverResult)
        {
            var rankLoose = CalculateRankLoose(leaver);

            leaverResult = null;

            var list = new List<IFightResult>();
            foreach (var fighter in GetFightersAndLeavers().OfType<CharacterFighter>())
            {
                var outcome = fighter.Team == leaver.Team
                    ? FightOutcomeEnum.RESULT_LOST
                    : FightOutcomeEnum.RESULT_VICTORY;

                var result = new ArenaFightResult(fighter, outcome, new FightLoot(), fighter == leaver ? rankLoose : 0, false);

                if (fighter == leaver)
                    leaverResult = result;

                list.Add(result);
            }

            return list;
        }

        protected override void OnPlayerLeft(FightActor fighter)
        {
            base.OnPlayerLeft(fighter);

            var characterFighter = fighter as CharacterFighter;
            if (characterFighter == null)
                return;

            characterFighter.Character.ToggleArenaPenality();

            if (characterFighter.Character.ArenaParty != null)
                characterFighter.Character.LeaveParty(characterFighter.Character.ArenaParty);

            var rankLoose = CalculateRankLoose(characterFighter);
            characterFighter.Character.UpdateArenaProperties(rankLoose, false);
        }

        protected override void OnPlayerReadyToLeave(CharacterFighter characterFighter)
        {
            base.OnPlayerReadyToLeave(characterFighter);

            if (characterFighter.Character.ArenaParty != null)
                characterFighter.Character.LeaveParty(characterFighter.Character.ArenaParty);
        }

        protected override bool CanCancelFight()
        {
            return false;
        }

        protected int CalculateRankLoose(CharacterFighter character)
        {
            var opposedTeamRank = (int)character.OpposedTeam.GetAllFightersWithLeavers().OfType<CharacterFighter>().Average(x => x.Character.ArenaRank);
            return ArenaRankFormulas.AdjustRank(character.Character.ArenaRank, opposedTeamRank, false);
        }
    }
}