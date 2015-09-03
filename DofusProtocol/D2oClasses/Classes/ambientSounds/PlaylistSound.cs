

// Generated on 09/01/2015 11:16:30
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("PlaylistSound", "com.ankamagames.dofus.datacenter.ambientSounds")]
    [Serializable]
    public class PlaylistSound : IDataObject, IIndexedData
    {
        public const String MODULE = "PlaylistSounds";
        public String id;
        public int volume;
        public int channel = 0;
        int IIndexedData.Id
        {
            get { return int.Parse(id); }
        }
        [D2OIgnore]
        public String Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public int Volume
        {
            get { return this.volume; }
            set { this.volume = value; }
        }
        [D2OIgnore]
        public int Channel
        {
            get { return this.channel; }
            set { this.channel = value; }
        }
    }
}