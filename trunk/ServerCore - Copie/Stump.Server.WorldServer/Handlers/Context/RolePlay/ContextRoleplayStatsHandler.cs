
using System;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Breeds;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class ContextHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (StatsUpgradeRequestMessage))]
        public static void HandleStatsUpgradeRequestMessage(WorldClient client, StatsUpgradeRequestMessage message)
        {
            var statsid = (CaracteristicsIdEnum) message.statId;

            if (statsid < CaracteristicsIdEnum.Strength ||
                statsid > CaracteristicsIdEnum.Intelligence)
                throw new Exception("Wrong statsid");

            if (message.boostPoint <= 0)
                throw new Exception("Client given 0 as boostpoint. Forbidden value.");

            BaseBreed breed = BreedManager.GetBreed(client.ActiveCharacter.BreedId);
            int neededpts = breed.GetNeededPointForStats(client.ActiveCharacter.Stats[statsid.ToString()].Base, statsid);

            var boost = (int) (message.boostPoint/neededpts);

            if (boost < 0)
                throw new Exception("Client is attempt to use more points that he has.");

            // Exception for Sacrieur Vitality * 2
            if (breed.Id == PlayableBreedEnum.Sacrieur && statsid == CaracteristicsIdEnum.Vitality)
                boost *= 2;

            client.ActiveCharacter.Stats[statsid.ToString()].Base += boost;

            SendStatsUpgradeResultMessage(client, message.boostPoint);
            CharacterHandler.SendCharacterStatsListMessage(client);
        }

        public static void SendStatsUpgradeResultMessage(WorldClient client, uint usedpts)
        {
            client.Send(new StatsUpgradeResultMessage(usedpts));
        }
    }
}