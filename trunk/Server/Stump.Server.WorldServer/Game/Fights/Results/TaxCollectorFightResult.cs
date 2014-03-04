using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Items.TaxCollector;
using FightLoot = Stump.Server.WorldServer.Game.Fights.Results.FightLoot;

namespace Stump.Server.WorldServer.Game.Fights.Results
{
    public class TaxCollectorFightResult : IFightResult, IExperienceResult
    {
        public TaxCollectorFightResult(TaxCollectorNpc taxCollector, Fight fight)
        {
            TaxCollector = taxCollector;
            Fight = fight;
            Loot = new FightLoot();
        }

        public TaxCollectorNpc TaxCollector
        {
            get;
            private set;
        }

        public Fight Fight
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

        public int Prospecting
        {
            get
            {
                return TaxCollector.Guild.TaxCollectorProspecting;
            }
        }

        public int Wisdom
        {
            get
            {
                return TaxCollector.Guild.TaxCollectorWisdom;
            }
        }

        public int Level
        {
            get { return TaxCollector.Guild.Level; }
        }

        public bool CanLoot(FightTeam team)
        {
            return team is FightPlayerTeam;
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
            foreach (DroppedItem drop in Loot.Items.Values)
            {
                ItemTemplate template = ItemManager.Instance.TryGetTemplate(drop.ItemId);

                if (template.Effects.Count > 0)
                    for (int i = 0; i < drop.Amount; i++)
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
            TaxCollector.GatheredExperience += Experience;
            TaxCollector.GatheredKamas += Loot.Kamas;
        }

        public void AddEarnedExperience(int experience)
        {
            Experience += (int) (experience * 0.1d); // own only a percent
        }
    }
}