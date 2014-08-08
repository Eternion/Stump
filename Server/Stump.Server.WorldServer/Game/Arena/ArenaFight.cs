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

        public override bool IsDeathTemporarily
        {
            get { return true; }
        }

        public override void StartPlacement()
        {            
             ContextHandler.SendGameRolePlayArenaRegistrationStatusMessage(Clients, false,
                            PvpArenaStepEnum.ARENA_STEP_STARTING_FIGHT, PvpArenaTypeEnum.ARENA_TYPE_3VS3);

            base.StartPlacement();

            m_placementTimer = Map.Area.CallDelayed(PlacementPhaseTime, StartFighting);
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
            var timeleft = PlacementPhaseTime - ( DateTime.Now - CreationTime ).TotalMilliseconds;

            if (timeleft < 0)
                timeleft = 0;

            return (int)timeleft;
        }

        protected override IEnumerable<IFightResult> GenerateResults()
        {
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
            var opposedTeamRank = (int)leaver.OpposedTeam.GetAllFightersWithLeavers().OfType<CharacterFighter>().Average(x => x.Character.ArenaRank);
            var rankLose = ArenaRankFormulas.AdjustRank(leaver.Character.ArenaRank, opposedTeamRank, false);
            leaverResult = null;

            var list = new List<IFightResult>();
            foreach (var fighter in GetFightersAndLeavers().OfType<CharacterFighter>())
            {
                var outcome = fighter.Team == leaver.Team
                    ? FightOutcomeEnum.RESULT_LOST
                    : FightOutcomeEnum.RESULT_VICTORY;

                var result = new ArenaFightResult(fighter, outcome, fighter.Loot, fighter == leaver ? rankLose : 0);

                if (fighter == leaver)
                    leaverResult = result;

                list.Add(result);
            }

            return list;
        }

        protected override void OnPlayerLeft(FightActor fighter)
        {
            if (fighter is CharacterFighter)
            {
                (fighter as CharacterFighter).Character.ToggleArenaPenality();
            }

            base.OnPlayerLeft(fighter);
        }

        protected override void OnCharacterAdded(CharacterFighter fighter)
        {
            var shield = fighter.Character.Inventory.TryGetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_SHIELD);

            if (shield != null)
                fighter.Character.Inventory.MoveItem(shield, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);

            base.OnCharacterAdded(fighter);
        }

        protected override bool CanCancelFight()
        {
            return false;
        }
    }
}