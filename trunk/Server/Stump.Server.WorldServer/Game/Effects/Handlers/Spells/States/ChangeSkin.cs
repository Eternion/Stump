using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Fights.Buffs.Customs;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.States
{
    [EffectHandler(EffectsEnum.Effect_ChangeAppearance)]
    [EffectHandler(EffectsEnum.Effect_ChangeAppearance_335)]
    public class ChangeSkin : SpellEffectHandler
    {
        public ChangeSkin(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (FightActor actor in GetAffectedActors())
            {
                var look = actor.Look.Clone();
                look.BonesID = Dice.Value;

                var buff = new SkinBuff(actor.PopNextBuffId(), actor, Caster, Dice, look, Spell, true);
                actor.AddAndApplyBuff(buff);
            }

            return true;
        }
    }
}