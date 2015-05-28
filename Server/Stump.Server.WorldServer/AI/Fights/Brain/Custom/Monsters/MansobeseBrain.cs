using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.AI.Fights.Actions;
using Stump.Server.WorldServer.AI.Fights.Spells;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Spells;
using TreeSharp;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Monsters
{
    [BrainIdentifier(2856)]
    public class MansobeseBrain : Brain
    {
        public MansobeseBrain(AIFighter fighter)
            : base(fighter)
        {
        }

        public override void Play()
        {
            var monster = Fighter as MonsterFighter;

            if (monster == null)
                return;

            var target = Environment.GetNearestAlly();
            var spell = new Spell((int)SpellIdEnum.MANSOLDAT, (byte)monster.Monster.Grade.Level);

            if (Fighter.CanCastSpell(spell, target.Cell) == SpellCastResult.OK)
            {
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

                    Fighter.CastSpell(spell, target.Cell);
                }
            }

            base.Play();
        }
    }
}
