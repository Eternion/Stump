using System.Drawing;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Maps.Cells.Shapes;
using Stump.Server.WorldServer.Game.Spells;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Fights.Triggers
{
    public class Rune : MarkTrigger
    {
        public Rune(short id, FightActor caster, Spell spell, EffectDice originEffect, Spell runeSpell, Cell centerCell, SpellShapeEnum shape, byte minSize, byte size)
            : base(id, caster, spell, originEffect, centerCell, new MarkShape(caster.Fight, centerCell, shape, GetMarkShape(shape), minSize, size, GetRuneColorBySpell(spell)))
        {
            RuneSpell = runeSpell;
            Duration = originEffect.Duration;
        }

        public Spell RuneSpell
        {
            get;
        }

        public int Duration
        {
            get;
            private set;
        }

        public override bool StopMovement => false;

        public override GameActionMarkTypeEnum Type => GameActionMarkTypeEnum.RUNE;

        public override TriggerType TriggerType => TriggerType.NEVER;

        public override bool DoesSeeTrigger(FightActor fighter) => true;

        public override bool DecrementDuration()
        {
            if (Duration == -1)
                return false;

            return (--Duration) <= 0;
        }

        public override void Trigger(FightActor trigger, Cell triggerCell)
        {
            NotifyTriggered(trigger, RuneSpell);
            var handler = SpellManager.Instance.GetSpellCastHandler(Caster, RuneSpell, Shape.Cell, false);
            handler.MarkTrigger = this;
            handler.TriggerCell = triggerCell;
            handler.Initialize();

            foreach (var effectHandler in handler.GetEffectHandlers())
            {
                effectHandler.EffectZone = new Zone(effectHandler.Effect.ZoneShape, Shape.Size);

                if (!effectHandler.GetAffectedActors().Any() && effectHandler.IsValidTarget(trigger))
                    effectHandler.SetAffectedActors(new[] { trigger });
            }

            handler.Execute();

            Remove();
        }

        public override GameActionMark GetHiddenGameActionMark() => new GameActionMark(Caster.Id, (sbyte)Caster.Team.Id, CastedSpell.Template.Id, (sbyte)CastedSpell.CurrentLevel, Id, (sbyte)Type, -1,
                                      new GameActionMarkedCell[0], true);

        public override GameActionMark GetGameActionMark() => new GameActionMark(Caster.Id, (sbyte)Caster.Team.Id, CastedSpell.Template.Id, (sbyte)CastedSpell.CurrentLevel, Id, (sbyte)Type, CenterCell.Id,
                                      Shape.GetGameActionMarkedCells(), true);

        public override bool CanTrigger(FightActor actor) => false;

        private static Color GetRuneColorBySpell(Spell spell)
        {
            switch (spell.Id)
            {
                case (int)SpellIdEnum.RUNE_TERRE:
                    return Color.Brown;
                case (int)SpellIdEnum.RUNE_FEU:
                    return Color.Red;
                case (int)SpellIdEnum.RUNE_EAU:
                    return Color.Blue;
                case (int)SpellIdEnum.RUNE_AIR:
                    return Color.Green;
                default:
                    return Color.Black;
            }
        }
    }
}