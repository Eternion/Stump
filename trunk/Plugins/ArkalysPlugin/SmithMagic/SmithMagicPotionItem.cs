using System;
using System.Collections.Generic;
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
        [Initialization(typeof(ItemManager), Silent=true)]
        public static void Initialize()
        {
            ItemManager.Instance.AddItemConstructor(typeof(SmithMagicPotionItem));
        }

        private static Tuple<EffectsEnum, int[]>[] m_potions =
        {
            Tuple.Create(EffectsEnum.Effect_DamageEarth, new []{1338, 1340, 1348}),
            Tuple.Create(EffectsEnum.Effect_DamageFire, new []{1333, 1343, 1345}),
            Tuple.Create(EffectsEnum.Effect_DamageWater, new []{1335, 1341, 1346}),
            Tuple.Create(EffectsEnum.Effect_DamageAir, new []{1337, 1342, 1347}),

        };

        private static double[] m_potionsBoosts = {0.5, 0.6, 0.8};

        public SmithMagicPotionItem(Character owner, PlayerItemRecord record) : base(owner, record)
        {
        }

        public override uint UseItem(uint amount = 1, Cell targetCell = null, Character target = null)
        {
            var weapon = Owner.Inventory.TryGetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON);

            if (weapon == null || weapon.Effects.All(x => x.EffectId != EffectsEnum.Effect_DamageNeutral))
            {
                Owner.SendServerMessage("Vous devez vous équipper d'une arme de dégats neutre pour la forgemager");
                return 0;
            }

            var tuple = m_potions.FirstOrDefault(x => x.Item2.Contains(Template.Id));

            if (tuple != null)
            {
                var boost = m_potionsBoosts[Array.IndexOf(tuple.Item2, Template.Id)];
                var effect = weapon.Effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_DamageNeutral);

                effect.EffectId = tuple.Item1;
                if (effect is EffectInteger)
                    (effect as EffectInteger).Value = (short)((effect as EffectInteger).Value * boost);
                else if (effect is EffectMinMax)
                {
                    (effect as EffectMinMax).ValueMin = (short)((effect as EffectMinMax).ValueMin * boost);
                    (effect as EffectMinMax).ValueMax = (short)((effect as EffectMinMax).ValueMax * boost);
                }

                Owner.Inventory.RefreshItem(weapon);

                return 1;
            }

            return 1;
        }
    }
}