using System;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Items;

namespace ArkalysPlugin
{
    public static class OrbsManager
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Variable]
        public static short OrbItemTemplateId = 20000;

        [Variable]
        public static short OrbsExchangeRate = 1000;

        public static ItemTemplate OrbItemTemplate;

        [Variable]
        public static double FormulasCoefficient = 0.0022;

        [Variable]
        public static double FormulasExponent = 2.18;

        [Variable]
        public static double BossFactor = 8;



        [Initialization(typeof(ItemManager))]
        public static void Initialize()
        {
            OrbItemTemplate = ItemManager.Instance.TryGetTemplate(OrbItemTemplateId);

            if (OrbItemTemplate == null)
                logger.Error("Orb item template {0} doesn't exist in database !", OrbItemTemplateId);
            else
                FightManager.Instance.EntityAdded += OnFightCreated;
        }

        private static void OnFightCreated(IFight fight)
        {
            if (fight is FightPvM)
            {
                fight.ResultGenerated += OnResultGenerated;
            }
        }

        private static void OnResultGenerated(IFight fight)
        {
            var monsters = fight.GetAllFighters<MonsterFighter>(entry => entry.IsDead()).ToList();
            var players = fight.GetAllFighters<CharacterFighter>().ToList();

            var challengeBonus = fight.GetChallengeBonus();

            var totalOrbs = (uint)monsters.Sum(x => GetMonsterDroppedOrbs(x));
            totalOrbs += (uint)Math.Truncate(totalOrbs * (challengeBonus / 100d));

            foreach (var player in players)
            {
                var teamPP = player.Team.GetAllFighters<CharacterFighter>().Sum(entry => entry.Stats[PlayerFields.Prospecting].Total);
                var orbs = (uint)(((double)player.Stats[PlayerFields.Prospecting].Total / teamPP) * totalOrbs);

                if (orbs > 0)
                    player.Loot.AddItem(new DroppedItem(OrbItemTemplateId, orbs));
            }

            if (fight.Map.TaxCollector == null)
                return;

            var item = fight.Map.TaxCollector.Bag.TryGetItem(OrbItemTemplate);
            var limit = fight.Map.TaxCollector.Guild.TaxCollectorPods;

            if (item != null)
            {
                limit -= (int)item.Stack;
            }

            var collectorOrbs = (uint)(((double)fight.Map.TaxCollector.Guild.TaxCollectorProspecting /
                                players.Sum(entry => entry.Stats[PlayerFields.Prospecting].Total)) * totalOrbs * 0.05);

            if (collectorOrbs > limit)
                collectorOrbs = (uint)limit;

            fight.TaxCollectorLoot.AddItem(new DroppedItem(OrbItemTemplateId, collectorOrbs));
        }

        private static uint GetMonsterDroppedOrbs(MonsterFighter monster)
        {
            if (monster.Monster.Grade.GradeXp == 0)
                return 0;


             return (uint)Math.Floor(FormulasCoefficient * (monster.Monster.Template.IsBoss ? BossFactor : 1) * Math.Pow(monster.Level, FormulasExponent)) +
                (uint)Math.Floor(Math.Pow(Math.Log(2 * monster.Level), 0.6));

            // formulas based on xp
            //return (uint) Math.Floor(FormulasCoefficient*(monster.Monster.Template.IsBoss ? BossFactor : 1)*Math.Pow(monster.Monster.Grade.GradeXp, FormulasExponent));
        }
    }
}