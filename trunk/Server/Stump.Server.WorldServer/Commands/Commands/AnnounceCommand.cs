using System;
using System.Drawing;
using System.Globalization;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class AnnounceCommand : CommandBase
    {
        [Variable(true)]
        public static string AnnounceColor = ColorTranslator.ToHtml(Color.Red);

        public AnnounceCommand()
        {
            Aliases = new[] {"announce", "a"};
            Description = "Display an announce to all players";
            RequiredRole = RoleEnum.GameMaster;
            AddParameter<string>("message", "msg", "The announce");
        }

        public override void Execute(TriggerBase trigger)
        {
            Color color = ColorTranslator.FromHtml(AnnounceColor);

            var msg = trigger.Get<string>("msg");

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