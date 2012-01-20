using System;
using Stump.Core.IO;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Commands.Trigger
{
    public abstract class GameTrigger : TriggerBase
    {
        protected GameTrigger(StringStream args, RoleEnum userRole, Character character)
            : base(args, userRole)
        {
            Character = character;
        }


        protected GameTrigger(string args, RoleEnum userRole, Character character)
            : base(args, userRole)
        {
            Character = character;
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
    }
}