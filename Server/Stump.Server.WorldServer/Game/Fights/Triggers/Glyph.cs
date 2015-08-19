using System.Drawing;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;
using Stump.Server.WorldServer.Game.Actors;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Fights.Triggers
{
    public class Glyph : MarkTrigger
    {
        private static readonly int[] SPELLS_GLYPH_END_TURN =
        {
            (int)SpellIdEnum.GLYPHE_DE_RÉPULSION,
            (int)SpellIdEnum.GLYPHE_DE_RÉPULSION_DU_DOPEUL,
            (int)SpellIdEnum.GLYPHE_DAIVAIN
        };

        public Glyph(short id, FightActor caster, Spell castedSpell, EffectDice originEffect, Spell glyphSpell,
                     Cell centerCell, SpellShapeEnum shape, byte size, Color color)
            : base(id, caster, castedSpell, originEffect, centerCell,
                new MarkShape(caster.Fight, centerCell, shape, GameActionMarkCellsTypeEnum.CELLS_CIRCLE, size, color))
        {
            GlyphSpell = glyphSpell;
            Duration = originEffect.Duration;
        }

        public Spell GlyphSpell
        {
            get;
            private set;
        }

        public int Duration
        {
            get;
            private set;
        }

        public override GameActionMarkTypeEnum Type
        {
            get { return GameActionMarkTypeEnum.GLYPH; }
        }

        public override TriggerType TriggerType
        {
            get { return OriginEffect.EffectId == EffectsEnum.Effect_GlyphAura ? (TriggerType.MOVE | TriggerType.CREATION) : (SPELLS_GLYPH_END_TURN.Contains(CastedSpell.Id) ? TriggerType.TURN_END : TriggerType.TURN_BEGIN); }
        }

        public override bool DecrementDuration()
        {
            if (Duration == -1)
                return false;

            return (--Duration) <= 0;
        }

        public bool IsGlyphAura()
        {
            return OriginEffect.EffectId == EffectsEnum.Effect_GlyphAura;
        }

        public override void Trigger(FightActor trigger)
        {
            NotifyTriggered(trigger, GlyphSpell);

            if (trigger == Caster && !IsGlyphAura())
                return;

            if (IsGlyphAura() && trigger.GetBuffs(b => b.Spell.Id == GlyphSpell.Id).Any())
            {
                trigger.PositionChanged += OnPositionChanged;
                return;
            }

            var handler = SpellManager.Instance.GetSpellCastHandler(Caster, GlyphSpell, trigger.Cell, false);
            handler.MarkTrigger = this;
            handler.Initialize();
            handler.Execute();

            if (IsGlyphAura())
                trigger.PositionChanged += OnPositionChanged;
        }

        private void OnPositionChanged(ContextActor actor, ObjectPosition position)
        {
            if (actor is FightActor)
            {
                if (Shapes.Any(x => x.GetCells().Any(c => c == position.Cell)))
                    return;

                ((FightActor)actor).RemoveSpellBuffs(GlyphSpell.Id);
            }
                

            actor.PositionChanged -= OnPositionChanged;
        }

        public override GameActionMark GetGameActionMark()
        {
            return new GameActionMark(Caster.Id, (sbyte)Caster.Team.Id, CastedSpell.Template.Id, (sbyte)CastedSpell.CurrentLevel, Id, (sbyte)Type, CenterCell.Id,
                                      Shapes[0].GetGameActionMarkedCells(), true);
        }

        public override GameActionMark GetHiddenGameActionMark()
        {
            return GetGameActionMark();
        }

        public override bool DoesSeeTrigger(FightActor fighter)
        {
            return true;
        }

        public override bool IsAffected(FightActor actor)
        {
            return true;
        }
    }
}