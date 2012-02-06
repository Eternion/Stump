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
            uint neededpts = breed.GetNeededPointForStats(client.ActiveCharacter.Stats[m_statsEnumRelations[statsid]].Base, statsid);

            var boost = (short) (message.boostPoint/ (double)neededpts);

            if (boost < 1 || message.boostPoint > client.ActiveCharacter.StatsPoints)
                throw new Exception("Client is attempt to use more points that he has.");

            client.ActiveCharacter.Stats[m_statsEnumRelations[statsid]].Base += boost;
            client.ActiveCharacter.StatsPoints -= (ushort)message.boostPoint;

            SendStatsUpgradeResultMessage(client, message.boostPoint);
            client.ActiveCharacter.RefreshStats();
        }

        public static void SendStatsUpgradeResultMessage(IPacketReceiver client, short usedpts)
        {
            client.Send(new StatsUpgradeResultMessage(usedpts));
        }
    }
}