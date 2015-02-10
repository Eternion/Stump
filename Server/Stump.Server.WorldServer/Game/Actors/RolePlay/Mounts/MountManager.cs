using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Mounts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Maps.Paddocks;
using Stump.Server.WorldServer.Handlers.Mounts;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Mounts
{
    public class MountManager : DataManager<MountManager>
    {
        private Dictionary<int, MountTemplate> m_mountTemplates;

        [Initialization(InitializationPass.Sixth)]
        public override void Initialize()
        {
            m_mountTemplates = Database.Query<MountTemplate>(MountTemplateRelator.FetchQuery).ToDictionary(entry => entry.Id);
        }

        public MountTemplate[] GetTemplates()
        {
            return m_mountTemplates.Values.ToArray();
        }

        public MountTemplate GetTemplate(int id)
        {
            MountTemplate result;
            return !m_mountTemplates.TryGetValue(id, out result) ? null : result;
        }

        public MountTemplate GetTemplateByScrollId(int scrollId)
        {
            return m_mountTemplates.FirstOrDefault(x => x.Value.ScrollId == scrollId).Value;
        }

        private static short GetBonusByLevel(int finalBonus, int level)
        {
            return (short)Math.Floor((double)(finalBonus * level / 100));
        }

        public List<EffectInteger> GetMountEffects(Mount mount)
        {
            var effects = new List<EffectInteger>();

            switch (mount.TemplateId)
            {
                case 20:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddSummonLimit, GetBonusByLevel(2, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddInitiative, GetBonusByLevel(1000, mount.Level)));
                    break;
                case 33:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(50, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddInitiative, GetBonusByLevel(500, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddSummonLimit, GetBonusByLevel(1, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddWisdom, GetBonusByLevel(25, mount.Level)));
                    break;
                case 34:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddInitiative, GetBonusByLevel(500, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddAgility, GetBonusByLevel(60, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(50, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddSummonLimit, GetBonusByLevel(1, mount.Level)));
                    break;
                case 35:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddInitiative, GetBonusByLevel(500, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddMPAttack, GetBonusByLevel(1, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddSummonLimit, GetBonusByLevel(1, mount.Level)));
                    break;
                case 36:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(50, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddInitiative, GetBonusByLevel(500, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddChance, GetBonusByLevel(60, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddSummonLimit, GetBonusByLevel(1, mount.Level)));
                    break;
                case 37:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(40, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddInitiative, GetBonusByLevel(500, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_IncreaseDamage_138, GetBonusByLevel(60, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddSummonLimit, GetBonusByLevel(1, mount.Level)));
                    break;
                case 40:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddInitiative, GetBonusByLevel(500, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddSummonLimit, GetBonusByLevel(1, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddIntelligence, GetBonusByLevel(60, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(50, mount.Level)));
                    break;
                case 41:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddInitiative, GetBonusByLevel(500, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddStrength, GetBonusByLevel(60, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddSummonLimit, GetBonusByLevel(1, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(50, mount.Level)));
                    break;
                case 38:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddInitiative, GetBonusByLevel(500, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddSummonLimit, GetBonusByLevel(2, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    break;
                case 39:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(50, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddInitiative, GetBonusByLevel(500, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddSummonLimit, GetBonusByLevel(1, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddProspecting, GetBonusByLevel(40, mount.Level)));
                    break;
                case 89:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddDamageReflection, GetBonusByLevel(10, mount.Level)));
                    break;
                case 18:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(50, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddWisdom, GetBonusByLevel(40, mount.Level)));
                    break;
                case 42:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddAgility, GetBonusByLevel(60, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddWisdom, GetBonusByLevel(25, mount.Level)));
                    break;
                case 43:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddWisdom, GetBonusByLevel(25, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddMP, GetBonusByLevel(1, mount.Level)));
                    break;
                case 44:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddWisdom, GetBonusByLevel(25, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddChance, GetBonusByLevel(60, mount.Level)));
                    break;
                case 45:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_IncreaseDamage_138, GetBonusByLevel(40, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddWisdom, GetBonusByLevel(25, mount.Level)));
                    break;
                case 48:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddWisdom, GetBonusByLevel(25, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddIntelligence, GetBonusByLevel(60, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    break;
                case 49:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddWisdom, GetBonusByLevel(25, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddStrength, GetBonusByLevel(60, mount.Level)));
                    break;
                case 46:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddWisdom, GetBonusByLevel(25, mount.Level)));
                    break;
                case 47:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddWisdom, GetBonusByLevel(25, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddProspecting, GetBonusByLevel(40, mount.Level)));
                    break;
                case 3:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddAgility, GetBonusByLevel(80, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(50, mount.Level)));
                    break;
                case 50:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddAgility, GetBonusByLevel(30, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddMP, GetBonusByLevel(1, mount.Level)));
                    break;
                case 51:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddAgility, GetBonusByLevel(50, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddChance, GetBonusByLevel(50, mount.Level)));
                    break;
                case 9:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddAgility, GetBonusByLevel(40, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_IncreaseDamage_138, GetBonusByLevel(40, mount.Level)));
                    break;
                case 53:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddAgility, GetBonusByLevel(50, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddIntelligence, GetBonusByLevel(50, mount.Level)));
                    break;
                case 54:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddAgility, GetBonusByLevel(50, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddStrength, GetBonusByLevel(50, mount.Level)));
                    break;
                case 12:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddAgility, GetBonusByLevel(60, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(150, mount.Level)));
                    break;
                case 52:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddAgility, GetBonusByLevel(60, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddProspecting, GetBonusByLevel(40, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    break;
                case 21:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddMP, GetBonusByLevel(1, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(200, mount.Level)));
                    break;
                case 55:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddMP, GetBonusByLevel(1, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddChance, GetBonusByLevel(30, mount.Level)));
                    break;
                case 56:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddMP, GetBonusByLevel(1, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_IncreaseDamage_138, GetBonusByLevel(20, mount.Level)));
                    break;
                case 59:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddMP, GetBonusByLevel(1, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddIntelligence, GetBonusByLevel(30, mount.Level)));
                    break;
                case 60:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddMP, GetBonusByLevel(1, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddStrength, GetBonusByLevel(30, mount.Level)));
                    break;
                case 57:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddMP, GetBonusByLevel(1, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(300, mount.Level)));
                    break;
                case 58:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddProspecting, GetBonusByLevel(40, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddMP, GetBonusByLevel(1, mount.Level)));
                    break;
                case 88:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddAirResistPercent, GetBonusByLevel(5, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddWaterResistPercent, GetBonusByLevel(5, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddFireResistPercent, GetBonusByLevel(5, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddNeutralResistPercent, GetBonusByLevel(5, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddEarthResistPercent, GetBonusByLevel(5, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_IncreaseDamage_138, GetBonusByLevel(50, mount.Level)));
                    break;
                case 17:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddChance, GetBonusByLevel(80, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(50, mount.Level)));
                    break;
                case 61:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddChance, GetBonusByLevel(40, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_IncreaseDamage_138, GetBonusByLevel(40, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    break;
                case 64:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddChance, GetBonusByLevel(50, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddIntelligence, GetBonusByLevel(50, mount.Level)));
                    break;
                case 65:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddChance, GetBonusByLevel(50, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddStrength, GetBonusByLevel(50, mount.Level)));
                    break;
                case 62:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(150, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddChance, GetBonusByLevel(60, mount.Level)));
                    break;
                case 63:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddChance, GetBonusByLevel(60, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddProspecting, GetBonusByLevel(40, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    break;
                case 16:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_IncreaseDamage_138, GetBonusByLevel(50, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(50, mount.Level)));
                    break;
                case 67:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_IncreaseDamage_138, GetBonusByLevel(40, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddIntelligence, GetBonusByLevel(40, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    break;
                case 68:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_IncreaseDamage_138, GetBonusByLevel(40, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddStrength, GetBonusByLevel(40, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    break;
                case 11:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(200, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_IncreaseDamage_138, GetBonusByLevel(40, mount.Level)));
                    break;
                case 66:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddProspecting, GetBonusByLevel(40, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_IncreaseDamage_138, GetBonusByLevel(40, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    break;
                case 22:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(50, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddIntelligence, GetBonusByLevel(80, mount.Level)));
                    break;
                case 76:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddIntelligence, GetBonusByLevel(50, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddStrength, GetBonusByLevel(50, mount.Level)));
                    break;
                case 70:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddIntelligence, GetBonusByLevel(60, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(150, mount.Level)));
                    break;
                case 19:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(50, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddStrength, GetBonusByLevel(80, mount.Level)));
                    break;
                case 71:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddStrength, GetBonusByLevel(60, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(150, mount.Level)));
                    break;
                case 23:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(200, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddRange, GetBonusByLevel(2, mount.Level)));
                    break;
                case 77:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddInitiative, GetBonusByLevel(500, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(200, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddRange, GetBonusByLevel(1, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddSummonLimit, GetBonusByLevel(1, mount.Level)));
                    break;
                case 78:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(200, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddRange, GetBonusByLevel(1, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddWisdom, GetBonusByLevel(25, mount.Level)));
                    break;
                case 79:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddRange, GetBonusByLevel(1, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(200, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddAgility, GetBonusByLevel(60, mount.Level)));
                    break;
                case 80:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(200, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddRange, GetBonusByLevel(1, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddMP, GetBonusByLevel(1, mount.Level)));
                    break;
                case 82:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(200, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddChance, GetBonusByLevel(60, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddRange, GetBonusByLevel(1, mount.Level)));
                    break;
                case 83:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_IncreaseDamage_138, GetBonusByLevel(40, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(200, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddRange, GetBonusByLevel(1, mount.Level)));
                    break;
                case 86:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(200, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddRange, GetBonusByLevel(1, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddIntelligence, GetBonusByLevel(60, mount.Level)));
                    break;
                case 87:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(200, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddRange, GetBonusByLevel(1, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddStrength, GetBonusByLevel(60, mount.Level)));
                    break;
                case 84:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(300, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddRange, GetBonusByLevel(1, mount.Level)));
                    break;
                case 85:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(200, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddRange, GetBonusByLevel(1, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddProspecting, GetBonusByLevel(60, mount.Level)));
                    break;
                case 10:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    break;
                case 15:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(50, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddProspecting, GetBonusByLevel(80, mount.Level)));
                    break;
                case 72:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddIntelligence, GetBonusByLevel(60, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddProspecting, GetBonusByLevel(40, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    break;
                case 73:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddStrength, GetBonusByLevel(60, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddProspecting, GetBonusByLevel(40, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(100, mount.Level)));
                    break;
                case 69:
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddVitality, GetBonusByLevel(200, mount.Level)));
                    effects.Add(new EffectInteger(EffectsEnum.Effect_AddProspecting, GetBonusByLevel(40, mount.Level)));
                    break;
            }

            return effects;
        }

        public Mount CreateMount(Character character, int itemId)
        {
            var template = GetTemplateByScrollId(itemId);
            if (template == null)
                return null;

            var sex = (new Random().Next(0, 2)) == 1;
            var mount = new Mount(sex, template.Id);
            WorldServer.Instance.IOTaskPool.ExecuteInContext(() =>
            {
                AddMount(mount);
                StoreMount(character, mount);
            });

            return mount;
        }

        public Mount CreateMount(Character character, bool sex, int modelid)
        {
            var mount = new Mount(sex, modelid);

            WorldServer.Instance.IOTaskPool.ExecuteInContext(() =>
            {
                AddMount(mount);
                LinkMountToCharacter(character, mount);

                mount.Owner = character;
                character.Mount = mount;

                MountHandler.SendMountSetMessage(character.Client, mount.GetMountClientData());
            });

            return mount;
        }

        public void StoreMount(Character character, Mount mount)
        {
            var item = ItemManager.Instance.CreatePlayerItem(character, mount.ScrollItem, 1);

            var date = DateTime.Now;

            var nameEffect = new EffectString((short)EffectsEnum.Effect_Name, mount.Name, new EffectBase());
            var belongEffect = new EffectString((short)EffectsEnum.Effect_BelongsTo, character.Name, new EffectBase());
            var validityEffect = new EffectDuration((short)EffectsEnum.Effect_Validity, 39, 23, 59, new EffectBase());
            var mountEffect = new EffectMount((short)EffectsEnum.Effect_ViewMountCharacteristics, mount.Id, date.GetUnixTimeStampDouble(), mount.TemplateId, new EffectBase());

            item.Effects.Add(nameEffect);
            item.Effects.Add(belongEffect);
            item.Effects.Add(mountEffect);
            item.Effects.Add(validityEffect);

            character.Inventory.AddItem(item);
        }

        public void LinkMountToCharacter(Character character, Mount mount)
        {
            WorldServer.Instance.IOTaskPool.ExecuteInContext(() =>
            {
                var record = new MountCharacter
                {
                    CharacterId = character.Id,
                    MountId = mount.Id
                };

                Database.Insert(record);
            });
        }

        public void UnlinkMountFromCharacter(Character character)
        {
            WorldServer.Instance.IOTaskPool.ExecuteInContext(() =>
            {
                var record = Database.Query<MountCharacter>(string.Format(MountCharacterRelator.FetchByCharacterId, character.Id)).FirstOrDefault();
                Database.Delete(record);
            });
        }

        public void LinkMountToPaddock(Paddock paddock, Mount mount, bool stabled)
        {
            var record = new MountPaddock
            {
                PaddockId = paddock.Id,
                MountId = mount.Id,
                CharacterId = mount.OwnerId,
                Stabled = stabled
            };

            WorldServer.Instance.IOTaskPool.ExecuteInContext(
                () => Database.Insert(record));
        }

        public void UnlinkMountFromPaddock(Mount mount)
        {
            WorldServer.Instance.IOTaskPool.ExecuteInContext(() =>
            {
                var record = Database.Query<MountPaddock>(string.Format(MountPaddockRelator.FetchByMountId, mount.Id)).FirstOrDefault();
                Database.Delete(record);
            });
        }

        public void AddMount(Mount mount)
        {
            WorldServer.Instance.IOTaskPool.ExecuteInContext
                (() => Database.Insert(mount.Record));

            mount.Record.IsNew = false;
        }

        public void DeleteMount(Mount mount)
        {
            WorldServer.Instance.IOTaskPool.AddMessage(
                () => Database.Delete(mount.Record));
        }

        public MountRecord TryGetMountByCharacterId(int characterId)
        {
            var record = Database.Query<MountCharacter>(string.Format(MountCharacterRelator.FetchByCharacterId, characterId)).FirstOrDefault();
            return record == null ? null : TryGetMount(record.MountId);
        }

        public MountRecord TryGetMount(int mountId)
        {
            return Database.Query<MountRecord>(string.Format(MountRecordRelator.FindById, mountId)).FirstOrDefault();
        }

        public List<MountRecord> TryGetMountsByPaddockId(int paddockId, bool stabled)
        {
            return Database.Fetch<MountRecord>(string.Format(MountRecordRelator.FetchByPaddockId, paddockId, stabled ? 1 : 0));
        }

        public Mount GetMountById(int mountId)
        {
            var record = TryGetMount(mountId);
            return record == null ? null : new Mount(record);
        }
    }
}
