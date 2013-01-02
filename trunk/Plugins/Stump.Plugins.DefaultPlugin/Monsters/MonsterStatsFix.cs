using System.Collections.Generic;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Plugins.DefaultPlugin.Monsters
{
    public class MonsterStatsFix
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        
        [Variable]
        public static readonly double BossBonusFactor = 1.8;

        [Variable]
        public static readonly double StatsFactor = 6.5;

        [Initialization(typeof(MonsterManager), Silent = true)]
        public static void ApplyFix()
        {
            logger.Debug("Apply monster stats fix");

            foreach (var grade in MonsterManager.Instance.GetMonsterGrades())
            {
                bool extraHp = grade.LifePoints / (double)grade.Level > 10;

                grade.TackleEvade = (short) ((int) (grade.Level / 10d)  * (extraHp ? 2 : 1));
                grade.TackleBlock = grade.TackleEvade;

                UpdateMonsterMainStats(grade);
            }
        }

        private static void UpdateMonsterMainStats(MonsterGrade monster)
        {
            var factor = monster.Template.IsBoss ? BossBonusFactor : 1;
            var points = monster.Level * StatsFactor * factor;

            var stats = GetMonsterMainStats(monster);

            if (stats.Length == 0)
            {
                if (!monster.Stats.ContainsKey(PlayerFields.DamageBonusPercent))
                    monster.Stats.Add(PlayerFields.DamageBonusPercent, (short)points);

                if (!monster.Stats.ContainsKey(PlayerFields.Initiative))
                    monster.Stats.Add(PlayerFields.Initiative, (short)points);
            }

            foreach (var field in stats)
            {
                switch (field)
                {
                    case PlayerFields.Strength:
                        monster.Strength += (short) (points/stats.Length);
                        break;
                    case PlayerFields.Intelligence:
                        monster.Intelligence += (short) (points/stats.Length);
                        break;
                    case PlayerFields.Agility:
                        monster.Agility += (short) (points/stats.Length);
                        break;
                    case PlayerFields.Chance:
                        monster.Chance += (short) (points/stats.Length);
                        break;
                }
            }
        }

        private static PlayerFields[] GetMonsterMainStats(MonsterGrade monster)
        {
            if (monster.Spells.Count == 0)
                return new PlayerFields[0];

            var stats = new List<PlayerFields>();
            foreach (var spell in monster.Spells)
            {
                var spellLevel = SpellManager.Instance.GetSpellLevel(spell.SpellId, spell.Level);

                if (spellLevel == null)
                    continue;

                foreach (var effect in spellLevel.Effects)
                {
                    switch (effect.EffectId)
                    {
                        case EffectsEnum.Effect_DamageNeutral:
                        case EffectsEnum.Effect_DamageEarth:
                            stats.Add(PlayerFields.Strength);
                            break;
                        case EffectsEnum.Effect_DamageAir:
                            stats.Add(PlayerFields.Agility);
                            break;
                        case EffectsEnum.Effect_DamageWater:
                            stats.Add(PlayerFields.Chance);
                            break;
                        case EffectsEnum.Effect_DamageFire:
                            stats.Add(PlayerFields.Intelligence);
                            break;
                    }
                }
            }

            return stats.Distinct().ToArray();
        }
    }
}