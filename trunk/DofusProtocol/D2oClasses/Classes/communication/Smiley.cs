
// Generated on 03/25/2013 19:24:32
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Smileys")]
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

        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint Order
        {
            get { return order; }
            set { order = value; }
        }

        public String GfxId
        {
            get { return gfxId; }
            set { gfxId = value; }
        }

        public Boolean ForPlayers
        {
            get { return forPlayers; }
            set { forPlayers = value; }
        }

        public List<String> Triggers
        {
            get { return triggers; }
            set { triggers = value; }
        }

    }
}