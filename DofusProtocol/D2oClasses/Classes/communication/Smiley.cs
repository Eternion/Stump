

// Generated on 12/20/2015 18:01:10
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Smiley", "com.ankamagames.dofus.datacenter.communication")]
    [Serializable]
    public class Smiley : IDataObject, IIndexedData
    {
        public const String MODULE = "Smileys";
        public uint id;
        public uint order;
        public String gfxId;
        public Boolean forPlayers;
        public List<String> triggers;
        public uint referenceId;
        public uint categoryId;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public uint Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public uint Order
        {
            get { return this.order; }
            set { this.order = value; }
        }
        [D2OIgnore]
        public String GfxId
        {
            get { return this.gfxId; }
            set { this.gfxId = value; }
        }
        [D2OIgnore]
        public Boolean ForPlayers
        {
            get { return this.forPlayers; }
            set { this.forPlayers = value; }
        }
        [D2OIgnore]
        public List<String> Triggers
        {
            get { return this.triggers; }
            set { this.triggers = value; }
        }
        [D2OIgnore]
        public uint ReferenceId
        {
            get { return this.referenceId; }
            set { this.referenceId = value; }
        }
        [D2OIgnore]
        public uint CategoryId
        {
            get { return this.categoryId; }
            set { this.categoryId = value; }
        }
    }
}