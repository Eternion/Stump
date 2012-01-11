using System;
using NLog;
using Stump.Server.WorldServer.AI.Fights.Brain;
using Stump.Server.WorldServer.Worlds.Fights;
using Stump.Server.WorldServer.Worlds.Spells;

namespace Stump.Server.WorldServer.Worlds.Actors.Fight
{
    public abstract class AIFighter : FightActor
    {
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        protected AIFighter(FightTeam team, Spell[] spells)
            : base(team)
        {
            Spells = spells;
            Brain = new Brain(this);
            Fight.TimeLine.TurnStarted += OnTurnStarted;
            Fight.RequestTurnReady += OnRequestTurnReady;
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

        public override bool IsReady
        {
            get { return true; }
            protected set { }
        }

        public override bool IsTurnReady
        {
            get { return true; }
            internal set { }
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
            try
            {
                Brain.Play();
            }
            catch (Exception ex)
            {
                logger.Error("Monster {0}, AI engine failed : {1}", this, ex);
            }
            finally
            {
                Fight.RequestTurnEnd(this);
            }
        }
    }
}