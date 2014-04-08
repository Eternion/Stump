#region License GNU GPL
// TitleCommands.cs
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
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Database.Tinsel;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class AddRemoveTitleCommand : AddRemoveCommand
    {
        public AddRemoveTitleCommand()
        {
            Aliases = new[] {"title"};
            RequiredRole = RoleEnum.Administrator;
            Description = "Add or remove a title on the target";
            AddParameter("target", "t", "Target", converter: ParametersConverter.CharacterConverter);
            AddParameter<short>("id", "id", "Id of the title", isOptional: true);
            AddParameter<bool>("all", "a", "Add/remove all titles", isOptional:true);
        }

        public Character GetTarget(TriggerBase trigger)
        {
            Character target = null;
            if (trigger.IsArgumentDefined("target"))
                target = trigger.Get<Character>("target");
            else if (trigger is GameTrigger)
                target = ( trigger as GameTrigger ).Character;

            if (target == null)
                throw new Exception("Target is not defined");

            return target;
        }

        public override void ExecuteAdd(TriggerBase trigger)
        {
            var target = GetTarget(trigger);

            if (trigger.Get<bool>("all"))
            {
                foreach (var title in TinselManager.Instance.Titles)
                {
                    target.AddTitle(title.Key);
                }
                trigger.ReplyBold("{0} learned all titles", target);
            }
            else
            {
                if (!trigger.IsArgumentDefined("id"))
                {
                    trigger.ReplyError("Define at least one argument (id or -all)");
                }
                else
                {
                    var id = trigger.Get<short>("id");
                    if (!TinselManager.Instance.Titles.ContainsKey(id))
                        trigger.ReplyError("Title {0} doesn't exists");
                    else
                    {
                        target.AddTitle(id);
                        trigger.ReplyBold("{0} learned title {1}", target, TinselManager.Instance.Titles[id].Name);
                    }
                }
            }
        }

        public override void ExecuteRemove(TriggerBase trigger)
        {
            var target = GetTarget(trigger);

            if (trigger.Get<bool>("all"))
            {
                foreach (var title in TinselManager.Instance.Titles)
                {
                    target.RemoveTitle(title.Key);
                }
                target.ResetTitle();
                trigger.ReplyBold("{0} forgot all titles", target);
            }
            else
            {
                if (!trigger.IsArgumentDefined("id"))
                {
                    trigger.ReplyError("Define at least one argument (id or -all)");
                }
                else
                {
                    var id = trigger.Get<short>("id");
                    if (!TinselManager.Instance.Titles.ContainsKey(id))
                        trigger.ReplyError("Title {0} doesn't exists");
                    else
                    {
                        target.RemoveTitle(id);
                        if (target.SelectedTitle == id)
                            target.ResetTitle();

                        trigger.ReplyBold("{0} forgot title {1}", target, TinselManager.Instance.Titles[id].Name);
                    }
                }
            }
        }
    }

    public class AddRemoveOrnamentCommand : AddRemoveCommand
    {
        public AddRemoveOrnamentCommand()
        {
            Aliases = new[] { "ornament" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Add or remove an ornament on the target";
            AddParameter("target", "t", "Target", converter: ParametersConverter.CharacterConverter);
            AddParameter<short>("id", "id", "Id of the ornament", isOptional: true);
            AddParameter<bool>("all", "a", "Add/remove all ornaments", isOptional: true);
        }

        public Character GetTarget(TriggerBase trigger)
        {
            Character target = null;
            if (trigger.IsArgumentDefined("target"))
                target = trigger.Get<Character>("target");
            else if (trigger is GameTrigger)
                target = ( trigger as GameTrigger ).Character;

            if (target == null)
                throw new Exception("Target is not defined");

            return target;
        }

        public override void ExecuteAdd(TriggerBase trigger)
        {
            var target = GetTarget(trigger);

            if (trigger.Get<bool>("all"))
            {
                foreach (var title in TinselManager.Instance.Ornaments)
                {
                    target.AddOrnament(title.Key);
                }
                trigger.ReplyBold("{0} learned all ornaments", target);
            }
            else
            {
                if (!trigger.IsArgumentDefined("id"))
                {
                    trigger.ReplyError("Define at least one argument (id or -all)");
                }
                else
                {
                    var id = trigger.Get<short>("id");
                    if (!TinselManager.Instance.Ornaments.ContainsKey(id))
                        trigger.ReplyError("Ornament {0} doesn't exists");
                    else
                    {
                        target.AddOrnament(id);
                        trigger.ReplyBold("{0} learned ornament {1}", target, id);
                    }
                }
            }
        }

        public override void ExecuteRemove(TriggerBase trigger)
        {
            var target = GetTarget(trigger);

            if (trigger.Get<bool>("all"))
            {
                target.RemoveAllOrnament();
                target.ResetOrnament();
                trigger.ReplyBold("{0} forgot all ornaments", target);
            }
            else
            {
                if (!trigger.IsArgumentDefined("id"))
                {
                    trigger.ReplyError("Define at least one argument (id or -all)");
                }
                else
                {
                    var id = trigger.Get<short>("id");
                    if (!TinselManager.Instance.Ornaments.ContainsKey(id))
                        trigger.ReplyError("Ornament {0} doesn't exists");
                    else
                    {
                        target.RemoveOrnament(id);
                        if (target.SelectedOrnament == id)
                            target.ResetOrnament();
                        trigger.ReplyBold("{0} forgot ornament {1}", target, id);
                    }
                }
            }
        }
    }
}