using System;
using System.Globalization;
using System.Linq;
using MongoDB.Bson;
using Stump.Core.IO;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Logging;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Commands.Trigger
{
    public abstract class GameTrigger : TriggerBase
    {
        protected GameTrigger(StringStream args, Character character)
            : base(args, character.UserGroup.Role)
        {
            Character = character;
        }


        protected GameTrigger(string args, Character character)
            : base(args, character.UserGroup.Role)
        {
            Character = character;
        }

        public override RoleEnum UserRole
        {
            get { return Character.UserGroup.Role; }
        }

        public override bool CanFormat
        {
            get
            {
                return true;
            }
        }

        public Character Character
        {
            get;
            protected set;
        }

        public override bool CanAccessCommand(CommandBase command)
        {
            return Character.UserGroup.IsCommandAvailable(command);
        }

        public override void Log()
        {
            if (BoundCommand.RequiredRole <= RoleEnum.Player)
                return;

            var document = new BsonDocument
            {
                { "AcctId", Character.Account.Id },
                { "CharacterId", Character.Id },
                { "Command", BoundCommand.Aliases[0] },
                { "Parameters", Args.String },
                { "Date", DateTime.Now.ToString(CultureInfo.InvariantCulture) }
            };

            MongoLogger.Instance.Insert("Commands", document);
        }
    }
}