using System;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Breeds;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Merchants
{
    public class Merchant : NamedActor
    {
        private readonly WorldMapMerchantRecord m_record;

        public Merchant(WorldMapMerchantRecord record, MerchantBag bag)
        {
            m_record = record;
            Bag = bag;

            if (record.Map == null)
                throw new Exception("Merchant's map not found");

            Position = new ObjectPosition(
                record.Map,
                record.Map.Cells[m_record.Cell],
                (DirectionsEnum)m_record.Direction);
        }

        public override int Id
        {
            get { return m_record.CharacterId; }
            protected set { m_record.CharacterId = value; }
        }

        public override ObjectPosition Position
        {
            get;
            protected set;
        }

        public MerchantBag Bag
        {
            get;
            set;
        }

        public override EntityLook Look
        {
            get { return m_record.Look; }
            set { m_record.Look = value; }
        }

        public override string Name
        {
            get { return m_record.Name; }
        }

        #region Network

        public override GameContextActorInformations GetGameContextActorInformations()
        {
            return new GameRolePlayMerchantInformations(Id, Look, GetEntityDispositionInformations(), Name, 0);
        }

        #endregion
    }
}