
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.Server.AuthServer.Database.Account;
using Stump.Server.AuthServer.Managers;
using Stump.Server.BaseServer.Commands;

namespace Stump.Server.AuthServer.Commands
{
    public class AccountCommands : SubCommandContainer
    {
        public AccountCommands()
        {
            Aliases = new[] {"account", "acc"};
            RequiredRole = RoleEnum.Administrator;
            Description = "Provides many commands to manage accounts";
        }
    }

    public class AccountCreateCommand : SubCommand
    {
        public AccountCreateCommand()
        {
            Aliases = new[] {"create", "cr", "new"};
            ParentCommand = typeof (AccountCommands);
            RequiredRole = RoleEnum.Administrator;
            Description = "Create a new account.";
            Parameters = new List<IParameterDefinition>
                {
                    new ParameterDefinition<string>("accountname", "name", "Name of the created account"),
                    new ParameterDefinition<string>("password", "pass", "Password of the created accont"),
                    new ParameterDefinition<RoleEnum>("role", "role", "Role of the created account. See RoleEnum",
                                                   RoleEnum.Player, ParametersConverter.RoleConverter),
                    new ParameterDefinition<string>("question", "quest", "Secret question", "dummy?"),
                    new ParameterDefinition<string>("answer", "answer", "Answer to the secret question", "dummy!"),
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            var accname = trigger.Get<string>("accountname");

            var acc = new Account
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

            if (AccountManager.Instance.CreateAccount(acc))
                trigger.Reply("Created Account \"{0}\". Role : {1}.", accname, Enum.GetName(typeof (RoleEnum), acc.Role));
            else
                trigger.Reply("Couldn't create account. Account may already exists.");
        }
    }
}