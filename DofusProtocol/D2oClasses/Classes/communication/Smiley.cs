

// Generated on 10/28/2013 14:03:17
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
        private const String MODULE = "Smileys";
        public uint id;
        public uint order;
        public String gfxId;
        public Boolean forPlayers;
        public List<String> triggers;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public uint Order
        {
            get { return order; }
            set { order = value; }
        }
        [D2OIgnore]
        public String GfxId
        {
            get { return gfxId; }
            set { gfxId = value; }
        }
        [D2OIgnore]
        public Boolean ForPlayers
        {
            get { return forPlayers; }
            set { forPlayers = value; }
        }
        [D2OIgnore]
        public List<String> Triggers
        {
            get { return triggers; }
            set { triggers = value; }
        }
    }
}