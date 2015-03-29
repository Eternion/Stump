using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Mounts;
using Stump.Server.WorldServer.Game.Guilds;

namespace Stump.Server.WorldServer.Game.Maps.Paddocks
{
    public class Paddock
    {
        private readonly WorldMapPaddockRecord m_record;

        public Paddock(WorldMapPaddockRecord record)
        {
            m_record = record;

            if (record.Map == null)
                throw new Exception(string.Format("Paddock's map({0}) not found", record.MapId));

            StabledMounts = MountManager.Instance.TryGetMountsByPaddockId(record.Id, true).Select(x => PaddockManager.Instance.LoadMount(x)).ToList();
            PaddockedMounts = MountManager.Instance.TryGetMountsByPaddockId(record.Id, false).Select(x => PaddockManager.Instance.LoadMount(x)).ToList();
        }

        public WorldMapPaddockRecord Record
        {
            get
            {
                return m_record;
            }
        }

        public int Id
        {
            get { return m_record.Id; }
            protected set { m_record.Id = value; }
        }

        public Guild Guild
        {
            get { return m_record.Guild; }
            protected set { m_record.Guild = value; }
        }

        public Map Map
        {
            get { return m_record.Map; }
            protected set { m_record.Map = value; }
        }

        public List<Mount> StabledMounts
        {
            get;
            private set;
        }

        public List<Mount> PaddockedMounts
        {
            get;
            private set;
        }

        public uint MaxOutdoorMount
        {
            get { return m_record.MaxOutdoorMount; }
            protected set { m_record.MaxOutdoorMount = value; }
        }

        public uint MaxItems
        {
            get { return m_record.MaxItems; }
            protected set { m_record.MaxItems = value; }
        }

        public bool Abandonned
        {
            get { return m_record.Abandonned; }
            protected set { m_record.Abandonned = value; }
        }

        public bool OnSale
        {
            get { return m_record.OnSale; }
            protected set { m_record.OnSale = value; }
        }

        public bool Locked
        {
            get { return m_record.Locked; }
            protected set { m_record.Locked = value; }
        }

        public int Price
        {
            get { return m_record.Price; }
            protected set { m_record.Price = value; }
        }

        public bool IsRecordDirty
        {
            get;
            set;
        }

        public void Save()
        {
            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                WorldServer.Instance.DBAccessor.Database.Update(Record);
                IsRecordDirty = false;
            });
        }

        public bool IsPaddockOwner(Character character)
        {
            if (Guild == null)
                return true;

            if (character.Guild == null)
                return false;

            return character.Guild.Id == Guild.Id;
        }

        public void AddMountToPaddock(Mount mount)
        {
            IsRecordDirty = true;
            PaddockedMounts.Add(mount);

            MountManager.Instance.LinkMountToPaddock(this, mount, false);
        }

        public void RemoveMountFromPaddock(Mount mount)
        {
            IsRecordDirty = true;
            PaddockedMounts.Remove(mount);

            MountManager.Instance.UnlinkMountFromPaddock(mount);
        }

        public void AddMountToStable(Mount mount)
        {
            IsRecordDirty = true;
            StabledMounts.Add(mount);

            MountManager.Instance.LinkMountToPaddock(this, mount, true);
        }

        public void RemoveMountFromStable(Mount mount)
        {
            IsRecordDirty = true;
            StabledMounts.Remove(mount);

            MountManager.Instance.UnlinkMountFromPaddock(mount);
        }

        public Mount GetPaddockedMount(int mountId)
        {
            return PaddockedMounts.FirstOrDefault(x => x.Id == mountId);
        }

        public Mount GetStabledMount(int mountId)
        {
            return StabledMounts.FirstOrDefault(x => x.Id == mountId);
        }

        #region Network

        public PaddockPropertiesMessage GetPaddockPropertiesMessage()
        {
            var informations = new PaddockInformations((short)MaxOutdoorMount, (short)MaxItems);

            if (Abandonned)
                informations = new PaddockAbandonnedInformations((short)MaxOutdoorMount, (short)MaxItems, Price, Locked, Guild.Id);
            else if (OnSale)
                informations = new PaddockBuyableInformations((short)MaxOutdoorMount, (short)MaxItems, Price, Locked);
            else if (Guild != null)
                informations = new PaddockPrivateInformations((short)MaxOutdoorMount, (short)MaxItems, Price, Locked, Guild.Id, Guild.GetGuildInformations());
            else
                informations = new PaddockContentInformations((short)MaxOutdoorMount, (short)MaxItems, Id, (short)Map.Position.X, (short)Map.Position.Y,
                Map.Id, (short)Map.SubArea.Id, Abandonned, PaddockedMounts.Select(x => x.GetMountInformationsForPaddock()));

            return new PaddockPropertiesMessage(informations);
        }

        #endregion
    }
}
