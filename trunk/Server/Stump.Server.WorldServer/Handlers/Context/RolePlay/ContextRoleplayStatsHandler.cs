using System;
using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Handlers.Characters;

namespace Stump.Server.WorldServer.Handlers.Context.RolePlay
{
    public partial class ContextRoleplayHandler : WorldHandlerContainer
    {
        private readonly static Dictionary<StatsBoostTypeEnum, PlayerFields> m_statsEnumRelations = new Dictionary<StatsBoostTypeEnum, PlayerFields>
            {
                {StatsBoostTypeEnum.Strength, PlayerFields.Strength},
                {StatsBoostTypeEnum.Agility, PlayerFields.Agility},
                {StatsBoostTypeEnum.Chance, PlayerFields.Chance},
                {StatsBoostTypeEnum.Wisdom, PlayerFields.Wisdom},
                {StatsBoostTypeEnum.Intelligence, PlayerFields.Intelligence},
                {StatsBoostTypeEnum.Vitality, PlayerFields.Vitality},
            };
            
        [WorldHandler(StatsUpgradeRequestMessage.Id)]
        public static void HandleStatsUpgradeRequestMessage(WorldClient client, StatsUpgradeRequestMessage message)
        {
            var statsid = (StatsBoostTypeEnum)message.statId;

            if (statsid < StatsBoostTypeEnum.Strength ||
                statsid > StatsBoostTypeEnum.Intelligence)
                throw new Exception("Wrong statsid");

            if (message.boostPoint <= 0)
                throw new Exception("Client given 0 as boostpoint. Forbidden value.");

            var breed = client.ActiveCharacter.Breed;
            var actualPoints = client.ActiveCharacter.Stats[m_statsEnumRelations[statsid]].Base;

            var pts = message.boostPoint;

            if (pts < 1 || message.boostPoint > client.ActiveCharacter.StatsPoints)
                return;

            var thresholds = breed.GetThresholds(statsid);
            var index = breed.GetThresholdIndex(actualPoints, thresholds);

            while (pts > thresholds[index][1])
            {
                // if not last threshold and enough pts to reach the next threshold we fill this first
                if (index < thresholds.Count - 1 && (pts / (double)thresholds[index][1]) > (thresholds[index + 1][0] - actualPoints))
                {
                    var boost = thresholds[index + 1][0] - actualPoints;
                    actualPoints += (short)boost;
                    pts -= (short)( boost * thresholds[index][1] );
                }
                else
                {
                    var boost = (short)Math.Floor( pts / (double)thresholds[index][1] );
                    actualPoints += boost;
                    pts -= (short)(boost * thresholds[index][1]);
                }

                index = breed.GetThresholdIndex(actualPoints, thresholds);
            }

            client.ActiveCharacter.Stats[m_statsEnumRelations[statsid]].Base = actualPoints;
            client.ActiveCharacter.StatsPoints -= (ushort)(message.boostPoint - pts);

            SendStatsUpgradeResultMessage(client, message.boostPoint);
            client.ActiveCharacter.RefreshStats();
        }

        public static void SendStatsUpgradeResultMessage(IPacketReceiver client, short usedpts)
        {
            client.Send(new StatsUpgradeResultMessage(usedpts));
        }
    }
}