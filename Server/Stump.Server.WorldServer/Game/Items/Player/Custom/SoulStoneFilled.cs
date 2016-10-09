using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Effects.Instances;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    [ItemType(ItemTypeEnum.FULL_SOUL_STONE)]
    public class SoulStoneFilled : BasePlayerItem
    {
        public SoulStoneFilled(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override bool IsUsable()
        {
            return true;
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {


            return base.UseItem(amount, targetCell, target);
        }

        public void SetMonsterGroup(IEnumerable<Monster> group)
        {
            foreach (var monster in group)
            {
                Effects.Add(new EffectDice((short) EffectsEnum.Effect_SoulStoneSummon, (short) monster.Template.Id, (short) monster.Grade.GradeId, 0, new EffectBase()));
            }

            Invalidate();
            Owner.Inventory.RefreshItem(this);
        }

        public void PopulateMonsterGroup(MonsterGroup group)
        {
            foreach (var effect in Effects.OfType<EffectDice>().Where(x => x.EffectId == EffectsEnum.Effect_SoulStoneSummon))
            {
                group.AddMonster(new Monster(MonsterManager.Instance.GetMonsterGrade(effect.Value, effect.DiceNum), group));
            }
        }
    }
}