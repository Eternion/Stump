using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights;
using Monster = Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters.Monster;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    [ItemType(ItemTypeEnum.PIERRE_D_ÂME_PLEINE)]
    public class SoulStoneFilled : BasePlayerItem
    {
        public static int[] FIGHT_MAPS = {
            4981250,
            4981762,
            4981761,
            4981760,
            4981248,
            4981249,
            4980738,
            4980737,
            4980736,
            11796480,
            11796481,
            11796482,
            11796992,
            11796993,
            11796994,
            11797504,
            11797505,
            11797506
        }; // Brakmar and bonta arenas

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
            if (!FIGHT_MAPS.Contains(Owner.Map.Id))
                return 0;

            var group = Owner.Map.SpawnMonsterGroup(GetMonsterGroup(), Owner.Position.Clone());
            group.AuthorizedAgressor = Owner;

            group.EnterFight += OnEnterFight;

            return 1;
        }

        private void OnEnterFight(MonsterGroup group, Character fighter)
        {
            group.EnterFight -= OnEnterFight;

            var fightPvM = @group.Fight as FightPvM;
            if (fightPvM != null)
                fightPvM.IsPvMArenaFight = true;
        }

        public void SetMonsterGroup(IEnumerable<Monster> group)
        {
            Effects.Clear();
            foreach (var monster in group)
            {
                Effects.Add(new EffectDice((short) EffectsEnum.Effect_SoulStoneSummon, (short) monster.Template.Id, (short) monster.Grade.GradeId, 0, new EffectBase()));
            }

            Invalidate();
        }

        public IEnumerable<MonsterGrade> GetMonsterGroup()
        {
            return Effects.OfType<EffectDice>().Where(x => x.EffectId == EffectsEnum.Effect_SoulStoneSummon).
                Select(effect => MonsterManager.Instance.GetMonsterGrade(effect.Value, effect.DiceNum));
        }

        protected override ObjectItem BuildObjectItem()
        {
            return new ObjectItem(
                (byte)Position,
                (short)Template.Id,
                Effects.Where(entry => !entry.Hidden).Select(entry => entry.GetObjectEffect()),
                Guid,
                (int)(Stack));
        }
    }
}