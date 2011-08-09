using System;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Handlers.Characters;

namespace Stump.Server.WorldServer.Handlers.Context.RolePlay
{
    public partial class ContextHandler : WorldHandlerContainer
    {
        /*[WorldHandler(StatsUpgradeRequestMessage.Id)]
        public static void HandleStatsUpgradeRequestMessage(WorldClient client, StatsUpgradeRequestMessage message)
        {
            var statsid = (CaracteristicsEnum) message.statId;

            if (statsid < CaracteristicsEnum.Strength ||
                statsid > CaracteristicsEnum.Intelligence)
                throw new Exception("Wrong statsid");

            if (message.boostPoint <= 0)
                throw new Exception("Client given 0 as boostpoint. Forbidden value.");

            BaseBreed breed = BreedManager.GetBreed(client.ActiveCharacter.BreedId);
            int neededpts = breed.GetNeededPointForStats(client.ActiveCharacter.Stats[statsid.ToString()].Base, statsid);

            var boost = (int) (message.boostPoint/neededpts);

            if (boost < 0)
                throw new Exception("Client is attempt to use more points that he has.");

            // Exception for Sacrieur Vitality * 2
            if (breed.Id == PlayableBreedEnum.Sacrieur && statsid == CaracteristicsEnum.Vitality)
                boost *= 2;

            client.ActiveCharacter.Stats[statsid.ToString()].Base += boost;

            SendStatsUpgradeResultMessage(client, message.boostPoint);
            CharacterHandler.SendCharacterStatsListMessage(client);
        }

        public static void SendStatsUpgradeResultMessage(WorldClient client, uint usedpts)
        {
            client.Send(new StatsUpgradeResultMessage(usedpts));
        }*/
    }
}