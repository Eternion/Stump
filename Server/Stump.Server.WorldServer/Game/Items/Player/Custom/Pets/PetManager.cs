using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Effects;
using Stump.Server.WorldServer.Database.Items.Pets;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Effects.Instances;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    public class PetManager : DataManager<PetManager>
    {
        public const double DEFAULT_STAT_POWER = 1;
        public static readonly Dictionary<CharacteristicEnum, double> POWER_PER_STAT = new Dictionary<CharacteristicEnum, double>
        {
            {CharacteristicEnum.AP, 100},
            {CharacteristicEnum.MP, 90},
            {CharacteristicEnum.RANGE, 51},
            {CharacteristicEnum.REFLECT, 30},
            {CharacteristicEnum.SUMMONS, 30},
            {CharacteristicEnum.CRITICAL_HITS, 10},
            {CharacteristicEnum.HEALS, 10},
            {CharacteristicEnum.DAMAGE, 20},
            {CharacteristicEnum.TRAPS_BONUS_FIXED, 15},
            {CharacteristicEnum.AP_REDUCTION, 7},
            {CharacteristicEnum.MP_REDUCTION, 7},
            {CharacteristicEnum.AP_LOSS_RES, 7},
            {CharacteristicEnum.MP_LOSS_RES, 7},
            {CharacteristicEnum.FIRE_REDUCTION_PERCENT, 4},
            {CharacteristicEnum.WATER_REDUCTION_PERCENT, 4},
            {CharacteristicEnum.AIR_REDUCTION_PERCENT, 4},
            {CharacteristicEnum.EARTH_REDUCTION_PERCENT, 4},
            {CharacteristicEnum.NEUTRAL_REDUCTION_PERCENT, 4},
            {CharacteristicEnum.NEUTRAL_DAMAGE_FIXED, 5},
            {CharacteristicEnum.AIR_DAMAGE_FIXED, 5},
            {CharacteristicEnum.FIRE_DAMAGE_FIXED, 5},
            {CharacteristicEnum.EARTH_DAMAGE_FIXED, 5},
            {CharacteristicEnum.WATER_DAMAGE_FIXED, 5},
            {CharacteristicEnum.CRITICAL_DAMAGE, 5},
            {CharacteristicEnum.PUSHBACK_DAMAGE_FIXED, 5},
            {CharacteristicEnum.LOCK, 4},
            {CharacteristicEnum.DODGE, 4},
            {CharacteristicEnum.WISDOM, 3},
            {CharacteristicEnum.PROSPECTING, 3},
            {CharacteristicEnum.POWER, 2},
            {CharacteristicEnum.TRAPS_BONUS_PERCENT, 2},
            {CharacteristicEnum.FIRE_REDUCTION_FIXED, 2},
            {CharacteristicEnum.EARTH_REDUCTION_FIXED, 2},
            {CharacteristicEnum.WATER_REDUCTION_FIXED, 2},
            {CharacteristicEnum.AIR_REDUCTION_FIXED, 2},
            {CharacteristicEnum.NEUTRAL_REDUCTION_FIXED, 2},
            {CharacteristicEnum.PUSHBACK_REDUCTION, 2},
            {CharacteristicEnum.CRITICAL_REDUCTION_FIXED, 2},
            {CharacteristicEnum.STRENGTH, 1},
            {CharacteristicEnum.CHANCE, 1},
            {CharacteristicEnum.INTELLIGENCE, 1},
            {CharacteristicEnum.AGILITY, 1},
            {CharacteristicEnum.WEIGHT, 0.25},
            {CharacteristicEnum.VITALITY, 1},
            {CharacteristicEnum.INITIATIVE, 0.1},
        };

        private Dictionary<int, PetTemplate> m_pets;
        public ReadOnlyDictionary<int, PetTemplate> Pets => new ReadOnlyDictionary<int, PetTemplate>(m_pets);

        [Initialization(typeof(ItemManager))]
        public override void Initialize()
        {
            m_pets = Database.Query<PetTemplate, PetFoodRecord, PetTemplate>(new PetTemplateRelator().Map, PetTemplateRelator.FetchQuery).ToDictionary(x => x.Id);
        }

        public PetTemplate GetPetTemplate(int id)
        {
            PetTemplate template;
            return m_pets.TryGetValue(id, out template) ? template : null;
        }

        public double GetEffectMinPower(EffectDice effect)
        {
            var min = effect.DiceFace < effect.DiceNum && effect.DiceFace != 0 ? effect.DiceFace : effect.DiceNum;

            return (effect.Template.BonusType < 0 ? -1 : 1) * min * GetEffectPower(effect.Template);
        }

        public double GetEffectMaxPower(EffectDice effect)
        {
            int max;
            if (effect.DiceFace == 0 || effect.DiceNum == 0)
                max = effect.DiceFace == 0 ? effect.DiceNum : effect.DiceFace;
            else
                max = effect.DiceFace < effect.DiceNum ? effect.DiceNum : effect.DiceFace;

            return (effect.Template.BonusType < 0 ? -1 : 1) * max * GetEffectPower(effect.Template);
        }

        public double GetEffectPower(EffectInteger effect)
        {
            return (effect.Template.BonusType < 0 ? -1 : 1) * GetEffectPower(effect.Template) * effect.Value;
        }

        public double GetEffectBasePower(EffectBase effect)
        {
            return (effect.Template.BonusType < 0 ? -1 : 1) * GetEffectPower(effect.Template);
        }
        public double GetItemPower(IItem item)
        {
            return item.Effects.OfType<EffectInteger>().Sum(x => GetEffectPower(x));
        }

        public double GetItemMinPower(IItem item)
        {
            return item.Template.Effects.OfType<EffectDice>().Sum(x => GetEffectMinPower(x));
        }

        public double GetItemMaxPower(IItem item)
        {
            return item.Template.Effects.OfType<EffectDice>().Sum(x => GetEffectMaxPower(x));
        }

        public double GetEffectPower(EffectTemplate effect)
        {
            return POWER_PER_STAT.ContainsKey((CharacteristicEnum)effect.Characteristic) ? POWER_PER_STAT[(CharacteristicEnum)effect.Characteristic] : DEFAULT_STAT_POWER;
        }
    }
}