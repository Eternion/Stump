using System;

namespace Stump.Server.WorldServer.Game.Fights.Triggers
{
    [Flags]
    public enum TriggerType
    {
        NEVER=0,
        OnTurnBegin=1,
        OnTurnEnd=2,
        MOVE=4,
        CREATION=8,
    }
}