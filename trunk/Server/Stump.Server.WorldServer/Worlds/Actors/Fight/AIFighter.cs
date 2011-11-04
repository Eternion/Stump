using System;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Stats;
using Stump.Server.WorldServer.Worlds.Fights;
using Stump.Server.WorldServer.Worlds.Maps.Cells;
using Stump.Server.WorldServer.Worlds.Maps.Pathfinding;
using Stump.Server.WorldServer.Worlds.Spells;

namespace Stump.Server.WorldServer.Worlds.Actors.Fight
{
    public abstract class AIFighter : FightActor
    {
        protected AIFighter(FightTeam team, Spell[] spells)
            : base(team)
        {
            Spells = spells;
            Fight.TimeLine.TurnStarted += OnTurnStarted;
            Fight.RequestTurnReady += OnRequestTurnReady;

            IsReady = true;
        }

        public Spell[] Spells
        {
            get;
            set;
        }

        private void OnRequestTurnReady(Fights.Fight obj)
        {
            ToggleTurnReady(true);
        }

        private void OnTurnStarted(TimeLine sender, FightActor currentfighter)
        {
            if (!IsFighterTurn())
                return;

            PlayIA();
        }

        private void PlayIA()
        {
            Fight.RequestTurnEnd(this);
        }
    }
}