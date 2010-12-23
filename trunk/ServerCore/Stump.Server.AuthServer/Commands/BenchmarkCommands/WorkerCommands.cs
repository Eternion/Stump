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
using System;
using System.Collections.Generic;
using Stump.Database;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Network;
using Stump.Server.AuthServer.Accounts;
using Stump.Server.BaseServer.Commands;

namespace Stump.Server.AuthServer.Commands
{
    public class WorkerCommands : AuthCommand
    {
        public WorkerCommands()
        {
            Aliases = new[] { "worker", "wk" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Manage worker benchmarking.";
        }

        public override void Execute(TriggerBase trigger)
        {
            throw new DummyCommandException();
        }
    }


    public class WorkerMessageTypesCommand : AuthSubCommand
    {
        public WorkerMessageTypesCommand()
        {
            Aliases = new[] { "messType", "mt" };
            ParentCommand = typeof(WorkerCommands);
            RequiredRole = RoleEnum.Administrator;
            Description = "View message benchmarking group by type.";
            Parameters = new List<ICommandParameter>
                {
                    new CommandParameter<bool>("orderByTime", "order", "Order the messageTypes by treatmentTime",true,true),
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            var order = trigger.GetArgument<bool>("orderByTime");
            trigger.Reply(AuthentificationServer.Instance.WorkerManager.GetDetailedMessageTypes(order));
        }
    }

    public class WorkerMessageCommand : AuthSubCommand
    {
        public WorkerMessageCommand()
        {
            Aliases = new[] { "mess"};
            ParentCommand = typeof(WorkerCommands);
            RequiredRole = RoleEnum.Administrator;
            Description = "View message benchmarking group by type.";
            Parameters = new List<ICommandParameter>
                {
                    new CommandParameter<string>("messageType", "type", "type of the message"),
                    new CommandParameter<bool>("orderByTime", "order", "Order the messageTypes by treatmentTime",true,true),
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            var type = trigger.GetArgument<string>("messageType");
            var order = trigger.GetArgument<bool>("orderByTime");
            trigger.Reply(AuthentificationServer.Instance.WorkerManager.GetDetailedMessages(type, order));
        }
    }

    public class WorkerMessageInfoCommand : AuthSubCommand
    {
        public WorkerMessageInfoCommand()
        {
            Aliases = new[] { "messInfo", "mi" };
            ParentCommand = typeof(WorkerCommands);
            RequiredRole = RoleEnum.Administrator;
            Description = "View info on the message";
            Parameters = new List<ICommandParameter>
                {
                    new CommandParameter<string>("messageType", "type", "type of the message"),
                    new CommandParameter<int>("messId", "id", "Id of the message"),
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            var type = trigger.GetArgument<string>("messageType");
            var id = trigger.GetArgument<int>("messId");
            trigger.Reply(AuthentificationServer.Instance.WorkerManager.GetDetailedMessage(type, id));
        }
    }
}