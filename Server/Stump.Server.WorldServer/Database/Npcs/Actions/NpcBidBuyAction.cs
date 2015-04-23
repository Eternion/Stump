using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;

namespace Stump.Server.WorldServer.Database.Npcs.Actions
{
    [Discriminator(Discriminator, typeof(NpcActionDatabase), typeof(NpcActionRecord))]
    public class NpcBidBuyAction : NpcActionDatabase
    {
        public const string Discriminator = "Bid";

        public NpcBidBuyAction(NpcActionRecord record)
            : base(record)
        {
        }

        public override NpcActionTypeEnum ActionType
        {
            get { return NpcActionTypeEnum.ACTION_BUY; }
        }

        public override void Execute(Npc npc, Character character)
        {
            character.Client.Send(new ExchangeStartedBidBuyerMessage(new SellerBuyerDescriptor(new[] {1, 10, 100}, new[] {2}, 2, 0, 63, npc.Id, 7)));

            //RCV ExchangeBidHouseType -> ExchangeTypesExchangerDescriptionForUserMessage
        }
    }
}
