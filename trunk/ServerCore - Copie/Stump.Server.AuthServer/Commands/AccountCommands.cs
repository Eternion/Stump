
using System;
using System.Collections.Generic;
using Stump.Database;
using Stump.Database.AuthServer;
using Stump.DofusProtocol.Enums;
using Stump.Server.AuthServer.Managers;
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
            Parameters = new List<IParameter>
                {
                    new ParameterDefinition<string>("accountname", "name", "Name of the created account"),
                    new ParameterDefinition<string>("password", "pass", "Password of the created accont"),
                    new ParameterDefinition<RoleEnum>("role", "role", "Role of the created account. See RoleEnum", true,
                                                   RoleEnum.Player, ParametersConverter.RoleConverter),
                    new ParameterDefinition<string>("question", "quest", "Secret question", true, "dummy?"),
                    new ParameterDefinition<string>("answer", "answer", "Answer to the secret question", true, "dummy!"),
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            var accname = trigger.Get<string>("accountname");

            var acc = new AccountRecord
                {
                    Login = accname,
                    Password = trigger.Get<string>("password"),
                    Nickname = trigger.Get<string>("accountname"),
                    SecretQuestion = trigger.Get<string>("question"),
                    SecretAnswer = trigger.Get<string>("answer"),
                    Role = trigger.Get<RoleEnum>("role"),
                    Email = "",
                    AvailableBreeds = AccountManager.AvailableBreeds,
                };

            if (AccountManager.CreateAccount(acc))
                trigger.Reply("Created Account \"{0}\". Role : {1}.", accname, Enum.GetName(typeof (RoleEnum), acc.Role));
            else
                trigger.Reply("Couldn't create account. Account may already exists.");
        }
    }
}