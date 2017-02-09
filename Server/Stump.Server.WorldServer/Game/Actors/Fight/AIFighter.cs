using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.AI.Fights.Brain;
using Stump.Server.WorldServer.Game.Actors.Interfaces;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Chat;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public abstract class AIFighter : FightActor, INamedActor
    {
        protected AIFighter(FightTeam team, IEnumerable<Spell> spells)
            : base(team)
        {
            Spells = spells.ToDictionary(entry => entry.Id);
            Brain = BrainManager.Instance.GetDefaultBrain(this);
            Fight.TurnStarted += OnTurnStarted;
        }

        protected AIFighter(FightTeam team, IEnumerable<Spell> spells, int identifier)
            : base(team)
        {
            Spells = spells.ToDictionary(entry => entry.Id);
            Brain = BrainManager.Instance.GetBrain(identifier, this);
            Fight.TurnStarted += OnTurnStarted;
        }

        public Brain Brain
        {
            get;
            protected set;
        }

        public Dictionary<int, Spell> Spells
        {
            get;
        }

        public override Spell GetSpell(int id) => Spells.ContainsKey(id) ? Spells[id] : null;

        public override bool HasSpell(int id) => Spells.ContainsKey(id);

        public abstract string Name
        {
            get;
        }

        public override bool IsReady => true;

        protected virtual void OnTurnStarted(IFight fight, FightActor currentfighter)
        {
            if (!IsFighterTurn())
                return;

            PlayIA();
        }

        private void PlayIA()
        {
            try
            {
                if (CanPlay())
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
                if (!Fight.AIDebugMode)
                    Fight.StopTurn();
            }
        }

        public void Say(string msg)
        {
            ChatHandler.SendChatServerMessage(Fight.Clients, this, ChatActivableChannelsEnum.CHANNEL_GLOBAL, msg);
        }
    }
}