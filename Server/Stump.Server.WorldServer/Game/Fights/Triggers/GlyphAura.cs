using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Triggers
{
    public class GlyphAura : Glyph
    {
        private List<FightActor> m_actorsInside = new List<FightActor>();

        public GlyphAura(short id, FightActor caster, Spell castedSpell, EffectDice originEffect, Spell glyphSpell, Cell centerCell, SpellShapeEnum shape, byte minSize, byte size, Color color)
            : base(id, caster, castedSpell, originEffect, glyphSpell, centerCell, shape, minSize, size, color, false)
        {
        }

        public override TriggerType TriggerType => TriggerType.MOVE | TriggerType.CREATION;
        public override bool StopMovement => false;

        private void Enter(FightActor actor)
        {
            var handler = SpellManager.Instance.GetSpellCastHandler(Caster, GlyphSpell, actor.Cell, false);
            handler.MarkTrigger = this;
            handler.Initialize();
            foreach (var effectHandler in handler.GetEffectHandlers())
            {
                effectHandler.SetAffectedActors(new [] { actor });
            }
            handler.Execute();

            actor.PositionChanged += OnPositionChanged;
            m_actorsInside.Add(actor);
        }

        private void OnPositionChanged(ContextActor actor, ObjectPosition position)
        {
            var fighter = actor as FightActor;
            if (!CanTrigger(fighter) && !ContainsCell(actor.Cell))
                Leave(fighter);
        }

        private void Leave(FightActor actor)
        {
            var buffs = actor.GetBuffs(x => x.Caster == Caster && x.Spell.Id == GlyphSpell.Id).ToArray();
            foreach(var buff in buffs)
            {
                actor.RemoveBuff(buff);
            }

            actor.PositionChanged -= OnPositionChanged;
            m_actorsInside.Remove(actor);
        }

        public override void Trigger(FightActor trigger)
        {
            if (CanTrigger(trigger))
            {
                NotifyTriggered(trigger, GlyphSpell);
                Enter(trigger);
            }
        }

        public override bool CanTrigger(FightActor actor)
        {
            return !m_actorsInside.Contains(actor);
        }

        public override void Remove()
        {
            foreach (var actor in m_actorsInside.ToArray())
            {
                Leave(actor);
            }

            base.Remove();
        }
    }
}