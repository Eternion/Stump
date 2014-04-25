using System;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
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
    }
}