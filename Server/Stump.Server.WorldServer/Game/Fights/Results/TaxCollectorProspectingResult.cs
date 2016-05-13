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
        }

        public IFight Fight
        {
            get;
        }

        public bool Alive => true;

        public bool HasLeft => false;

        public int Id => TaxCollector.GlobalId;

        public int Prospecting => TaxCollector.Guild.TaxCollectorProspecting;

        public int Wisdom => TaxCollector.Guild.TaxCollectorWisdom;

        public int Level => TaxCollector.Guild.Level;

        public bool CanLoot(FightTeam team) => team is FightPlayerTeam;

        public FightLoot Loot
        {
            get;
        }

        public int Experience
        {
            get;
            set;
        }

        public FightOutcomeEnum Outcome => FightOutcomeEnum.RESULT_TAX;

        public FightResultListEntry GetFightResultListEntry() => new FightResultTaxCollectorListEntry((short)Outcome, 0, Loot.GetFightLoot(), Id, Alive,
                                    TaxCollector.Guild.Level, TaxCollector.Guild.GetBasicGuildInformations(), Experience);

        public void Apply()
        {
            foreach (var drop in Loot.Items.Values)
            {
                var template = ItemManager.Instance.TryGetTemplate(drop.ItemId);

                if (template.Effects.Count > 0)
                    for (var i = 0; i < drop.Amount; i++)
                    {
                        var item = ItemManager.Instance.CreateTaxCollectorItem(TaxCollector, drop.ItemId, (int)drop.Amount);
                        TaxCollector.Bag.AddItem(item);
                    }
                else
                {
                    var item = ItemManager.Instance.CreateTaxCollectorItem(TaxCollector, drop.ItemId, (int)drop.Amount);
                    TaxCollector.Bag.AddItem(item);
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