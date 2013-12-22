using System;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Items.Player.Custom;

namespace ArkalysPlugin.SmithMagic
{
    [ItemType(ItemTypeEnum.SMITHMAGIC_POTION)]
    public class SmithMagicPotionItem : BasePlayerItem
    {
        private static readonly Tuple<EffectsEnum, int[]>[] m_potions =
        {
            Tuple.Create(EffectsEnum.Effect_DamageEarth, new[] {1338, 1340, 1348}),
            Tuple.Create(EffectsEnum.Effect_DamageFire, new[] {1333, 1343, 1345}),
            Tuple.Create(EffectsEnum.Effect_DamageWater, new[] {1335, 1341, 1346}),
            Tuple.Create(EffectsEnum.Effect_DamageAir, new[] {1337, 1342, 1347})
        };

        private static readonly double[] m_potionsBoosts = {0.5, 0.6, 0.8};

        public SmithMagicPotionItem(Character owner, PlayerItemRecord record) : base(owner, record)
        {
        }

        [Initialization(typeof (ItemManager), Silent = true)]
        public static void Initialize()
        {
            ItemManager.Instance.AddItemConstructor(typeof (SmithMagicPotionItem));
        }

        public override uint UseItem(uint amount = 1, Cell targetCell = null, Character target = null)
        {
            BasePlayerItem weapon = Owner.Inventory.TryGetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON);

            if (weapon == null || weapon.Effects.All(x => x.EffectId != EffectsEnum.Effect_DamageNeutral))
            {
                Owner.SendServerMessage("Vous devez vous équipper d'une arme de dégats neutre pour la forgemager");
                return 0;
            }

            Tuple<EffectsEnum, int[]> tuple = m_potions.FirstOrDefault(x => x.Item2.Contains(Template.Id));

            if (tuple == null)
                return 1;

            double boost = m_potionsBoosts[Array.IndexOf(tuple.Item2, Template.Id)];
            EffectBase effect = weapon.Effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_DamageNeutral);
            weapon.Effects.Remove(effect);

            if (effect is EffectDice)
            {
                var newEffect = new EffectDice((EffectDice) effect) {EffectId = tuple.Item1};
                newEffect.DiceFace = (short) (newEffect.DiceFace*boost);
                newEffect.DiceNum = (short) (newEffect.DiceNum*boost);
            }
            if (effect is EffectInteger)
            {
                var newEffect = new EffectInteger((EffectInteger) effect) {EffectId = tuple.Item1};
                newEffect.Value = (short) (newEffect.Value*boost);
            }
            else if (effect is EffectMinMax)
            {
                var newEffect = new EffectMinMax((EffectMinMax) effect) {EffectId = tuple.Item1};
                newEffect.ValueMin = (short) (newEffect.ValueMin*boost);
                newEffect.ValueMax = (short) (newEffect.ValueMax*boost);
            }

            weapon.Effects.Add(effect);
            Owner.Inventory.RefreshItem(weapon);
            weapon.OnObjectModified();

            return 1;
        }
    }
}