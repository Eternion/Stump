using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Damage;
using Stump.Server.WorldServer.Game.Fights;

namespace Stump.Server.WorldServer.Game.Spells.Casts
{
    [SpellCastHandler(2822)]
    [SpellCastHandler(2830)]
    [SpellCastHandler(2845)]
    public class BombExplodSpellCastHandler : DefaultSpellCastHandler
    {
        public BombExplodSpellCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical) : base(caster, spell, targetedCell, critical)
        {
        }

        public int DamageBonus
        {
            get;
            set;
        }

        public bool ActiveBonus
        {
            get;
            set;
        }

        public override void Initialize()
        {
            var len = Spell.CurrentSpellLevel.Effects.Count;
            Handlers = Spell.CurrentSpellLevel.Effects.GetRange(0, ActiveBonus ? len : len - 1)
                            .Select(x => EffectManager.Instance.GetSpellEffectHandler(x, Caster, Spell, TargetedCell,Critical))
                            .ToArray();

            foreach (var handler in Handlers)
            {
                if (handler is DirectDamage)
                    handler.Efficiency = 1 + DamageBonus/100d;
            }
        }
    }
}