using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Mounts;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Mounts;
using Stump.Server.WorldServer.Game.Guilds;

namespace Stump.Server.WorldServer.Game.Maps.Paddocks
{
    public class Paddock
    {
        private readonly List<Mount> m_mounts;

        public Paddock(WorldMapPaddockRecord record)
        {
            Record = record;

            if (record.Map == null)
                throw new Exception(string.Format("Paddock's map({0}) not found", record.MapId));

            m_mounts = Record.Mounts.Select(x => new Mount(x)).ToList();
        }

        private WorldMapPaddockRecord Record
        {
            get;
        }

        public int Id => Record.Id;

        public Guild Guild
        {
            get { return Record.Guild; }
            protected set
            {
                IsRecordDirty = true;
                Record.Guild = value;
            }
        }

        public Map Map
        {
            get { return Record.Map; }
            protected set
            {
                IsRecordDirty = true;
                Record.Map = value;
            }
        }

        public ReadOnlyCollection<Mount> PaddockedMounts => m_mounts.AsReadOnly();

        public uint MaxOutdoorMount
        {
            get { return Record.MaxOutdoorMount; }
            protected set
            {
                IsRecordDirty = true;
                Record.MaxOutdoorMount = value;
            }
        }

        public uint MaxItems
        {
            get { return Record.MaxItems; }
            protected set
            {
                IsRecordDirty = true;
                Record.MaxItems = value;
            }
        }

        public bool Abandonned
        {
            get { return Record.Abandonned; }
            protected set
            {
                IsRecordDirty = true;
                Record.Abandonned = value;
            }
        }

        public bool OnSale
        {
            get { return Record.OnSale; }
            protected set
            {
                IsRecordDirty = true;
                Record.OnSale = value;
            }
        }

        public bool Locked
        {
            get { return Record.Locked; }
            protected set
            {
                IsRecordDirty = true;
                Record.Locked = value;
            }
        }

        public int Price
        {
            get { return Record.Price; }
            protected set
            {
                IsRecordDirty = true;
                Record.Price = value;
            }
        }

        public bool IsRecordDirty
        {
            get;
            private set;
        }

        public bool IsPublicPaddock() => Guild == null;

        public void Save(ORM.Database database)
        {
            if (IsRecordDirty)
            {
                database.Update(Record);
                IsRecordDirty = false;

                foreach (var mount in PaddockedMounts.Where(x => x.IsDirty))
                    database.Save(mount);
            }
        }

        public bool IsPaddockOwner(Character character)
        {
            if (IsPublicPaddock())
                return true;

            return character.Guild?.Id == Guild.Id;
        }

        public void AddMountToStable(Mount mount)
        {
            if (mount.Paddock == null && !mount.IsInStable)
            {
                mount.Paddock = this;
                mount.IsInStable = true;
            }
        }

        public void RemoveMountFromStable(Mount mount)
        {
            if (mount.Paddock == this)
            {
                mount.Paddock = null;
                mount.IsInStable = false;
            }
        }

        public void AddMountToPaddock(Mount mount)
        {
            if (!mount.IsInStable)
            {
                IsRecordDirty = true;

                if (!IsPublicPaddock())
                {
                    m_mounts.Add(mount);
                    Record.Mounts.Add(mount.Record);
                }
                else
                    mount.Owner.AddPublicPaddockedMount(mount);

                mount.Paddock = this;
            }
        }

        public void RemoveMountFromPaddock(Mount mount)
        {
            if (mount.Paddock == this && !mount.IsInStable)
            {
                IsRecordDirty = true;

                if (!IsPublicPaddock())
                {
                    m_mounts.Remove(mount);
                    Record.Mounts.Remove(mount.Record);
                }
                else
                    mount.Owner.RemovePublicPaddockedMount(mount);

                mount.Paddock = null;
            }
        }

        public Mount GetPaddockedMount(Character character, int mountId)
        {
            return IsPublicPaddock() ? character.GetPublicPaddockedMount(mountId) : PaddockedMounts.FirstOrDefault(x => x.Id == mountId);
        }

        #region Network

        public PaddockPropertiesMessage GetPaddockPropertiesMessage()
        {
            PaddockInformations informations;

            if (Abandonned)
                informations = new PaddockAbandonnedInformations((short) MaxOutdoorMount, (short) MaxItems, Price, Locked, Guild.Id);
            else if (OnSale)
                informations = new PaddockBuyableInformations((short) MaxOutdoorMount, (short) MaxItems, Price, Locked);
            else if (Guild != null)
                informations = new PaddockPrivateInformations((short) MaxOutdoorMount, (short) MaxItems, Price, Locked, Guild.Id, Guild.GetGuildInformations());
            else
                informations = new PaddockContentInformations((short) MaxOutdoorMount, (short) MaxItems, Id, (short) Map.Position.X, (short) Map.Position.Y,
                    Map.Id, (short) Map.SubArea.Id, Abandonned, PaddockedMounts.Select(x => x.GetMountInformationsForPaddock()));

            return new PaddockPropertiesMessage(informations);
        }

        #endregion
    }
}