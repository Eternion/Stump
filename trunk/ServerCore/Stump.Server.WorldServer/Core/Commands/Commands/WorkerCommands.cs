// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;

namespace Stump.Server.WorldServer.Commands
{
    public class WorkerCommands : WorldCommand
    {
        public WorkerCommands()
        {
            Aliases = new[] { "worker", "wk" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Manage worker's.";
        }

        public override void Execute(TriggerBase trigger)
        {
            throw new DummyCommandException();
        }
    }

    public class WorkerBenchmarkCommand : WorldSubCommand
    {
        public WorkerBenchmarkCommand()
        {
            Aliases = new[] { "benchmark", "bench" };
            ParentCommand = typeof(WorkerCommands);
            RequiredRole = RoleEnum.Administrator;
            Description = "Display the benchmark of treated messages";
            Parameters = new List<ICommandParameter>
                {
                    new CommandParameter<bool>("order", "o", "Order messages by average treatment time", true, true),
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            trigger.Reply(WorldServer.Instance.WorkerManager.
                GetDetailedMessageTypes(trigger.GetArgument<bool>("order")));
        }
    }

    public class WorkerMessageCommand : WorldSubCommand
    {
        public WorkerMessageCommand()
        {
            Aliases = new[] { "message", "msg" };
            ParentCommand = typeof(WorkerCommands);
            RequiredRole = RoleEnum.Administrator;
            Description = "Display detailed treatment of a message";
            Parameters = new List<ICommandParameter>
                {
                    new CommandParameter<string>("name", "n", "Name of the message"),
                    new CommandParameter<bool>("order", "o", "Order messages by average treatment time", true, true),
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            trigger.Reply(WorldServer.Instance.WorkerManager.GetDetailedMessages(
                trigger.GetArgument<string>("name"),
                trigger.GetArgument<bool>("order")));
        }
    }

    public class WorkerMessageIdCommand : WorldSubCommand
    {
        public WorkerMessageIdCommand()
        {
            Aliases = new[] { "messageid", "msgid" };
            ParentCommand = typeof(WorkerCommands);
            RequiredRole = RoleEnum.Administrator;
            Description = "Display info about the treatment of a unique message id";
            Parameters = new List<ICommandParameter>
                {
                    new CommandParameter<string>("name", "n", "Name of the message"),
                    new CommandParameter<int>("id", "id", "Unique message id's treatment"),
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            trigger.Reply(WorldServer.Instance.WorkerManager.GetDetailedMessage(
                trigger.GetArgument<string>("name"),
                trigger.GetArgument<int>("id")));
        }
    }
}