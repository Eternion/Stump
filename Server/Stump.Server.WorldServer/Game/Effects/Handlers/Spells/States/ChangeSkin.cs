using System;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
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
            foreach (var actor in GetAffectedActors())
            {
                if (Dice.Value == 0)
                    continue;
                
                var look = actor.Look.Clone();
                var driverLook = look.SubLooks.FirstOrDefault(x => x.BindingCategory == SubEntityBindingPointCategoryEnum.HOOK_POINT_CATEGORY_MOUNT_DRIVER);
                short skinId = -1;

                switch (Dice.Value)
                {
                    case 1575: //Zobal - Pleutre
                        skinId = 1443;
                        break;
                    case 1576: //Zobal - Psychopathe
                        skinId = 1449;
                        break;
                }

                if (driverLook != null)
                {
                    if (skinId != -1)
                        driverLook.Look.AddSkin(skinId);

                    look.SetRiderLook(driverLook.Look);
                }
                else
                {
                    if (skinId != -1)
                        look.AddSkin(skinId);

                    look.BonesID = Dice.Value;
                }
                
                if (Dice.Value >= 0)
                {
                    var buff = new SkinBuff(actor.PopNextBuffId(), actor, Caster, Dice, look, Spell, true);
                    actor.AddAndApplyBuff(buff);
                }
                else
                {
                    var buff = actor.GetBuffs(x => x is SkinBuff && ((SkinBuff)x).Look.BonesID == Math.Abs(Dice.Value)).FirstOrDefault();
                    actor.RemoveAndDispellBuff(buff);
                }
            }

            return true;
        }
    }
}