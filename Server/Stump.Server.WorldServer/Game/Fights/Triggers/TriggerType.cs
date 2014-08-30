using System;

namespace Stump.Server.WorldServer.Game.Fights.Triggers
{
    [Flags]
    public enum TriggerType
    {
        NEVER=0,
        TURN_BEGIN=1,
        TURN_END=2,
        MOVE=4,
    }
}