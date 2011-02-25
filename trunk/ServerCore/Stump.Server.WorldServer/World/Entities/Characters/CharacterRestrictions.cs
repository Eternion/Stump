// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using Stump.BaseCore.Framework.Utils;
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
        CantBeAggressed,
        CantBeChallenged,
        CantTrade,
        CantBeAttackedByMutant,
        CantRun,
        ForceSlowWalk,
        CantMinimize,
        CantMove,
        CantAggress,
        CantChallenge,
        CantExchange,
        CantAttack,
        CantChat,
        CantBeMerchant,
        CantUseObject,
        CantUseTaxCollector,
        CantUseInteractive,
        CantSpeakToNpc,
        CantChangeZone,
        CantAttackMonster,
        CantWalk8Directions,
    }
}