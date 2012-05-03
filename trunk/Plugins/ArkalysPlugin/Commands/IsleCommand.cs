using System.Collections.Generic;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace ArkalysPlugin.Commands
{
    public class IsleCommand : InGameCommand
    {
        [Variable(true)]
        public static Isle[] ProfilsIsle = new Isle[]
        {
        };

        public IsleCommand()
        {
            Aliases = new[] { "ile", "isle" };
            Description = "Teleporte sur une des île";
            RequiredRole = RoleEnum.Player;
            AddParameter<int>("num", "num", "Numéro de l'île (de 1 à " + ProfilsIsle.Length + ")");
        }

        public override void Execute(GameTrigger trigger)
        {
            var character = trigger.Character;
            var num = trigger.Get<int>("num");

            if (num < 1 || num > ProfilsIsle.Length)
            {
                trigger.ReplyError("Veuillez spécifier un numéro entre {0} et {1}", 1, ProfilsIsle.Length);
                return;
            }

            var isle = ProfilsIsle[num - 1];

            if (!isle.CanJoinIsle(character))
            {
                trigger.ReplyError("Vous ne pouvez pas rejoindre l'île {0} avant d'avoir atteint le niveau {1}", num, isle.Level);
                return;
            }

            var map = World.Instance.GetMap(isle.StartMap);

            if (map == null)
            {
                trigger.ReplyError("Map {0} not found", isle.StartMap);
                return;
            }

            var cell = map.Cells[isle.StartCell];

            character.Teleport(new ObjectPosition(map, cell, (DirectionsEnum)isle.StartDirection));
            trigger.Reply("Téléporté sur l'île {0}", num);
        }
    }
}