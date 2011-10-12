using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Effects.Instances;
using Stump.Server.WorldServer.Worlds.Maps.Cells.Shapes;
using Stump.Server.WorldServer.Worlds.Spells;

namespace Stump.Server.WorldServer.Worlds.Fights.Triggers
{
    public abstract class MarkTrigger
    {
        public event Action<MarkTrigger, FightActor, Spell> Triggered;

        protected void NotifyTriggered(FightActor target, Spell triggeredSpell)
        {
            Action<MarkTrigger, FightActor, Spell> handler = Triggered;
            if (handler != null)
                handler(this, target, triggeredSpell);
        }

        protected MarkTrigger(short id, FightActor caster, Spell castedSpell, params MarkShape[] shapes)
        {
            Id = id;
            Caster = caster;
            CastedSpell = castedSpell;
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

        public Fight Fight
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
    }
}