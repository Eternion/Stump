using System;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Game;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class WhoCommand : CommandBase
    {
        [Variable]
        public static int DisplayedElementsLimit = 19;

        public WhoCommand()
        {
            Aliases = new [] { "who" };
            RequiredRole = RoleEnum.Moderator;
            Description = "Return a list of playe based on the given arguments";

        }

        public override void Execute(TriggerBase trigger)
        {
            var list = World.Instance.GetCharacters(entry => true);

            int counter = 0;

            foreach (var character in list)
            {
                trigger.Reply("- " + trigger.Bold(character.Name) + " (" + character.Level + ")");
                counter++;

                if (counter >= 19)
                {
                    trigger.Reply("(...)");
                    break;
                }
            }

            if (counter == 0)
                trigger.ReplyError("No results found");
        }
    }
}