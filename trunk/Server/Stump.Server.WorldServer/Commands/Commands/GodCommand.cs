using System.Collections.Generic;
using System.Drawing;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class GodCommand : SubCommandContainer
    {
        public GodCommand()
        {
            Aliases = new[] { "god" };
            RequiredRole = RoleEnum.Moderator;
            Description = "Just to be all powerfull.";
        }
    }

    public class CheatCommand : SubCommand
    {
        public CheatCommand()
        {
            Aliases = new[] {"cheat"};
            RequiredRole = RoleEnum.Administrator;
            ParentCommand = typeof (GodCommand);
        }

        public override void Execute(TriggerBase trigger)
        {
            if (!trigger.IsArgumentDefined("target") && trigger is GameTrigger)
            {
                Character target = (trigger as GameTrigger).Character;
                target.Stats.Health.Given += 5000;
                target.Stats.Fields[CaracteristicsEnum.Intelligence].Given += 5000;
                target.Stats.Fields[CaracteristicsEnum.Strength].Given += 5000;
                target.Stats.Fields[CaracteristicsEnum.Chance].Given += 5000;
                trigger.Reply("You're now a cheat. Make the good, love everyone etc.");
            }
        }
    }

    public class LevelUpCommand : CommandBase
    {
        public LevelUpCommand()
        {
            Aliases = new[] { "level_up" };
            RequiredRole = RoleEnum.Administrator;
 
            AddParameter("amount", "amount", "Amount of levels to add", (byte)1);
            AddParameter("target", "t", "Character who will level up", isOptional: true, converter: ParametersConverter.CharacterConverter);  
        }

        public override void Execute(TriggerBase trigger)
        {
            Character target;

            if (!trigger.IsArgumentDefined("target") && trigger is GameTrigger)
                target = (trigger as GameTrigger).Character;
            else
                target = trigger.Get<Character>("target");

            target.LevelUp(trigger.Get<byte>("amount"));

            trigger.Reply("Added " + trigger.Bold("{0}") + " levels to '{1}'.", trigger.Get<byte>("amount"), target.Name);
        }
    }
}