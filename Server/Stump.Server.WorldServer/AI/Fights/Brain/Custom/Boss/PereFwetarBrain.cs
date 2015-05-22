using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.AI.Fights.Actions;
using Stump.Server.WorldServer.AI.Fights.Spells;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Spells;
using TreeSharp;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Boss
{
    [BrainIdentifier(1194)]
    public class PereFwetarBrain : Brain
    {
        public PereFwetarBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.Fight.TurnStarted += OnTurnStarted;
            SpellSelector.AnalysePossibilitiesFinished += OnAnalysePossibilitiesFinished;
        }

        private void OnTurnStarted(IFight fight, FightActor fighter)
        {
            if (fighter != Fighter)
                return;

            fighter.CastSpell(new Spell((int)SpellIdEnum.VILAIN_GARNEMENT, 1), fighter.Cell, true, true);
            fighter.Fight.TurnStarted -= OnTurnStarted;
        }

        public override void Play()
        {
            var monster = Fighter as MonsterFighter;

            if (monster == null)
                return;
           
           var target = Environment.GetNearestFighter(x => (x is SummonedMonster
            && ((SummonedMonster) x).Monster.MonsterId == 494) || !x.IsFriendlyWith(monster));

            if (target == null)
            {
                base.Play();
                return;
            }

            Environment.ResetMoveZone();

            var spellId = target.IsFriendlyWith(monster) ? (int)SpellIdEnum.PARADE_DES_VIEUX_JOUETS : (int)SpellIdEnum.ASPIR_NENFAN;
            var spell = new Spell(spellId, (byte) monster.Monster.Grade.Level);

            var cell = Environment.GetCellToCastSpell(new TargetCell(target.Cell),
                spell, spell.CurrentSpellLevel.CastTestLos);

            if (cell != null)
            {
                var action = new MoveAction(Fighter, cell);

                foreach (var result in action.Execute(this))
                {
                    if (result == RunStatus.Failure)
                        break;
                }
            }

            var actionMoveNear = new MoveNearTo(Fighter, target);

            foreach (var result in actionMoveNear.Execute(this))
            {
                if (result == RunStatus.Failure)
                    break;
            }

            if (spellId == (int)SpellIdEnum.PARADE_DES_VIEUX_JOUETS)
            {
                Fighter.CastSpell(new Spell(spellId, (byte)monster.Monster.Grade.Level), target.Cell);
            }

            base.Play();
        }

        private void OnAnalysePossibilitiesFinished(AIFighter obj)
        {
            if ((Fighter.Stats.Health.TotalMax / Fighter.Stats.Health.Total) < 3)
                SpellSelector.Possibilities.RemoveAll(x => x.Spell.Id == (int)SpellIdEnum.EMBÛCHE_DE_NOWEL);
        }
    }
}
