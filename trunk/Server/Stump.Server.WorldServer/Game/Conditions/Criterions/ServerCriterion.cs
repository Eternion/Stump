using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class ServerCriterion : Criterion
    {
        public const string Identifier = "SI";

        public int Server
        {
            get;
            set;
        }

        public override bool Eval(Character character)
        {
            return WorldServer.ServerInformation.Id == Server;
        }

        public override void Build()
        {
            int server;

            if (!int.TryParse(Literal, out server))
                throw new Exception(string.Format("Cannot build ServerCriterion, {0} is not a valid server", Literal));

            Server = server;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}