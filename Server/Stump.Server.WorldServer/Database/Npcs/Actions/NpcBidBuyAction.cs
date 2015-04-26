using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Exchanges.BidHouse;

namespace Stump.Server.WorldServer.Database.Npcs.Actions
{
    [Discriminator(Discriminator, typeof(NpcActionDatabase), typeof(NpcActionRecord))]
    public class NpcBidBuyAction : NpcActionDatabase
    {
        public const string Discriminator = "BidBuy";

        public NpcBidBuyAction(NpcActionRecord record)
            : base(record)
        {
        }

        public override NpcActionTypeEnum[] ActionType
        {
            get { return new [] { NpcActionTypeEnum.ACTION_BUY }; }
        }

        private IEnumerable<int> m_types;

        /// <summary>
        /// Parameter 0
        /// </summary>
        private string TypesId
        {
            get { return Record.GetParameter<string>(0); }
            set { Record.SetParameter(0, value); }
        }

        /// <summary>
        /// Parameter 1
        /// </summary>
        private int MaxItemLevel
        {
            get { return Record.GetParameter<int>(1); }
            set { Record.SetParameter(1, value); }
        }

        public IEnumerable<int> Types
        {
            get { return m_types ?? (m_types = TypesId.Split('|').Select(int.Parse)); }
            set
            {
                m_types = value;
                TypesId = String.Join("|", value);
            }
        }

        public override void Execute(Npc npc, Character character)
        {
            var exchange = new BidHouseExchange(character, npc, Types, MaxItemLevel, true);
            exchange.Open();
        }
    }
}
