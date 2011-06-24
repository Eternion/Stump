
using System;
using Stump.Core.Reflection;
using Stump.Database.WorldServer;
using Stump.Database.WorldServer.Character;
using Stump.DofusProtocol.Classes;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Helpers;
using Stump.Server.WorldServer.World.Entities.Actors;
using Stump.Server.WorldServer.World.Guilds;
using Stump.Server.WorldServer.World.Storages;

namespace Stump.Server.WorldServer.World.Entities.Merchants
{
    public class Merchant : Actor, IInventoryOwner,ITrader
    {

        public Merchant(CharacterRecord record)
            : base(record.Id, record.Name, CharacterManager.GetStuffedCharacterLook(record), new VectorIsometric(Singleton<World>.Instance.GetMap(record.MapId), record.CellId, record.Direction))
        {
            m_record = record;
            Inventory = new Inventory(this, m_record.Inventory);
            //Guild = record.Guild;
        }

        private readonly CharacterRecord m_record;

        public Inventory Inventory
        {
            get;
            set;
        }

        public Guild Guild
        {
            get;
            set;
        }

        public override GameRolePlayActorInformations ToGameRolePlayActor()
        {
            if (Guild == null)
                return new GameRolePlayMerchantInformations((int)Id, Look.EntityLook, GetEntityDispositionInformations(), Name, 0);
            else
                return new GameRolePlayMerchantWithGuildInformations((int)Id, Look.EntityLook, GetEntityDispositionInformations(), Name, 0, Guild.GetInfo());
        }

        public void Trade()
        {
            throw new NotImplementedException();
        }
    }
}