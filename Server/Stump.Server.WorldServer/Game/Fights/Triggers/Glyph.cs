using System.Drawing;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Fights.Triggers
{
    public class Glyph : MarkTrigger
    {
        public Glyph(short id, FightActor caster, Spell castedSpell, EffectDice originEffect, Spell glyphSpell,
                     Cell centerCell, SpellShapeEnum shape, byte minSize, byte size, Color color, bool canBeForced, TriggerType triggerType = TriggerType.OnTurnBegin)
            : base(id, caster, castedSpell, originEffect, centerCell,
                new MarkShape(caster.Fight, centerCell, shape, GetMarkShape(shape), minSize, size, color))
        {
            GlyphSpell = glyphSpell;
            CanBeForced = canBeForced;
            Duration = originEffect.Duration;
            TriggerType = triggerType;
        }

        public Spell GlyphSpell
        {
            get;
        }

        public bool CanBeForced
        {
            get;
        }

        public int Duration
        {
            get;
            private set;
        }

        public override GameActionMarkTypeEnum Type => GameActionMarkTypeEnum.GLYPH;

        public override TriggerType TriggerType
        {
            get;
        }

        public override bool DecrementDuration()
        {
            if (Duration == -1)
                return false;

            return (--Duration) <= 0;
        }

        public override void Trigger(FightActor trigger, Cell triggerCell)
        {
            NotifyTriggered(trigger, GlyphSpell);
            
            var handler = SpellManager.Instance.GetSpellCastHandler(Caster, GlyphSpell, trigger.Cell, false);
            handler.MarkTrigger = this;
            handler.TriggerCell = triggerCell;
            handler.Initialize();

            foreach (var effectHandler in handler.GetEffectHandlers())
            {
                effectHandler.SetAffectedActors(effectHandler.IsValidTarget(trigger) ? new[] {trigger} : new FightActor[0]);
            }

            handler.Execute();
        }

        public void TriggerAllCells(FightActor trigger)
        {
            NotifyTriggered(trigger, GlyphSpell);

            var handler = SpellManager.Instance.GetSpellCastHandler(Caster, GlyphSpell, CenterCell, false);
            handler.MarkTrigger = this;
            handler.Initialize();

            handler.Execute();
        }

        public override GameActionMark GetGameActionMark()
            => new GameActionMark(Caster.Id, (sbyte)Caster.Team.Id, CastedSpell.Template.Id, (sbyte)CastedSpell.CurrentLevel, Id, (sbyte)Type, CenterCell.Id,
                                      Shape.GetGameActionMarkedCells(), true);

        public override GameActionMark GetHiddenGameActionMark() => GetGameActionMark();

        public override bool DoesSeeTrigger(FightActor fighter) => true;

        public override bool CanTrigger(FightActor actor) => true;
    }
}