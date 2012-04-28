using System.Collections.Generic;
using System.Drawing;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
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
            RequiredRole = RoleEnum.GameMaster;
            Description = "Just to be all powerfull.";
        }
    }


    public class GodOnCommand : TargetSubCommand
    {
        public GodOnCommand()
        {
            Aliases = new[] { "on" };
            RequiredRole = RoleEnum.GameMaster;
            ParentCommand = typeof(GodCommand);
            Description = "Activate god mode";
            AddTargetParameter(true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var target = GetTarget(trigger);

            target.ToggleGodMode(true);
            trigger.Reply("You are god !");
        }
    }
    public class GodOffCommand : TargetSubCommand
    {
        public GodOffCommand()
        {
            Aliases = new[] { "off" };
            RequiredRole = RoleEnum.Administrator;
            ParentCommand = typeof(GodCommand);
            Description = "Disable god mode";
            AddTargetParameter(true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var target = GetTarget(trigger);

            target.ToggleGodMode(false);
            trigger.Reply("You'r not god more");
        }
    }

    public class LevelUpCommand : TargetCommand
    {
        public LevelUpCommand()
        {
            Aliases = new[] { "levelup" };
            RequiredRole = RoleEnum.Administrator;
 
            AddParameter("amount", "amount", "Amount of levels to add", (short)1);
            AddTargetParameter(true, "Character who will level up");
        }

        public override void Execute(TriggerBase trigger)
        {
            Character target = GetTarget(trigger);
            byte delta;

            var amount = trigger.Get<short>("amount");
            if (amount > 0 && amount <= byte.MaxValue)
            {
                delta = (byte) (amount);
                target.LevelUp(delta);
                trigger.Reply("Added " + trigger.Bold("{0}") + " levels to '{1}'.", delta, target.Name);

            }
            else if (amount < 0 && -amount <= byte.MaxValue)
            {
                delta = (byte)( -amount );
                target.LevelDown(delta);
                trigger.Reply("Removed " + trigger.Bold("{0}") + " levels from '{1}'.", delta, target.Name);

            }
            else
            {
                trigger.ReplyError("Invalid level given. Must be greater then -255 and lesser than 255");
            }
        }
    }

    public class SetKamasCommand : TargetCommand
    {
        public SetKamasCommand()
        {
            Aliases = new[] { "kamas" };
            RequiredRole = RoleEnum.Administrator;

            AddParameter<int>("amount", "amount", "Amount of kamas to set");
            AddTargetParameter(true);
        }

        public override void Execute(TriggerBase trigger)
        {
            Character target = GetTarget(trigger);
            int kamas = trigger.Get<int>("amount");

            target.Inventory.SetKamas(kamas);
            trigger.Reply("{0} has now {1} kamas", target, kamas);
        }
    }
}