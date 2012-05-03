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

            var breed = client.Character.Breed;
            var actualPoints = client.Character.Stats[m_statsEnumRelations[statsid]].Base;

            var pts = message.boostPoint;

            if (pts < 1 || message.boostPoint > client.Character.StatsPoints)
                return;

            var thresholds = breed.GetThresholds(statsid);
            var index = breed.GetThresholdIndex(actualPoints, thresholds);

            // [0] => limit
            // [1] => pts for 1
            // [2] => boosts with 1

            // while enough pts to boost once
            while (pts >= thresholds[index][1])
            {
                short ptsUsed = 0;
                short boost = 0;
                // if not last threshold and enough pts to reach the next threshold we fill this first
                if (index < thresholds.Count - 1 && (pts / (double)thresholds[index][1]) > (thresholds[index + 1][0] - actualPoints))
                {
                    boost = (short) (thresholds[index + 1][0] - actualPoints);
                    ptsUsed = (short)( boost * thresholds[index][1] );

                    if (thresholds[index].Count > 2)
                        boost = (short)( boost * thresholds[index][2] );
                }
                else
                {
                    boost = (short)Math.Floor( pts / (double)thresholds[index][1] );
                    ptsUsed = (short)( boost * thresholds[index][1] );

                    if (thresholds[index].Count > 2)
                        boost = (short)(boost * thresholds[index][2]);
                }

                actualPoints += boost;
                pts -= ptsUsed;

                index = breed.GetThresholdIndex(actualPoints, thresholds);
            }

            client.Character.Stats[m_statsEnumRelations[statsid]].Base = actualPoints;
            client.Character.StatsPoints -= (ushort)(message.boostPoint - pts);

            SendStatsUpgradeResultMessage(client, message.boostPoint);
            client.Character.RefreshStats();
        }

        public static void SendStatsUpgradeResultMessage(IPacketReceiver client, short usedpts)
        {
            client.Send(new StatsUpgradeResultMessage(usedpts));
        }
    }
}