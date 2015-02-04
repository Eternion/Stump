using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Damage;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Triggers
{
    public class Wall : MarkTrigger
    {
        public Wall(short id, FightActor caster, Spell castedSpell, EffectDice originEffect, Cell centerCell, WallsBinding binding, params MarkShape[] shapes)
            : base(id, caster, castedSpell, originEffect, centerCell, shapes)
        {
            WallBinding = binding;
        }

        public WallsBinding WallBinding
        {
            get;
            private set;
        }

        public override GameActionMarkTypeEnum Type
        {
            get { return GameActionMarkTypeEnum.WALL; }
        }

        public override TriggerType TriggerType
        {
            get { return TriggerType.MOVE | TriggerType.TURN_BEGIN | TriggerType.TURN_END; }
        }

        public SummonedBomb[] Bombs
        {
            get { return new[] { WallBinding.Bomb1, WallBinding.Bomb2 }; }
        }

        public override void Trigger(FightActor trigger)
        {
            if (!IsAffected(trigger))
                return;

            NotifyTriggered(trigger, CastedSpell);

            var handler = SpellManager.Instance.GetSpellCastHandler(Caster, CastedSpell, trigger.Cell, false);
            handler.MarkTrigger = this;
            handler.Initialize();
            var bonus = Bombs.Sum(x => x.DamageBonusPercent);

            foreach (var effect in handler.GetEffectHandlers().OfType<DirectDamage>())
            {
                effect.Efficiency = 1 + bonus/100d;
            }

            handler.Execute();
        }

        public override GameActionMark GetGameActionMark()
        {
            return new GameActionMark(Caster.Id, CastedSpell.Id, Id, (sbyte) Type, Shapes.Select(entry => entry.GetGameActionMarkedCell()));
        }

        public override GameActionMark GetHiddenGameActionMark()
        {
            return GetGameActionMark();
        }

        public override bool DoesSeeTrigger(FightActor fighter)
        {
            return true;
        }

        public override bool DecrementDuration()
        {
            return false;
        }

        public override bool IsAffected(FightActor actor)
        {
            if (!(actor is SummonedBomb))
                return true;

            var bomb = Bombs.FirstOrDefault();
            var triggerBomb = (actor as SummonedBomb);

            return bomb == null || bomb.MonsterBombTemplate != triggerBomb.MonsterBombTemplate || !bomb.IsFriendlyWith(triggerBomb);
        }
    }
}