using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types.Extensions;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Fights.Buffs.Customs;
using Stump.Server.WorldServer.Game.Spells;

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
                var look = actor.Look.Copy();
                look.bonesId = Dice.Value;

                var buff = new SkinBuff(actor.PopNextBuffId(), actor, Caster, Dice, look, Spell, true);
                actor.AddAndApplyBuff(buff);
            }

            return true;
        }
    }
}