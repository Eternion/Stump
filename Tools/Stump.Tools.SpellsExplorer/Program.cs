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
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Spells;
using System.IO;
using Stump.Core.Attributes;

namespace Stump.Tools.SpellsExplorer
{
    internal static class Program
    {
        public const string ConfigFile = "config.xml";
        private static XmlConfig m_config;

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

        private const Languages SecondaryLanguage = Languages.French;

        public static void Main()
        {
            Console.BufferWidth = 90;
            Console.BufferHeight = 1024;
            Console.WindowWidth = 90;
            Console.WindowHeight = 45;

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
                    if (pattern.StartsWith("flags:"))
                    {
                        var flag = int.Parse(pattern.Remove(0, "flags:".Length), NumberStyles.HexNumber);

                        foreach (
                            var spell in
                                SpellManager.Instance.GetSpellTemplates()
                                            .Where(
                                                x =>
                                                    SpellManager.Instance.GetSpellLevel((int) x.SpellLevelsIds[0])
                                                                .Effects.Any(y => (int) y.Targets == flag)))
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
                                                                .Effects.Any(y => (int) y.Delay != 0)))
                            Console.WriteLine("Spell:{0} ({1})", spell.Name, spell.Id);
                    }
                                        
                    if (pattern.StartsWith("mod"))
                    {
                        foreach (
                            var spell in
                                SpellManager.Instance.GetSpellTemplates()
                                            .Where(
                                                x =>
                                                    SpellManager.Instance.GetSpellLevel((int) x.SpellLevelsIds[0])
                                                                .Effects.Any(y => (int) y.Modificator != 0)))
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
                                                                .Effects.Any(y => (int) y.Group != 0)))
                            Console.WriteLine("Spell:{0} ({1})", spell.Name, spell.Id);
                    }
                    if (pattern.StartsWith("@"))
                    {
                        pattern = pattern.Remove(0, 1);

                        foreach (var spell in SpellManager.Instance.GetSpellTemplates() .Where(
                            x => SpellManager.Instance.GetSpellLevel((int)x.SpellLevelsIds[0]).Effects.Any(y => y.Id == int.Parse(pattern))))
                        {
                            Console.WriteLine("Spell:{0} ({1})", spell.Name, spell.Id);
                        }
                        continue;
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
                int selectorIndex = pattern.LastIndexOf('[');

                pattern = pattern.Remove(selectorIndex);
            }

            if (pattern.All(entry => entry >= '0' && entry <= '9'))
            {
                int id;

                if (!int.TryParse(pattern, out id))
                    yield break;

                yield return SpellManager.Instance.GetSpellTemplate(id);
            }
            else
            {
                foreach (SpellTemplate spell in SpellManager.Instance.GetSpellTemplates())
                {
                    if (string.IsNullOrEmpty(pattern) || spell.Name.IndexOf(pattern, StringComparison.InvariantCultureIgnoreCase) != -1 || 
                        TextManager.Instance.GetText(spell.NameId, SecondaryLanguage).Contains(pattern))
                        yield return spell;
                }
            }
        }

        public static int FindPatternSpellLevel(string pattern)
        {
            if (pattern.EndsWith("]") && pattern.LastIndexOf('[') != -1)
            {
                int selectorIndex = pattern.LastIndexOf('[') + 1;

                return int.Parse(pattern.Substring(selectorIndex, pattern.LastIndexOf(']') - selectorIndex));
            }

            return 1;
        }

        public static void ExploreSpell(SpellTemplate spell, int level, bool critical)
        {
            var levelTemplate = SpellManager.Instance.GetSpellLevel((int) spell.SpellLevelsIds[level - 1]);
            var type = SpellManager.Instance.GetSpellType(spell.TypeId);

            Console.WriteLine("Spell '{0}'  : {1} ({2}) - Level {3}", spell.Id, spell.Name, TextManager.Instance.GetText(spell.NameId, SecondaryLanguage), level);
            Console.WriteLine("Type : {0} - {1}", type.ShortName, type.LongName);
            Console.WriteLine("Level.SpellBreed = {0}, Level.HideEffects = {1}", levelTemplate.SpellBreed, levelTemplate.HideEffects);
            Console.WriteLine("");

            foreach (var effect in critical ? levelTemplate.CriticalEffects : levelTemplate.Effects)
            {
                Console.WriteLine("Effect \"{0}\" ({1}, {2})", TextManager.Instance.GetText(effect.Template.DescriptionId), effect.EffectId, (int)effect.EffectId);
                Console.WriteLine("DiceFace = {0}, DiceNum = {1}, Value = {2}", effect.DiceFace, effect.DiceNum, effect.Value);
                Console.WriteLine("Hidden = {0}, Modificator = {1}, Random = {2}, Trigger = {3}, Delay = {4}", effect.Hidden, effect.Modificator, effect.Random, effect.Trigger, effect.Delay);
                Console.WriteLine("ZoneShape = {0}, ZoneSize = {1}-{2}, Duration = {3}, Target = {4}, Group = {5}", effect.ZoneShape, effect.ZoneMinSize, effect.ZoneSize, effect.Duration, effect.Targets, effect.Group);
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