#region License GNU GPL
// SpellsCommands.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class SpellsCommands : SubCommandContainer
    {
        public SpellsCommands()
        {
            Aliases = new[] {"spell"};
            RequiredRole = RoleEnum.Administrator;
            Description = "Manage spells";
        }
    }

    public class LearnSpellCommand : TargetSubCommand
    {
        public LearnSpellCommand()
        {
            Aliases = new[] {"learn", "add"};
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof (SpellsCommands);
            Description = "Learn the given spell";
            AddParameter("spell", "spell", "Given spell to learn", converter: ParametersConverter.SpellTemplateConverter);
            AddTargetParameter(true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var spell = trigger.Get<SpellTemplate>("spell");

            var target = GetTarget(trigger);

            var result = target.Spells.LearnSpell(spell);

            if (result != null)
                trigger.Reply("'{0}' learned the spell '{1}'", trigger.Bold(target), trigger.Bold(spell.Name));
            else
                trigger.ReplyError("Spell {0} not learned. Unknow reason", trigger.Bold(spell.Name));
        }
    }

    public class UnLearnSpellCommand : TargetSubCommand
    {
        public UnLearnSpellCommand()
        {
            Aliases = new[] {"unlearn", "forget", "remove"};
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof(SpellsCommands);
            Description = "Forget the given spell";
            AddParameter("spell", "spell", "Given spell to forget", converter: ParametersConverter.SpellTemplateConverter);
            AddTargetParameter(true);
            AddParameter<bool>("keep", "keep", "If true, keep the spell but reset it to level 1", isOptional:true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var spell = trigger.Get<SpellTemplate>("spell");
            var target = GetTarget(trigger);

            var result = trigger.Get<bool>("keep") ? target.Spells.ForgetSpell(spell) : target.Spells.UnLearnSpell(spell);

            if (result)
                trigger.Reply("'{0}' forgot the spell '{1}'{2}", trigger.Bold(target), trigger.Bold(spell.Name),
                    trigger.Get<bool>("keep") ? " (but kept it)" : string.Empty);
            else
                trigger.ReplyError("Spell {0} not unlearned. {1} may not have this spell", trigger.Bold(spell.Name), trigger.Bold(target));

        }
    }

    public class LearnMonsterSpellCommand : TargetSubCommand
    {
        public LearnMonsterSpellCommand()
        {
            Aliases = new[] { "learnmonster" };
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof(SpellsCommands);
            Description = "Learn the given spell";
            AddParameter("monster", "monster", "Target monster to learn spells", converter: ParametersConverter.MonsterTemplateConverter, isOptional: true);
            AddTargetParameter(true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var monster = trigger.Get<MonsterTemplate>("monster");
            var target = GetTarget(trigger);

            foreach (var spell in monster.Grades.FirstOrDefault().SpellsTemplates)
            {
                var result = target.Spells.LearnSpell(spell.SpellId);

                if (result != null)
                    trigger.Reply("'{0}' learned the spell '{1}'", trigger.Bold(target), trigger.Bold(result.Template.Name));
                else
                    trigger.ReplyError("Spell {0} not learned. Unknow reason", trigger.Bold(spell.SpellId));
            }
        }
    }
    public class ListSpellsCommand : TargetSubCommand
    {
        public ListSpellsCommand()
        {
            Aliases = new[] {"list"};
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof(SpellsCommands);
            Description = "List the spells of the target";
            AddTargetParameter(true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var target = GetTarget(trigger);
            foreach (var spell in target.Spells)
            {
                trigger.Reply("{0} ({1}) - Level {2}", trigger.Bold(spell.Template.Name), trigger.Bold(spell.Id),
                              trigger.Bold(spell.CurrentLevel));
            }
        }
    }

    public class SetSpellLevelCommand : TargetSubCommand
    {
        public SetSpellLevelCommand()
        {
            Aliases = new[] {"level"};
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof(SpellsCommands);
            Description = "Set the level of the given spell of the target";
            AddParameter("spell", "spell", "Given spell to forget", converter: ParametersConverter.SpellTemplateConverter);
            AddParameter<int>("level", "l");
            AddTargetParameter(true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var target = GetTarget(trigger);
            var template = trigger.Get<SpellTemplate>("spell");
            var level = trigger.Get<int>("level");

            var spell = target.Spells.GetSpell(template.Id);

            if (spell == null)
            {
                trigger.ReplyError("Spell {0} not found", trigger.Bold(spell));
                return;
            }

            if (!spell.ByLevel.ContainsKey(level))
            {
                trigger.ReplyError("Level {0} not found. Give a level between {1} and {2}", trigger.Bold(level),
                                   trigger.Bold(spell.ByLevel.Keys.Min()), trigger.Bold(spell.ByLevel.Keys.Max()));
                return;
            }

            spell.CurrentLevel = (byte)level;
            trigger.ReplyBold("{0}'s spell {1} is now level {2}", target, spell.Template.Name, level);
            
            //ContextRoleplayHandler.SendSpellUpgradeSuccessMessage(target.Client, spell);
        }
    }

    public class SpellInfoCommand : SubCommand
    {
        public SpellInfoCommand()
        {
            Aliases = new[] { "info" };
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof(SpellsCommands);
            Description = "Get informations about a spell";
            AddParameter("spell", "spell", "Given spell to forget", converter: ParametersConverter.SpellTemplateConverter);
            AddParameter<int>("level", "l");
        }

        public override void Execute(TriggerBase trigger)
        {
            var template = trigger.Get<SpellTemplate>("spell");
            var level = trigger.Get<int>("level");

            ExploreSpell(template, level, false, trigger);
        }

        public static void ExploreSpell(SpellTemplate spell, int level, bool critical, TriggerBase trigger)
        {
            var levelTemplate = SpellManager.Instance.GetSpellLevel((int)spell.SpellLevelsIds[level - 1]);
            var type = SpellManager.Instance.GetSpellType(spell.TypeId);

            trigger.ReplyBold("Spell '{0}'  : {1} - Level {2}", spell.Id, spell.Name, level);
            trigger.ReplyBold("Type : {0} - {1}", type.ShortName, type.LongName);
            trigger.ReplyBold("Level.SpellBreed = {0}, Level.HideEffects = {1}", levelTemplate.SpellBreed, levelTemplate.HideEffects);
            trigger.ReplyBold("");

            foreach (var effect in critical ? levelTemplate.CriticalEffects : levelTemplate.Effects)
            {
                trigger.ReplyBold("Effect \"{0}\" ({1}, {2})", TextManager.Instance.GetText(effect.Template.DescriptionId), effect.EffectId, (int)effect.EffectId);
                trigger.ReplyBold("DiceFace = {0}, DiceNum = {1}, Value = {2}", effect.DiceFace, effect.DiceNum, effect.Value);
                trigger.ReplyBold("Hidden = {0}, Modificator = {1}, Random = {2}, Trigger = {3}, Delay = {4}", effect.Hidden, effect.Modificator, effect.Random, effect.Trigger, effect.Delay);
                trigger.ReplyBold("ZoneShape = {0}, ZoneSize = {1}-{2}, Duration = {3}, Target = {4}, Group = {5}", effect.ZoneShape, effect.ZoneMinSize, effect.ZoneSize, effect.Duration, effect.Targets, effect.Group);
                trigger.ReplyBold("Template.Active = {0}, Template.BonusType = {1}, Template.Boost = {2}", effect.Template.Active, effect.Template.BonusType, effect.Template.Boost);
                trigger.ReplyBold("Template.Category = {0}, Template.Characteristic = {1}, Template.ForceMinMax = {2}", effect.Template.Category, effect.Template.Characteristic, effect.Template.ForceMinMax);
                trigger.ReplyBold("Template.Operator = {0}, Template.Id = {1}, Template.ShowInSet = {2}", effect.Template.Operator, effect.Template.Id, effect.Template.ShowInSet);
                trigger.ReplyBold("Template.ShowInTooltip = {0}, Template.UseDice = {1}", effect.Template.ShowInTooltip, effect.Template.UseDice);
                trigger.ReplyBold("");
            }

            trigger.ReplyBold("");
            trigger.ReplyBold("---------------------------------------------");
            trigger.ReplyBold("");
        }
    }
}