using System;
using System.Globalization;
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

        public override RoleEnum UserRole => Character.UserGroup.Role;

        public override bool CanFormat => true;

        public Character Character
        {
            get;
            protected set;
        }

        public override bool CanAccessCommand(CommandBase command) => Character.UserGroup.IsCommandAvailable(command);

        public override void Log()
        {
            if (BoundCommand.RequiredRole <= RoleEnum.Player)
                return;

            var document = new BsonDocument
            {
                { "AcctId", Character.Account.Id },
                { "AcctName", Character.Account.Login },
                { "CharacterId", Character.Id },
                { "CharacterName", Character.Name },
                { "Command", BoundCommand.Aliases[0] },
                { "Parameters", Args.String },
                { "Date", DateTime.Now.ToString(CultureInfo.InvariantCulture) }
            };

            MongoLogger.Instance.Insert("Commands", document);
        }
    }
}