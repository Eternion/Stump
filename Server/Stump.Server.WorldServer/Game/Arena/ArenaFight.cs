using MongoDB.Bson;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Logging;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Handlers.Context;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Arena
{
    public class ArenaFight : Fight<ArenaTeam, ArenaTeam>
    {
        public ArenaFight(int id, Map fightMap, ArenaTeam defendersTeam, ArenaTeam challengersTeam)
            : base(id, fightMap, defendersTeam, challengersTeam)
        {
        }

        public override FightTypeEnum FightType => FightTypeEnum.FIGHT_TYPE_PVP_ARENA;

        public override bool IsPvP => false;

        public override bool IsMultiAccountRestricted => true;

        public override bool IsDeathTemporarily => true;

        public override bool CanKickPlayer => false;

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

        public override TimeSpan GetPlacementTimeLeft()
        {
            var timeleft = FightConfiguration.PlacementPhaseTime - (DateTime.Now - CreationTime).TotalMilliseconds;

            if (timeleft < 0)
                timeleft = 0;

            return TimeSpan.FromMilliseconds(timeleft);
        }

        protected override List<IFightResult> GetResults()
        {
            var challengersRank =
                (int)ChallengersTeam.GetAllFightersWithLeavers().OfType<CharacterFighter>().Average(x => x.Character.ArenaRank);
            var defendersRank =
                (int)DefendersTeam.GetAllFightersWithLeavers().OfType<CharacterFighter>().Average(x => x.Character.ArenaRank);

            var results = (from fighter in GetFightersAndLeavers().OfType<CharacterFighter>()
                    let outcome = fighter.GetFighterOutcome()
                    select new ArenaFightResult(fighter, outcome, fighter.Loot,
                        ArenaRankFormulas.AdjustRank(fighter.Character.ArenaRank,
                        fighter.Team == ChallengersTeam ? defendersRank : challengersRank,
                        outcome == FightOutcomeEnum.RESULT_VICTORY)) as IFightResult).ToList();

            foreach (var playerResult in results.OfType<FightPlayerResult>())
            {
                var document = new BsonDocument
                    {
                        { "FightId", UniqueId.ToString() },
                        { "FightType", Enum.GetName(typeof(FightTypeEnum), FightType) },
                        { "Duration", GetFightDuration().TotalMilliseconds },
                        { "Team", Enum.GetName(typeof(TeamEnum), playerResult.Fighter.Team.Id) },
                        { "Win", Winners.Id == playerResult.Fighter.Team.Id },
                        { "AcctId", playerResult.Character.Account.Id },
                        { "AcctName", playerResult.Character.Account.Login },
                        { "CharacterId", playerResult.Character.Id },
                        { "CharacterName", playerResult.Character.Name },
                        { "IPAddress", playerResult.Character.Client.IP },
                        { "ClientKey", playerResult.Character.Account.LastHardwareId },
                        { "Date", DateTime.Now.ToString(CultureInfo.InvariantCulture) }
                    };

                MongoLogger.Instance.Insert("fights_results", document);
            }

            return results;
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

            if (characterFighter.IsDisconnected)
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

        protected override bool CanCancelFight() => false;

        protected static int CalculateRankLoose(CharacterFighter character)
        {
            var opposedTeamRank = (int)character.OpposedTeam.GetAllFightersWithLeavers().OfType<CharacterFighter>().Average(x => x.Character.ArenaRank);
            return ArenaRankFormulas.AdjustRank(character.Character.ArenaRank, opposedTeamRank, false);
        }

        protected override void OnDisposed()
        {
            if (m_placementTimer != null)
                m_placementTimer.Dispose();

            base.OnDisposed();
        }
    }
}