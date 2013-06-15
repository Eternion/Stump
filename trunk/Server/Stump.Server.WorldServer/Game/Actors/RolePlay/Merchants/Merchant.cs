using System;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Breeds;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Merchants
{
    public class Merchant : NamedActor
    {
        private readonly WorldMapMerchantRecord m_record;


        public Merchant(Character character)
        {
            m_record = new WorldMapMerchantRecord()
                {
                    CharacterId = character.Id,
                    Name = character.Name,
                    Map = character.Map,
                    Cell = character.Cell.Id,
                    Direction = (int) character.Direction,
                    EntityLook = character.Look,
                    MerchantSince = DateTime.Now,
                };

            Bag = new MerchantBag(this, character.MerchantBag);
            Position = character.Position.Clone();
        }

        public Merchant(WorldMapMerchantRecord record)
        {
            m_record = record;
            Bag = new MerchantBag(this);

            if (record.Map == null)
                throw new Exception("Merchant's map not found");

            Position = new ObjectPosition(
                record.Map,
                record.Map.Cells[m_record.Cell],
                (DirectionsEnum)m_record.Direction);
        }

        public WorldMapMerchantRecord Record
        {
            get
            {
                return m_record;
            }
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
            protected set;
        }

        public override EntityLook Look
        {
            get { return m_record.EntityLook; }
            set { m_record.EntityLook = value; }
        }

        public override string Name
        {
            get { return m_record.Name; }
        }

        public override bool CanBeSee(Maps.WorldObject byObj)
        {
            return base.CanBeSee(byObj) && !IsBagEmpty();
        }

        public bool IsBagEmpty()
        {
            return Bag.Count == 0;
        }

        public void LoadRecord()
        {
            Bag.LoadRecord();
        }

        public void Save()
        {
            if (Bag.IsDirty)
                Bag.Save();
        }

        #region Network

        public override GameContextActorInformations GetGameContextActorInformations()
        {
            return new GameRolePlayMerchantInformations(Id, Look, GetEntityDispositionInformations(), Name, 0);
        }

        #endregion
    }
}