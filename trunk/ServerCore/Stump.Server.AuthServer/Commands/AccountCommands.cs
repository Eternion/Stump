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
using Stump.Server.AuthServer.Accounts;
using Stump.Server.BaseServer.Commands;

namespace Stump.Server.AuthServer.Commands
{
    public class AccountCommands : AuthCommand
    {
        public AccountCommands()
        {
            Aliases = new[] {"account", "acc"};
            RequiredRole = RoleEnum.Administrator;
            Description = "Provides many commands to manage accounts";
        }

        public override void Execute(TriggerBase trigger)
        {
            throw new DummyCommandException();
        }
    }

    public class AccountCreateCommand : AuthSubCommand
    {
        public AccountCreateCommand()
        {
            Aliases = new[] {"create", "cr", "new"};
            ParentCommand = typeof (AccountCommands);
            RequiredRole = RoleEnum.Administrator;
            Description = "Create a new account.";
            Parameters = new List<ICommandParameter>
                {
                    new CommandParameter<string>("accountname", "name", "Name of the created account"),
                    new CommandParameter<string>("password", "pass", "Password of the created accont"),
                    new CommandParameter<RoleEnum>("role", "role", "Role of the created account. See RoleEnum", true,
                                                   RoleEnum.Player, ParametersConverter.RoleConverter),
                    new CommandParameter<string>("question", "quest", "Secret question", true, "dummy?"),
                    new CommandParameter<string>("answer", "answer", "Answer to the secret question", true, "dummy!"),
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            var accname = trigger.GetArgument<string>("accountname");

            var acc = new AccountRecord
                {
                    Login = accname,
                    Password = trigger.GetArgument<string>("password"),
                    Nickname = trigger.GetArgument<string>("accountname"),
                    SecretQuestion = trigger.GetArgument<string>("question"),
                    SecretAnswer = trigger.GetArgument<string>("answer"),
                    Role = trigger.GetArgument<RoleEnum>("role")
                };

            if (AccountManager.CreateAccount(acc))
                trigger.Reply("Created Account \"{0}\". Role : {1}.", accname, Enum.GetName(typeof (RoleEnum), acc.Role));
            else
                trigger.Reply("Couldn't create account. Account may already exists.");
        }
    }
}