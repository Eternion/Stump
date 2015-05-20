using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.AI.Fights.Actions;
using Stump.Server.WorldServer.AI.Fights.Spells;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Summon;
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
           
            var target = Environment.GetNearestFighter(x => x is SummonedMonster
            && ((SummonedMonster) x).Monster.MonsterId == 494);

            var spellId = (int) SpellIdEnum.PARADE_DES_VIEUX_JOUETS;

            if (target == null)
            {
                target = Environment.GetNearestEnemy();
                spellId = (int)SpellIdEnum.ASPIR_NENFAN;
            }

            var cell = Environment.GetCellToCastSpell(new TargetCell(target.Cell),
                new Spell(spellId, (byte)monster.Monster.Grade.Level), true);

            if (cell != null)
            {
                var action = new MoveAction(Fighter, cell);

                foreach (var result in action.Execute(this))
                {
                    if (result == RunStatus.Failure)
                        break;
                }
            }

            if (spellId == (int)SpellIdEnum.PARADE_DES_VIEUX_JOUETS && target.Position.Point.IsAdjacentTo(monster.Position.Point))
            {
                Fighter.CastSpell(new Spell((int)SpellIdEnum.PARADE_DES_VIEUX_JOUETS, (byte)monster.Monster.Grade.Level), target.Cell);
            }

            base.Play();
        }
    }
}
