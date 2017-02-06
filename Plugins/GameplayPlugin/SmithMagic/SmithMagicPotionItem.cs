using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Items.Player.Custom;
using System;
using System.Linq;

namespace GameplayPlugin.SmithMagic
{
    [ItemType(ItemTypeEnum.POTION_DE_FORGEMAGIE)]
    public class SmithMagicPotionItem : BasePlayerItem
    {
        private static readonly Tuple<EffectsEnum, int[]>[] m_potions =
        {
            Tuple.Create(EffectsEnum.Effect_DamageEarth, new[] {1338, 1340, 1348}),
            Tuple.Create(EffectsEnum.Effect_DamageFire, new[] {1333, 1343, 1345}),
            Tuple.Create(EffectsEnum.Effect_DamageWater, new[] {1335, 1341, 1346}),
            Tuple.Create(EffectsEnum.Effect_DamageAir, new[] {1337, 1342, 1347})
        };

        private static readonly double[] m_potionsBoosts = { 0.5, 0.6, 0.8 };

        public SmithMagicPotionItem(Character owner, PlayerItemRecord record) : base(owner, record)
        {
        }

        [Initialization(typeof(ItemManager), Silent = true)]
        public static void Initialize()
        {
            ItemManager.Instance.AddItemTypeConstructor(typeof(SmithMagicPotionItem));
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            var weapon = Owner.Inventory.TryGetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON);

            if (weapon == null || weapon.Effects.All(x => x.EffectId != EffectsEnum.Effect_DamageNeutral))
            {
                Owner.SendServerMessage("Vous devez vous équipper d'une arme de dégats neutre pour la forgemager");
                return 0;
            }

            var tuple = m_potions.FirstOrDefault(x => x.Item2.Contains(Template.Id));

            if (tuple == null)
                return 1;

            var boost = m_potionsBoosts[Array.IndexOf(tuple.Item2, Template.Id)];
            var effect = weapon.Effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_DamageNeutral);
            weapon.Effects.Remove(effect);

            if (effect is EffectDice effectDice)
            {
                var newEffect = new EffectDice(effectDice) { EffectId = tuple.Item1 };
                newEffect.DiceFace = (short)(newEffect.DiceFace * boost);
                newEffect.DiceNum = (short)Math.Ceiling((newEffect.DiceNum * boost));

                weapon.Effects.Add(newEffect);
            }
            else
            {
                if (effect is EffectInteger effectInteger)
                {
                    var newEffect = new EffectInteger(effectInteger) { EffectId = tuple.Item1 };
                    newEffect.Value = (short)(newEffect.Value * boost);

                    weapon.Effects.Add(newEffect);
                }
                else
                {
                    if (effect is EffectMinMax effectMinMax)
                    {
                        var newEffect = new EffectMinMax(effectMinMax) { EffectId = tuple.Item1 };
                        newEffect.ValueMin = (short)(newEffect.ValueMin * boost);
                        newEffect.ValueMax = (short)(newEffect.ValueMax * boost);

                        weapon.Effects.Add(newEffect);
                    }
                }
            }

            weapon.Invalidate();
            Owner.Inventory.RefreshItem(weapon);
            weapon.OnObjectModified();

            return 1;
        }
    }
}