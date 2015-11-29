using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Fights.Triggers
{
    public abstract class MarkTrigger
    {
        public event Action<MarkTrigger, FightActor, Spell> Triggered;

        protected void NotifyTriggered(FightActor target, Spell triggeredSpell)
        {
            var handler = Triggered;
            if (handler != null)
                handler(this, target, triggeredSpell);
        }

        public event Action<MarkTrigger> Removed;

        public void NotifyRemoved()
        {
            var handler = Removed;
            if (handler != null)
                handler(this);
        }

        protected MarkTrigger(short id, FightActor caster, Spell castedSpell, EffectDice originEffect, Cell centerCell, params MarkShape[] shapes)
        {
            Id = id;
            Caster = caster;
            CastedSpell = castedSpell;
            OriginEffect = originEffect;
            CenterCell = centerCell;
            Shapes = shapes;
        }

        public short Id
        {
            get;
            private set;
        }

        public FightActor Caster
        {
            get;
            private set;
        }

        public Spell CastedSpell
        {
            get;
            private set;
        }

        public EffectDice OriginEffect
        {
            get;
            set;
        }

        public Cell CenterCell
        {
            get;
            private set;
        }

        public IFight Fight
        {
            get { return Caster.Fight; }
        }

        public MarkShape[] Shapes
        {
            get;
            private set;
        }

        public abstract GameActionMarkTypeEnum Type
        {
            get;
        }

        public abstract TriggerType TriggerType
        {
            get;
        }

        public virtual bool StopMovement => true;

        public bool ContainsCell(Cell cell)
        {
            return Shapes.Any(entry => entry.GetCells().Contains(cell));
        }

        public IEnumerable<Cell> GetCells()
        {
            return Shapes.SelectMany(entry => entry.GetCells());
        }

        public virtual void Remove()
        {
            Fight.RemoveTrigger(this);
        }

        public abstract void Trigger(FightActor trigger);
        public abstract GameActionMark GetGameActionMark();
        public abstract GameActionMark GetHiddenGameActionMark();
        public abstract bool DoesSeeTrigger(FightActor fighter);
        public abstract bool DecrementDuration();
        public abstract bool CanTrigger(FightActor actor);
    }
}