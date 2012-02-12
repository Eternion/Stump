using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Commands.Commands;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class SaveCommand : SubCommandContainer
    {
        public SaveCommand()
        {
            Aliases = new[] { "save" };
            Description = "Save the player";
            RequiredRole = RoleEnum.Player;
        }

        public override void Execute(TriggerBase trigger)
        {
            string str = trigger.Args.PeekNextWord();

            if (string.IsNullOrEmpty(str))
            {
                if (trigger is GameTrigger)
                {
                    ( trigger as GameTrigger ).Character.SaveLater();
                    trigger.Reply("Player saved");
                }
            }
            else
            {
                base.Execute(trigger);
            }
        }
    }

    public class SaveWorldCommand : SubCommand
    {
        public SaveWorldCommand()
        {
            Aliases = new [] { "world" };
            Description = "Save world";
            RequiredRole = RoleEnum.GameMaster;
            ParentCommand = typeof(SaveCommand);
        }

        public override void Execute(TriggerBase trigger)
        {
            WorldServer.Instance.IOTaskPool.AddMessage(World.Instance.Save);
        }
    }
}