using System;
using System.Linq;
using Stump.Server.WorldServer.AI.Fights.Brain;
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
            Brain = new Brain(this);
            Fight.TimeLine.TurnStarted += OnTurnStarted;
            Fight.RequestTurnReady += OnRequestTurnReady;

            IsReady = true;
        }

        public Brain Brain
        {
            get;
            private set;
        }

        public Spell[] Spells
        {
            get;
            private set;
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
            Brain.Play();

            Fight.RequestTurnEnd(this);
        }

        #region AI Methods
        public FightActor GetNearestFighter()
        {
            return GetNearestFighter(entry => true);
        }

        public FightActor GetNearestAlly()
        {
            return GetNearestFighter(entry => entry.IsFriendlyWith(this));
        }

        public FightActor GetNearestEnnemy()
        {
            return GetNearestFighter(entry => entry.IsEnnemyWith(this));
        }

        public FightActor GetNearestFighter(Predicate<FightActor> predicate)
        {
            return Fight.GetAllFighters(predicate).OrderBy(entry => entry.Position.Point.DistanceTo(Position.Point)).FirstOrDefault();
        }
        #endregion
    }
}