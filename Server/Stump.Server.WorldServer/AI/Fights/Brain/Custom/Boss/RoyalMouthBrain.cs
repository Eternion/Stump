using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.States;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Boss
{
    [BrainIdentifier(2854)]
    public class RoyalMouthBrain : Brain
    {
        private SpellEffectHandler[] m_iniMouthHandlers;
        private FightActor m_invulnerabilityBreaker;

        public RoyalMouthBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.ActorMoved += OnActorMoved;
            fighter.Dead += OnDead;
            fighter.Fight.TurnStarted += OnTurnStarted;
            fighter.Fight.FightStarted += OnFightStarted;
        }

        private void OnFightStarted(IFight fight)
        {
            var inimouthSpell = SpellManager.Instance.GetSpellCastHandler(Fighter, new Spell((int)SpellIdEnum.INIMOUTH, 1), Fighter.Cell, false);
            inimouthSpell.Initialize();
            m_iniMouthHandlers = inimouthSpell.GetEffectHandlers().ToArray();

            Fighter.Fight.StartSequence(SequenceTypeEnum.SEQUENCE_SPELL);

            //Trigger Pushback Damages
            m_iniMouthHandlers[0].Apply();

            Fighter.Fight.EndSequence(SequenceTypeEnum.SEQUENCE_SPELL);

            AddInvulnerability();
        }

        private void OnDead(FightActor fighter, FightActor killer)
        {
            fighter.ActorMoved -= OnActorMoved;
            fighter.Dead -= OnDead;
            fighter.Fight.TurnStarted -= OnTurnStarted;

            if (m_invulnerabilityBreaker != null)
                m_invulnerabilityBreaker.Dead -= OnInvulnerabilityBreakerDead;
        }

        private void OnTurnStarted(IFight fight, FightActor player)
        {
            if (player != m_invulnerabilityBreaker)
                return;

            AddInvulnerability();

            player.Dead -= OnInvulnerabilityBreakerDead;
            m_invulnerabilityBreaker = null;
        }

        private void OnActorMoved(FightActor fighter, bool takeDamage)
        {
            if (fighter == Fighter)
                return;

            Fighter.Fight.StartSequence(SequenceTypeEnum.SEQUENCE_SPELL);

            //Disable Invulnerability
            m_iniMouthHandlers[1].Apply();

            m_iniMouthHandlers[2].Apply(); //Add MP

            Fighter.Fight.EndSequence(SequenceTypeEnum.SEQUENCE_SPELL);

            if (m_invulnerabilityBreaker != null)
                m_invulnerabilityBreaker.Dead -= OnInvulnerabilityBreakerDead;

            m_invulnerabilityBreaker = fighter;
            fighter.Dead += OnInvulnerabilityBreakerDead;
        }

        private void OnInvulnerabilityBreakerDead(FightActor fighter, FightActor killer)
        {
            AddInvulnerability();
        }

        private void AddInvulnerability()
        {
            Fighter.Fight.StartSequence(SequenceTypeEnum.SEQUENCE_SPELL);

            //Add State
            m_iniMouthHandlers[3].Apply();

            Fighter.Fight.EndSequence(SequenceTypeEnum.SEQUENCE_SPELL);
        }
    }
}
