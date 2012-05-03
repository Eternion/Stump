using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.AI.Fights.Brain;
using Stump.Server.WorldServer.Game.Actors.Interfaces;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Chat;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public abstract class AIFighter : FightActor, INamedActor
    {
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        protected AIFighter(FightTeam team, IEnumerable<Spell> spells)
            : base(team)
        {
            Spells = spells.ToDictionary(entry => entry.Id);
            Brain = new Brain(this);
            Fight.TurnStarted += OnTurnStarted;
        }

        public Brain Brain
        {
            get;
            private set;
        }

        public bool Frozen
        {
            get;
            set;
        }

        public Dictionary<int, Spell> Spells
        {
            get;
            private set;
        }

        public override Spell GetSpell(int id)
        {
            return Spells.ContainsKey(id) ? Spells[id] : null;
        }

        public override bool HasSpell(int id)
        {
            return Spells.ContainsKey(id);
        }

        public abstract string Name
        {
            get;
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

        private void OnTurnStarted(Fights.Fight fight, FightActor currentfighter)
        {
            if (!IsFighterTurn())
                return;

            PlayIA();
        }

        private void PlayIA()
        {
            try
            {
                if (!Frozen)
                    Brain.Play();
            }
            catch (Exception ex)
            {
                logger.Error("Monster {0}, AI engine failed : {1}", this, ex);

                if (Brain.DebugMode)
                    Say("My AI has just failed :s (" + ex.Message + ")");
            }
            finally
            {
                Fight.StopTurn();
            }
        }

        public void Say(string msg)
        {
            ChatHandler.SendChatServerMessage(Fight.Clients, this, ChatActivableChannelsEnum.CHANNEL_GLOBAL, msg);
        }
    }
}