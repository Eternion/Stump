using System.Drawing;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class AnnounceCommand : CommandBase
    {
        [Variable]
        public static string AnnounceColor = Color.Red.ToArgb().ToString();

        public AnnounceCommand()
        {
            Aliases = new[] {"announce", "a"};
            Description = "Display an announce to all players";
            RequiredRole = RoleEnum.GameMaster;
        }

        public override void Execute(TriggerBase trigger)
        {
            var color = Color.FromArgb(int.Parse(AnnounceColor));

            string msg = trigger.Args.String;

            WorldServer.Instance.IOTaskPool.AddMessage(
                () =>
                    {
                        if (trigger is GameTrigger)
                        {
                            World.Instance.ForEachCharacter(entry => entry.SendServerMessage(string.Format("(ANNOUNCE) [{0}] : {1}", ( (GameTrigger)trigger ).Character.Name, msg), color));
                        }
                        else
                        {
                            World.Instance.ForEachCharacter(entry => entry.SendServerMessage(string.Format("(ANNOUNCE) {0}", msg), color));
                        }
                    });
        }
    }
}