using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Game.Fights.Results
{
    public class TaxCollectorFightResult : IFightResult
    {
        public TaxCollectorFightResult(TaxCollectorNpc taxCollector)
        {
            TaxCollector = taxCollector;
            Loot = new FightLoot();
        }

        public TaxCollectorNpc TaxCollector
        {
            get;
            private set;
        }

        public bool Alive
        {
            get
            {
                return true;
            }
        }

        public int Id
        {
            get
            {
                return TaxCollector.Id;
            }
        }

        public FightLoot Loot
        {
            get;
            private set;
        }

        public int Experience
        {
            get;
            set;
        }

        public FightOutcomeEnum Outcome
        {
            get
            {
                return FightOutcomeEnum.RESULT_TAX;
            }
        }

        public FightResultListEntry GetFightResultListEntry()
        {
            return new FightResultTaxCollectorListEntry((short) Outcome, Loot.GetFightLoot(), Id, Alive,
                TaxCollector.Guild.Level, TaxCollector.Guild.GetBasicGuildInformations(), Experience);
        }

        public void Apply()
        {
            foreach (var drop in Loot.Items.Values)
            {
                var template = ItemManager.Instance.TryGetTemplate(drop.ItemId);

                if (template.Effects.Count > 0)
                    for (var i = 0; i < drop.Amount; i++)
                    {
                        var item = ItemManager.Instance.CreateTaxCollectorItem(TaxCollector, drop.ItemId, drop.Amount);
                        TaxCollector.Bag.AddItem(item);
                    }
                else
                {
                    var item = ItemManager.Instance.CreateTaxCollectorItem(TaxCollector, drop.ItemId, drop.Amount);
                    TaxCollector.Bag.AddItem(item);
                }
            }

            TaxCollector.Guild.AddXP(Experience);
        }
    }
}