using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Stump.Core.I18N;
using Stump.Core.Xml.Config;
using Stump.DofusProtocol.Enums;
using Stump.ORM;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Spells;
using System.IO;
using Stump.Core.Attributes;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Tools.SpellsExplorer
{
    static class Program
    {
        public const string ConfigFile = "config.xml";
        static XmlConfig m_config;

        public static DatabaseAccessor m_databaseAccessor;

        [Variable]
        public static readonly DatabaseConfiguration DatabaseConfiguration = new DatabaseConfiguration
        {
            DbName = "stump_world",
            Host = "localhost",
            User = "root",
            Password = "",
            ProviderName = "MySql.Data.MySqlClient",
        };

        const Languages SecondaryLanguage = Languages.French;

        public static void Main()
        {
            Console.WriteLine("Load {0}...", ConfigFile);
            m_config = new XmlConfig(ConfigFile);
            m_config.AddAssembly(Assembly.GetExecutingAssembly());

            if (!File.Exists(ConfigFile))
                m_config.Create();
            else
                m_config.Load();

            Console.WriteLine("Initializing Database...");
            m_databaseAccessor = new DatabaseAccessor(DatabaseConfiguration);
            m_databaseAccessor.RegisterMappingAssembly(Assembly.GetExecutingAssembly());
            m_databaseAccessor.Initialize();

            Console.WriteLine("Opening Database...");
            m_databaseAccessor.OpenConnection();
            DataManager.DefaultDatabase = m_databaseAccessor.Database;

            Console.WriteLine("Loading texts...");
            TextManager.Instance.Initialize();
            TextManager.Instance.SetDefaultLanguage(SecondaryLanguage);

            Console.WriteLine("Loading effects...");
            EffectManager.Instance.Initialize();

            Console.WriteLine("Loading spells...");
            SpellManager.Instance.Initialize();
            

            while (true)
            {
                Console.Write(">");
                var pattern = Console.ReadLine();
                try
                {
                    if (pattern.StartsWith("target:"))
                    {
                        var target = pattern.Remove(0, "target:".Length);

                        if (!target.Any())
                        {
                            var targets = SpellManager.Instance.GetSpellTemplates()
                                .SelectMany(x => SpellManager.Instance.GetSpellLevel((int)x.SpellLevelsIds[0]).Effects.SelectMany(y => y.TargetMask.Split(','))).Select(z => z[0]).Distinct();

                            foreach (var mask in targets)
                                Console.WriteLine(mask);
                        }

                        foreach (
                            var spell in
                                SpellManager.Instance.GetSpellTemplates()
                                            .Where(
                                                x =>
                                                    SpellManager.Instance.GetSpellLevel((int) x.SpellLevelsIds[0])
                                                                .Effects.Any(y => y.TargetMask.Contains(target))))
                            Console.WriteLine("Spell:{0} ({1})", spell.Name, spell.Id);
                    }

                    if (pattern.StartsWith("trigger:"))
                    {
                        var trigger = pattern.Remove(0, "trigger:".Length);

                        foreach (
                            var spell in
                                SpellManager.Instance.GetSpellTemplates()
                                            .Where(
                                                x =>
                                                    SpellManager.Instance.GetSpellLevel((int)x.SpellLevelsIds[0])
                                                                .Effects.Any(y => y.Triggers.Split('|').Any(z => z== trigger))))
                            Console.WriteLine("Spell:{0} ({1})", spell.Name, spell.Id);
                    }

                    if (pattern.StartsWith("zone:"))
                    {
                        var zone = pattern.Remove(0, "zone:".Length);

                        foreach (
                            var spell in
                                SpellManager.Instance.GetSpellTemplates()
                                            .Where(
                                                x =>
                                                    SpellManager.Instance.GetSpellLevel((int)x.SpellLevelsIds[0])
                                                                .Effects.Any(y => y.ZoneShape.ToString() == zone)))
                            Console.WriteLine("Spell:{0} ({1})", spell.Name, spell.Id);
                    }

                    if (pattern.StartsWith("delay"))
                    {
                        foreach (
                            var spell in
                                SpellManager.Instance.GetSpellTemplates()
                                            .Where(
                                                x =>
                                                    SpellManager.Instance.GetSpellLevel((int) x.SpellLevelsIds[0])
                                                                .Effects.Any(y => y.Delay != 0)))
                            Console.WriteLine("Spell:{0} ({1})", spell.Name, spell.Id);
                    }
                                        
                    if (pattern.StartsWith("priority"))
                    {
                        foreach (
                            var spell in
                                SpellManager.Instance.GetSpellTemplates()
                                            .Where(
                                                x =>
                                                    SpellManager.Instance.GetSpellLevel((int) x.SpellLevelsIds[0])
                                                                .Effects.Any(y => y.Priority != 0)))
                            Console.WriteLine("Spell:{0} ({1})", spell.Name, spell.Id);
                    }                                        
                    if (pattern.StartsWith("group"))
                    {
                        foreach (
                            var spell in
                                SpellManager.Instance.GetSpellTemplates()
                                            .Where(
                                                x =>
                                                    SpellManager.Instance.GetSpellLevel((int) x.SpellLevelsIds[0])
                                                                .Effects.Any(y => y.Group != 0)))
                            Console.WriteLine("Spell:{0} ({1})", spell.Name, spell.Id);
                    }
                    if (pattern.StartsWith("@"))
                    {
                        pattern = pattern.Remove(0, 1);

                        var spells = SpellManager.Instance.GetSpellLevels()
                                        .Where(x => x.Effects.Any(y => y.Id == int.Parse(pattern)));

                        foreach (var spell in spells.Distinct())
                            Console.WriteLine("Spell:{0} ({1})", spell.Spell.Name, spell.SpellId);
                    }
                    if (pattern.StartsWith("casted:"))
                    {
                        var spellId = int.Parse(pattern.Remove(0, "casted:".Length));

                        var spells = SpellManager.Instance.GetSpellLevels()
                                        .Where(x => x.Effects.Any(y => (y.EffectId == EffectsEnum.Effect_TriggerBuff || y.EffectId == EffectsEnum.Effect_TriggerBuff_793 ||
                                                                   y.EffectId == EffectsEnum.Effect_CastSpell_1160 || y.EffectId == EffectsEnum.Effect_CastSpell_1017) && y.DiceNum == spellId));

                        foreach (var spell in spells.Distinct())
                            Console.WriteLine("Spell:{0} ({1})", spell.Spell.Name, spell.SpellId);
                    }
                    if (pattern.StartsWith("state:"))
                    {
                        var stateId = int.Parse(pattern.Remove(0, "state:".Length));

                        var spells = SpellManager.Instance.GetSpellLevels()
                                        .Where(x => x.Effects.Any(y => y.EffectId == EffectsEnum.Effect_AddState && y.Value == stateId));

                        foreach (var spell in spells.Distinct())
                            Console.WriteLine("Spell:{0} ({1})", spell.Spell.Name, spell.SpellId);
                    }

                    var critical = pattern.EndsWith("!");
                    if (critical)
                        pattern = pattern.Remove(pattern.Length - 1, 1);

                    foreach (SpellTemplate spell in FindSpells(pattern))
                        {
                        if (spell == null)
                            Console.WriteLine("Spell not found");
                        else
                            ExploreSpell(spell, FindPatternSpellLevel(pattern), critical);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public static IEnumerable<SpellTemplate> FindSpells(string pattern)
        {
            if (pattern.EndsWith("]") && pattern.LastIndexOf('[') != -1)
            {
                var selectorIndex = pattern.LastIndexOf('[');

                pattern = pattern.Remove(selectorIndex);
            }

            if (pattern.All(entry => entry >= '0' && entry <= '9'))
            {
                if (!int.TryParse(pattern, out int id))
                    yield break;

                yield return SpellManager.Instance.GetSpellTemplate(id);
            }
            else
            {
                foreach (SpellTemplate spell in SpellManager.Instance.GetSpellTemplates())
                {
                    if (string.IsNullOrEmpty(pattern) || spell.Name.StartsWith(pattern, StringComparison.InvariantCultureIgnoreCase) || 
                        TextManager.Instance.GetText(spell.NameId, SecondaryLanguage).Contains(pattern))
                        yield return spell;
                }
            }
        }

        public static int FindPatternSpellLevel(string pattern)
        {
            if (pattern.EndsWith("]") && pattern.LastIndexOf('[') != -1)
            {
                var selectorIndex = pattern.LastIndexOf('[') + 1;

                return int.Parse(pattern.Substring(selectorIndex, pattern.LastIndexOf(']') - selectorIndex));
            }

            return 1;
        }

        public static void ExploreSpell(SpellTemplate spell, int level, bool critical)
        {
            var levelTemplate = SpellManager.Instance.GetSpellLevel((int) spell.SpellLevelsIds[level - 1]);
            var type = SpellManager.Instance.GetSpellType(spell.TypeId);

            Console.WriteLine("Spell '{0}'  : {1} ({2}) - Level {3}", spell.Id, spell.Name, TextManager.Instance.GetText(spell.NameId, SecondaryLanguage), level);
            Console.WriteLine("Description: {0}", spell.Description);
            Console.WriteLine("");

            Console.WriteLine("Type : {0} - {1}", type.ShortName, type.LongName);
            Console.WriteLine("Level.SpellBreed = {0}, Level.HideEffects = {1}", levelTemplate.SpellBreed, levelTemplate.HideEffects);
            Console.WriteLine("Cost = {0}, Max Stack = {1}, MaxCastPerTurn = {2}, MaxCastPerTarget= {3}, MinCastInterval = {4}", levelTemplate.ApCost, levelTemplate.MaxStack, levelTemplate.MaxCastPerTurn, levelTemplate.MaxCastPerTarget, levelTemplate.MinCastInterval);
            Console.WriteLine("Range = {0}, MinRange = {1}, RangeCanBeBoosted={2}", levelTemplate.Range, levelTemplate.MinRange, levelTemplate.RangeCanBeBoosted);
            Console.WriteLine("CastInLine = {0}, CastInDiagonal = {1}, CastTestLos={2}", levelTemplate.CastInLine, levelTemplate.CastInDiagonal, levelTemplate.CastTestLos);
            Console.WriteLine("StatesRequired = {0}, StatesForbidden = {1}", levelTemplate.StatesRequiredCSV, levelTemplate.StatesForbiddenCSV);
            Console.WriteLine("");

            foreach (var effect in critical ? levelTemplate.CriticalEffects : levelTemplate.Effects)
            {
                Console.WriteLine("Effect \"{0}\" ({1}, {2})", TextManager.Instance.GetText(effect.Template.DescriptionId), effect.EffectId, (int)effect.EffectId);
                Console.WriteLine("DiceFace = {0}, DiceNum = {1}, Value = {2}", effect.DiceFace, effect.DiceNum, effect.Value);
                Console.WriteLine("Hidden = {0}, Modificator = {1}, Random = {2}, Group = {5}, Triggers = {3}, Delay = {4}, Category = {6}", effect.Hidden, effect.Modificator, effect.Random, effect.Triggers, effect.Delay, effect.Group, effect.Template.Category);
                Console.WriteLine("ZoneShape = {0}, ZoneSize = {1}-{2}, Duration = {3}, Target = {4}, Group = {5}, Priority = {6}, Template.Priority={7}", effect.ZoneShape, effect.ZoneMinSize, effect.ZoneSize, effect.Duration, effect.TargetMask, effect.Group, effect.Priority, effect.Template.EffectPriority);
                Console.WriteLine("Template.Active = {0}, Template.BonusType = {1}, Template.Boost = {2}", effect.Template.Active, effect.Template.BonusType, effect.Template.Boost);
                Console.WriteLine("Template.Category = {0}, Template.Characteristic = {1}, Template.ForceMinMax = {2}", effect.Template.Category, effect.Template.Characteristic, effect.Template.ForceMinMax);
                Console.WriteLine("Template.Operator = {0}, Template.Id = {1}, Template.ShowInSet = {2}", effect.Template.Operator, effect.Template.Id, effect.Template.ShowInSet);
                Console.WriteLine("Template.ShowInTooltip = {0}, Template.UseDice = {1}", effect.Template.ShowInTooltip, effect.Template.UseDice);
                Console.WriteLine("");
            }

            Console.WriteLine("");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("");
        }
    }
}