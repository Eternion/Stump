using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Handlers.Characters;

namespace Stump.Server.WorldServer.Game.Actors.Fight.Customs
{
    public class LivingChest : SummonedMonster
    {
        public LivingChest(int id, FightTeam team, FightActor summoner, MonsterGrade template, Cell cell)
            : base(id, team, summoner, template, cell)
        {
        }

        public override bool HasResult => IsAlive();

        public override IFightResult GetFightResult(FightOutcomeEnum outcome)
        {
            return new LivingChestFightResult(this, outcome, new FightLoot());
        }
    }

    public class LivingChestFightResult : FightResult<LivingChest>
    {
        public LivingChestFightResult(LivingChest fighter, FightOutcomeEnum outcome, FightLoot loot)
            : base(fighter, outcome, loot)
        {
        }

        public override bool CanLoot(FightTeam team)
        {
            return Fighter.Team == team && Fighter.IsAlive();
        }

        public override void Apply()
        {
            var summoner = Fighter.Summoner as CharacterFighter;

            if (summoner == null)
                return;

            summoner.Character.Inventory.AddKamas(Loot.Kamas);

            foreach (var drop in Loot.Items.Values)
            {
                var template = ItemManager.Instance.TryGetTemplate(drop.ItemId);

                if (template.Effects.Count > 0)
                    for (var i = 0; i < drop.Amount; i++)
                    {
                        var item = ItemManager.Instance.CreatePlayerItem(summoner.Character, drop.ItemId, 1);
                        summoner.Character.Inventory.AddItem(item);
                    }
                else
                {
                    var item = ItemManager.Instance.CreatePlayerItem(summoner.Character, drop.ItemId, (int)drop.Amount);
                    summoner.Character.Inventory.AddItem(item);
                }
            }
        }
    }
}