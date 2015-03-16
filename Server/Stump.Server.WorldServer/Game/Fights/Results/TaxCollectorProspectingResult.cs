using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Game.Fights.Results
{
    public class TaxCollectorProspectingResult : IFightResult, IExperienceResult
    {
        public TaxCollectorProspectingResult(TaxCollectorNpc taxCollector, IFight fight)
        {
            TaxCollector = taxCollector;
            Fight = fight;
            Loot = fight.TaxCollectorLoot;
        }

        public TaxCollectorNpc TaxCollector
        {
            get;
            private set;
        }

        public IFight Fight
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

        public bool HasLeft
        {
            get { return false; }
        }

        public int Id
        {
            get
            {
                return TaxCollector.GlobalId;
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
            foreach (var drop in Loot.Items.Values)
            {
                var template = ItemManager.Instance.TryGetTemplate(drop.ItemId);

                if (template.Effects.Count > 0)
                    for (var i = 0; i < drop.Amount; i++)
                    {
                        var item = ItemManager.Instance.CreateTaxCollectorItem(TaxCollector, drop.ItemId, (int)drop.Amount);
                        TaxCollector.Bag.AddItem(item, false);
                    }
                else
                {
                    var item = ItemManager.Instance.CreateTaxCollectorItem(TaxCollector, drop.ItemId, (int)drop.Amount);
                    TaxCollector.Bag.AddItem(item, false);
                }
            }

            TaxCollector.GatheredExperience += Experience;
            TaxCollector.GatheredKamas += Loot.Kamas;
        }

        public void AddEarnedExperience(int experience)
        {
            if (TaxCollector.GatheredExperience > TaxCollectorNpc.MaxGatheredXPTotal)
                return;

            var XP = (int) (experience * 0.1d); // own only a percent

            Experience += XP > TaxCollectorNpc.MaxGatheredXPFight ? TaxCollectorNpc.MaxGatheredXPFight : XP;
        }
    }
}