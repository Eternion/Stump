using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;

namespace Stump.Server.WorldServer.Commands.Commands
{

    public class GodCommand : SubCommandContainer
    {
        public GodCommand()
        {
            Aliases = new[] { "god" };
            RequiredRole = RoleEnum.GameMaster;
            Description = "Just to be all powerful.";
        }
    }


    public class GodOnCommand : TargetSubCommand
    {
        public GodOnCommand()
        {
            Aliases = new[] { "on" };
            RequiredRole = RoleEnum.GameMaster;
            ParentCommandType = typeof(GodCommand);
            Description = "Activate god mode";
            AddTargetParameter(true);
        }

        public override void Execute(TriggerBase trigger)
        {
            foreach (var target in GetTargets(trigger))
            {
                target.ToggleGodMode(true);
                trigger.Reply("You are god !");
            }
        }
    }
    public class GodOffCommand : TargetSubCommand
    {
        public GodOffCommand()
        {
            Aliases = new[] { "off" };
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof(GodCommand);
            Description = "Disable god mode";
            AddTargetParameter(true);
        }

        public override void Execute(TriggerBase trigger)
        {
            foreach (var target in GetTargets(trigger))
            {
                target.ToggleGodMode(false);
                trigger.Reply("You're not god anymore");
            }
        }
    }

    public class AdminChatCommand : InGameCommand
    {
        public AdminChatCommand()
        {
            Aliases = new[] { "admin" };
            RequiredRole = RoleEnum.Moderator;
            Description = "Enable/disable admin chat mode";
        }
        public override void Execute(GameTrigger trigger)
        {
            trigger.Reply("Admin chat mode is : {0}", trigger.Bold(
                (trigger.Character.AdminMessagesEnabled = !trigger.Character.AdminMessagesEnabled)
                    ? "Enabled"
                    : "Disabled"));
        }
    }

    public class LevelUpCommand : TargetCommand
    {
        public LevelUpCommand()
        {
            Aliases = new[] { "levelup" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Gives some levels to the target";
            AddParameter("amount", "amount", "Amount of levels to add", (short)1);
            AddTargetParameter(true, "Character who will level up");
        }

        public override void Execute(TriggerBase trigger)
        {
            foreach (var target in GetTargets(trigger))
            {
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
                    delta = (byte) (-amount);
                    target.LevelDown(delta);
                    trigger.Reply("Removed " + trigger.Bold("{0}") + " levels from '{1}'.", delta, target.Name);

                }
                else
                {
                    trigger.ReplyError("Invalid level given. Must be greater then -255 and lesser than 255");
                }
            }
        }
    }

    public class SetKamasCommand : TargetCommand
    {
        public SetKamasCommand()
        {
            Aliases = new[] { "kamas" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Set the amount kamas of target's inventory";
            AddParameter<int>("amount", "amount", "Amount of kamas to set");
            AddTargetParameter(true);
        }

        public override void Execute(TriggerBase trigger)
        {
            foreach (var target in GetTargets(trigger))
            {
                var kamas = trigger.Get<int>("amount");

                target.Inventory.SetKamas(kamas);
                trigger.ReplyBold("{0} has now {1} kamas", target, kamas);
            }
        }
    }

    public class SetStatsCommand : TargetCommand
    {
        public SetStatsCommand()
        {
            Aliases = new[] { "stats" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Set the amount of stats point of the target";
            AddParameter<ushort>("amount", "amount", "Amount of stats points to set");
            AddTargetParameter(true);
        }

        public override void Execute(TriggerBase trigger)
        {
            foreach (var target in GetTargets(trigger))
            {
                var statsPoints = trigger.Get<ushort>("amount");

                target.StatsPoints = statsPoints;
                target.RefreshStats();
                trigger.Reply("{0} has now {1} stats points", target, statsPoints);
            }

        }
    }

    public class InvisibleCommand : TargetCommand
    {
        public InvisibleCommand()
        {
            Aliases = new[] { "invisible", "setinv" };
            RequiredRole = RoleEnum.GameMaster_Padawan;
            Description = "Toggle invisible state";
            AddTargetParameter(true);
        }

        public override void Execute(TriggerBase trigger)
        {
            foreach (var target in GetTargets(trigger))
            {

                trigger.Reply(target.ToggleInvisibility() ? "{0} is now invisible" : "{0} is now visible", target);
            }
        }
    }

    public class HealCommand : TargetCommand
    {
        public HealCommand()
        {
            Aliases = new[] { "heal", "hp" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Restore Heal";
            AddTargetParameter(true);
        }

        public override void Execute(TriggerBase trigger)
        {
            foreach (var target in GetTargets(trigger))
            {
                target.Stats.Health.DamageTaken = 0;
                target.RefreshStats();
            }
        }
    }
}