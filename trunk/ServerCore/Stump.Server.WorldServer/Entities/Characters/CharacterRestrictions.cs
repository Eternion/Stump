
using Stump.DofusProtocol.Messages.Framework.IO;
using Stump.DofusProtocol.Classes;

namespace Stump.Server.WorldServer.World.Actors.Character
{
    public class CharacterRestrictions
    {
        public int Value { get; set; }

        public CharacterRestrictions(int value = 0)
        {
            Value = value;
        }

        public bool IsSet(Restrictions restriction)
        {
            return BooleanIntWrapper.GetFlag(Value, (byte) restriction);
        }

        public void Set(Restrictions restriction)
        {
          Value=  BooleanIntWrapper.SetFlag(Value, (byte) restriction, true);
        }

        public void UnSet(Restrictions restriction)
        {
            Value = BooleanIntWrapper.SetFlag(Value, (byte)restriction, false);
        }

        public ActorRestrictionsInformations ToActorRestrictionsInformations()
        {
            return new ActorRestrictionsInformations(
                IsSet(Restrictions.CantBeAggressed),
              IsSet(Restrictions.CantBeChallenged),
             IsSet(Restrictions.CantTrade),
             IsSet(Restrictions.CantBeAttackedByMutant),
             IsSet(Restrictions.CantRun),
             IsSet(Restrictions.ForceSlowWalk),
            IsSet(Restrictions.CantMinimize),
              IsSet(Restrictions.CantMove),
             IsSet(Restrictions.CantAggress),
             IsSet(Restrictions.CantChallenge),
             IsSet(Restrictions.CantExchange),
             IsSet(Restrictions.CantAttack),
             IsSet(Restrictions.CantChat),
             IsSet(Restrictions.CantBeMerchant),
              IsSet(Restrictions.CantUseObject),
             IsSet(Restrictions.CantUseTaxCollector),
            IsSet(Restrictions.CantUseInteractive),
              IsSet(Restrictions.CantSpeakToNpc),
              IsSet(Restrictions.CantChangeZone),
               IsSet(Restrictions.CantAttackMonster),
              IsSet(Restrictions.CantWalk8Directions));
        }
    }

    public enum Restrictions
    {
        CantBeAggressed = 0x0,
        CantBeChallenged = 0x1,
        CantTrade = 0x10,
        CantBeAttackedByMutant = 0x100,
        CantRun = 0x1000,
        ForceSlowWalk = 0x10000,
        CantMinimize = 1,
        CantMove = 1,
        CantAggress = 1,
        CantChallenge = 1,
        CantExchange = 1,
        CantAttack = 1,
        CantChat = 1,
        CantBeMerchant = 1,
        CantUseObject = 1,
        CantUseTaxCollector = 1,
        CantUseInteractive = 1,
        CantSpeakToNpc = 1,
        CantChangeZone = 1,
        CantAttackMonster = 1,
        CantWalk8Directions = 1,
    }
}