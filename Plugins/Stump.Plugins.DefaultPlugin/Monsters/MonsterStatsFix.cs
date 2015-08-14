using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.AI.Fights.Spells;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Plugins.DefaultPlugin.Monsters
{
    public class MonsterStatsFix
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Variable] public static readonly double BossBonusFactor = 1.8;

        [Variable]
        public static readonly double StatsFactor = 7;

        private static readonly int[] m_thresholds =
        {
            100, 350, 600
        };

        [Initialization(typeof(MonsterManager), Silent = true)]
        public static void ApplyFix()
        {
            logger.Debug("Apply monster stats fix");

            foreach (var grade in MonsterManager.Instance.GetMonsterGrades())
            {
                if (UpdateMonsterMainStats(grade))
                    MonsterManager.Instance.Database.Update(grade);
            }
        }

        private static bool UpdateMonsterMainStats(MonsterGrade monster)
        {
            if (monster.Strength != 0 || monster.Agility != 0 || 
                monster.Chance != 0 || monster.Intelligence != 0 ||
                monster.Stats.Count != 0)
                return false;

            var extraHp = monster.LifePoints / (double)monster.Level > 10;
            var factor = monster.Template.IsBoss ? BossBonusFactor : 1;
            var points = monster.Level * StatsFactor * factor;
            var stats = GetMonsterMainStats(monster);


            if (stats.Length == 0)
            {
                if (!monster.Stats.ContainsKey(PlayerFields.DamageBonusPercent))
                    monster.Stats.Add(PlayerFields.DamageBonusPercent, (short)GetPointsByInvest((int)points));

                if (!monster.Stats.ContainsKey(PlayerFields.Initiative))
                    monster.Stats.Add(PlayerFields.Initiative, (short)GetPointsByInvest((int)points));
            }

            var total = (double)stats.Length;

            if (monster.Strength == 0)
                monster.Strength += (short)GetPointsByInvest((int)Math.Floor(points*(stats.Count(x => x == PlayerFields.Strength)/total)));
            if (monster.Agility == 0)
                monster.Agility += (short)GetPointsByInvest((int)Math.Floor(points * ( stats.Count(x => x == PlayerFields.Agility) / total )));
            if (monster.Chance == 0)
                monster.Chance += (short)GetPointsByInvest((int)Math.Floor(points * ( stats.Count(x => x == PlayerFields.Chance) / total )));
            if (monster.Intelligence == 0)
                monster.Intelligence += (short)GetPointsByInvest((int)Math.Floor(points * ( stats.Count(x => x == PlayerFields.Intelligence) / total )));
        
            monster.TackleEvade = (short) ((int) (monster.Level / 10d)  * (extraHp ? 2 : 1));
            monster.TackleBlock = monster.TackleEvade;

            return true;
        }

        private static PlayerFields[] GetMonsterMainStats(MonsterGrade monster)
        {
            if (monster.Spells.Count == 0)
                return new PlayerFields[0];

            var stats = new List<PlayerFields>();
            foreach (var spell in monster.Spells)
            {
                var spellLevel = SpellManager.Instance.GetSpellLevel(spell.Id, spell.CurrentLevel);

                if (spellLevel == null)
                    continue;

                foreach (var effect in spellLevel.Effects)
                {
                    var categories = SpellIdentifier.GetEffectCategories(effect.EffectId);
                    if (categories.HasFlag(SpellCategory.DamagesAir))
                            stats.Add(PlayerFields.Agility);
                    if (categories.HasFlag(SpellCategory.DamagesEarth) || 
                        categories.HasFlag(SpellCategory.DamagesNeutral))
                            stats.Add(PlayerFields.Strength);
                    if (categories.HasFlag(SpellCategory.DamagesWater))
                            stats.Add(PlayerFields.Chance);
                    if (categories.HasFlag(SpellCategory.DamagesFire) ||
                        categories.HasFlag(SpellCategory.Healing))
                            stats.Add(PlayerFields.Intelligence);
                 
                }
            }

            return stats.Distinct().ToArray();
        }

        private static int GetPointsByInvest(int invest)
        {
            int points = 0;
            int costPerPoint = 1;
            while (invest > 0)
            {
                if (costPerPoint > m_thresholds.Length)
                {
                    points += (int)Math.Floor(invest / (double)costPerPoint);
                    invest = 0;
                }
                else if (invest/(double)costPerPoint > m_thresholds[costPerPoint - 1])
                {
                    points += m_thresholds[costPerPoint - 1] - (costPerPoint > 1 ? m_thresholds[costPerPoint - 2] : 0);
                    invest -= m_thresholds[costPerPoint - 1]*costPerPoint;
                    costPerPoint++;
                }
                else
                {
                    points += (int)Math.Floor(invest / (double)costPerPoint);
                    invest = 0;
                }
            }

            return points;
        }
    }
}