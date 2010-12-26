using System;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Actions
{
    public abstract class CharacterAction : ActionBase
    {
        public abstract void Execute(Character executer);
    }
}