using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.States;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom
{
    [BrainIdentifier(2854)]
    public class RoyalMouthBrain : Brain
    {
        private SpellEffectHandler[] m_iniMouthHandlers;
        private FightActor m_invulnerabilityBreaker;

        public RoyalMouthBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.ActorPushed += OnActorPushed;
            fighter.Dead += OnDead;
            fighter.Fight.TurnStarted += OnTurnStarted;
            fighter.Fight.FightStarted += OnFightStarted;
        }

        private void OnFightStarted()
        {
            var inimouthSpell = SpellManager.Instance.GetSpellCastHandler(Fighter, new Spell((int)SpellIdEnum.INIMOUTH, 1), Fighter.Cell, false);
            inimouthSpell.Initialize();
            m_iniMouthHandlers = inimouthSpell.GetEffectHandlers().ToArray();

            SetInvulnerability(true);
        }

        private void OnDead(FightActor fighter, FightActor killer)
        {
            fighter.ActorPushed -= OnActorPushed;
            fighter.Dead -= OnDead;
            fighter.Fight.TurnStarted -= OnTurnStarted;

            if (m_invulnerabilityBreaker != null)
                m_invulnerabilityBreaker.Dead -= OnInvulnerabilityBreakerDead;
        }

        private void OnTurnStarted(IFight fight, FightActor player)
        {
            if (player != m_invulnerabilityBreaker)
                return;

            SetInvulnerability(true);

            player.Dead -= OnInvulnerabilityBreakerDead;
            m_invulnerabilityBreaker = null;
        }

        private void OnActorPushed(FightActor fighter, bool takeDamage)
        {
            SetInvulnerability(false);
            m_iniMouthHandlers[2].Apply(); //Add MP

            if (m_invulnerabilityBreaker != null)
                m_invulnerabilityBreaker.Dead -= OnInvulnerabilityBreakerDead;

            m_invulnerabilityBreaker = fighter;
            fighter.Dead += OnInvulnerabilityBreakerDead;

            if (takeDamage)
                KillAllEnemiesInLine(Fighter.Position.Point);
        }

        private void OnInvulnerabilityBreakerDead(FightActor fighter, FightActor killer)
        {
            SetInvulnerability(true);
        }

        private void SetInvulnerability(bool enable)
        {
            if (enable)
            {
                //Add State
                var state = m_iniMouthHandlers[3] as AddState;

                state.BypassDispel = true;
                state.Apply();
            }
            else
            {
                //Remove State
                m_iniMouthHandlers[1].Apply();
            }
        }

        private void KillAllEnemiesInLine(MapPoint point)
        {
            foreach (var spell in Fighter.OpposedTeam.GetAllFighters(x => x.IsAlive()).Where(enemy => point.IsOnSameLine(enemy.Position.Point))
                .Select(enemy => SpellManager.Instance.GetSpellCastHandler(Fighter, new Spell((int)SpellIdEnum.BERSERKMOUTH, 1), enemy.Cell, false)))
            {
                spell.Execute();
            }
        }
    }
}
