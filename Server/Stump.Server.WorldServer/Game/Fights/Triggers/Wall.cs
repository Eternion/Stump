using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Summon;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Triggers
{
    public class Wall : MarkTrigger
    {
        private List<SummonedBomb> m_bombs = new List<SummonedBomb>();

        public Wall(short id, FightActor caster, Spell castedSpell, EffectDice originEffect, Cell centerCell, params MarkShape[] shapes)
            : base(id, caster, castedSpell, originEffect, centerCell, shapes)
        {
        }

        public override GameActionMarkTypeEnum Type
        {
            get { return GameActionMarkTypeEnum.WALL; }
        }

        public override TriggerType TriggerType
        {
            get { return TriggerType.MOVE | TriggerType.TURN_BEGIN | TriggerType.TURN_END; }
        }

        public ReadOnlyCollection<SummonedBomb> Bombs
        {
            get { return m_bombs.AsReadOnly(); }
        }

        public void BindToBomb(SummonedBomb bomb)
        {
            m_bombs.Add(bomb);
        }

        public override void Trigger(FightActor trigger)
        {
            NotifyTriggered(trigger, CastedSpell);

            var handler = SpellManager.Instance.GetSpellCastHandler(Caster, CastedSpell, trigger.Cell, false);
            handler.MarkTrigger = this;
            handler.Initialize();
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
    }
}