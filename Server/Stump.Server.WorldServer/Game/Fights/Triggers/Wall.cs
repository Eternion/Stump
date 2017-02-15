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
        public Wall(short id, FightActor caster, Spell castedSpell, EffectDice originEffect, Cell centerCell, WallsBinding binding, MarkShape shape)
            : base(id, caster, castedSpell, originEffect, centerCell, shape)
        {
            WallBinding = binding;
        }

        public WallsBinding WallBinding
        {
            get;
        }

        public override GameActionMarkTypeEnum Type => GameActionMarkTypeEnum.WALL;

        public override TriggerType TriggerType => TriggerType.MOVE | TriggerType.OnTurnBegin | TriggerType.OnTurnEnd;

        public SummonedBomb[] Bombs => new[] { WallBinding.Bomb1, WallBinding.Bomb2 };

        public override void Trigger(FightActor trigger, Cell triggerCell)
        {
            if (!CanTrigger(trigger))
                return;

            NotifyTriggered(trigger, CastedSpell);

            var handler = SpellManager.Instance.GetSpellCastHandler(Caster, CastedSpell, trigger.Cell, false);
            handler.MarkTrigger = this;
            handler.Initialize();
            var boundedBombs = Bombs.First().GetBombsBoundedWith();
            var bonus = boundedBombs.Sum(x => x.DamageBonusPercent);

            foreach (var effect in handler.GetEffectHandlers().OfType<DirectDamage>())
            {
                effect.Efficiency = 1 + bonus / 100d;
            }

            handler.Execute();

            if (Fight.FighterPlaying != trigger)
                Caster.SpellHistory.RegisterCastedSpell(CastedSpell.CurrentSpellLevel, trigger);
        }

        public override GameActionMark GetGameActionMark()
            => new GameActionMark(Caster.Id, (sbyte)Caster.Team.Id, CastedSpell.Template.Id, (sbyte)CastedSpell.CurrentLevel, Id,
                (sbyte)Type, CenterCell.Id, Shape.GetGameActionMarkedCells(), true);

        public override GameActionMark GetHiddenGameActionMark() => GetGameActionMark();

        public override bool DoesSeeTrigger(FightActor fighter) => true;

        public override bool DecrementDuration() => false;

        public override bool CanTrigger(FightActor actor)
        {
            var bomb = Bombs.FirstOrDefault();

            if (bomb == null)
                return true;

            if (actor is SummonedBomb triggerBomb && bomb.IsFriendlyWith(triggerBomb) && bomb.MonsterBombTemplate == triggerBomb.MonsterBombTemplate
                || actor.HasState((int)SpellStatesEnum.KABOOM_92) && bomb.IsFriendlyWith(actor))
                return false;
            else if (actor.HasState((int)SpellStatesEnum.KABOOM_92) && bomb.IsFriendlyWith(actor))
                return false;
            else if (Fight.FighterPlaying != actor && Caster.SpellHistory.GetEntries(x => x.Target == actor &&
                x.CastRound == Fight.TimeLine.RoundNumber && x.Spell.SpellId == CastedSpell.Id).Any())
                return false;

            return true;
        }
    }
}