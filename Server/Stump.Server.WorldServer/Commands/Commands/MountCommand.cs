using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Mounts;
using Stump.Server.WorldServer.Handlers.Mounts;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class MountCommands : SubCommandContainer
    {
        public MountCommands()
        {
            Aliases = new[] { "mount" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Mounts Commands";
        }
    }

    public class MountSetCommand : InGameSubCommand
    {
        public MountSetCommand()
        {
            Aliases = new[] { "equip" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Equip a mount";
            ParentCommandType = typeof(MountCommands);
        }

        public override void Execute(GameTrigger trigger)
        {
            var source = trigger.GetSource() as WorldClient;

            if (trigger.Character.HasEquipedMount())
                MountManager.Instance.DeleteMount(trigger.Character.Mount);

            var mount = MountManager.Instance.CreateMount(trigger.Character, false, 9);

            trigger.Character.Mount = mount;
            MountHandler.SendMountSetMessage(source, mount.GetMountClientData());
        }
    }

    public class MountStableCommand : InGameSubCommand
    {
        public MountStableCommand()
        {
            Aliases = new[] { "stable" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Open stable panel";
            ParentCommandType = typeof(MountCommands);
        }

        public override void Execute(GameTrigger trigger)
        {
            var source = trigger.GetSource() as WorldClient;

            source.Send(new ExchangeStartOkMountMessage(new MountClientData[0], new MountClientData[0]));
        }
    }
}
