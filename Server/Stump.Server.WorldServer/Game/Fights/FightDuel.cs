﻿using System;
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
    public class FightDuel : Fight<FightPlayerTeam, FightPlayerTeam>
    {
        public FightDuel(int id, Map fightMap, FightPlayerTeam defendersTeam, FightPlayerTeam challengersTeam)
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

        public override bool IsDeathTemporarily => true;

        public override FightTypeEnum FightType => FightTypeEnum.FIGHT_TYPE_CHALLENGE;

        public override bool IsPvP => true;

        public override bool IsMultiAccountRestricted => false;

        protected override IEnumerable<IFightResult> GenerateResults()
        {
            base.GenerateResults();

            return GetFightersAndLeavers().Where(entry => !(entry is SummonedFighter) && !(entry is SummonedBomb) && !(entry is SlaveFighter)).Select(fighter => fighter.GetFightResult());
        }

        protected override void SendGameFightJoinMessage(CharacterFighter fighter)
        {
            ContextHandler.SendGameFightJoinMessage(fighter.Character.Client, CanCancelFight(), !IsStarted, IsStarted, (int)GetPlacementTimeLeft().TotalMilliseconds, FightType);
        }

        public override TimeSpan GetPlacementTimeLeft()
        {
            var timeleft = TimeSpan.FromMilliseconds(FightConfiguration.PlacementPhaseTime) - ( DateTime.Now - CreationTime );

            if (timeleft < TimeSpan.Zero)
                timeleft = TimeSpan.Zero;

            return timeleft;
        }

        protected override bool CanCancelFight() => State == FightState.Placement;
    }
}