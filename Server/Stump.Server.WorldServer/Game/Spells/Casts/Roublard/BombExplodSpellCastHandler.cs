using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Buffs;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Damage;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Debuffs;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Roublard
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

        public FightActor Summoner
        {
            get;
            set;
        }

        public override void Initialize()
        {
            /*var len = Spell.CurrentSpellLevel.Effects.Count;
            Handlers = Spell.CurrentSpellLevel.Effects.GetRange(0, ActiveBonus ? len : len - 1)
                            .Select(x => EffectManager.Instance.GetSpellEffectHandler(x, Caster, Spell, TargetedCell, Critical))
                            .ToArray();*/

            Handlers = Spell.CurrentSpellLevel.Effects.Select(x => EffectManager.Instance.GetSpellEffectHandler(x, Summoner, Spell, TargetedCell, Critical)).ToArray();

            foreach (var handler in Handlers)
            {
                var affectedActors = handler.GetAffectedActors(x => !x.IsFriendlyWith(Caster) || !x.HasState((int)SpellStatesEnum.Kaboom));

                if (handler is DirectDamage)
                    handler.Efficiency = 1 + DamageBonus/100d;

                if (handler is APBuff || handler is MPBuff || handler is StatsBuff)
                    affectedActors = handler.GetAffectedActors(x => x != Caster && x.IsFriendlyWith(Caster) && x.HasState((int)SpellStatesEnum.Kaboom));
                    
                if (handler is APDebuffNonFix || handler is MPDebuffNonFix)
                    affectedActors = affectedActors.Where(x => x != Caster);

                if (handler is ReduceBuffDuration)
                {
                    if (handler.Dice.DiceNum == 1 && !Caster.HasState((int) SpellStatesEnum.Load))
                        affectedActors = new FightActor[0];

                    if (handler.Dice.DiceNum == 2 && !Caster.HasState((int)SpellStatesEnum.Unload))
                        affectedActors = new FightActor[0];

                    if (handler.Dice.DiceNum == 3 && !Caster.HasState((int)SpellStatesEnum.Overload))
                        affectedActors = new FightActor[0];
                }

                if (handler is Kill)
                    affectedActors = new[] { Caster };

                handler.SetAffectedActors(affectedActors);
            }
        }
    }
}