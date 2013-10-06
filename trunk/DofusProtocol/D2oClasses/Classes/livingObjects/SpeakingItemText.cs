

// Generated on 10/06/2013 17:58:55
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SpeakingItemText", "com.ankamagames.dofus.datacenter.livingObjects")]
    [Serializable]
    public class SpeakingItemText : IDataObject
    {
        private const String MODULE = "SpeakingItemsText";
        public int textId;
        public float textProba;
        public uint textStringId;
        public int textLevel;
        public int textSound;
        public String textRestriction;
        [D2OIgnore]
        public int TextId
        {
            get { return textId; }
            set { textId = value; }
        }
        [D2OIgnore]
        public float TextProba
        {
            get { return textProba; }
            set { textProba = value; }
        }
        [D2OIgnore]
        public uint TextStringId
        {
            get { return textStringId; }
            set { textStringId = value; }
        }
        [D2OIgnore]
        public int TextLevel
        {
            get { return textLevel; }
            set { textLevel = value; }
        }
        [D2OIgnore]
        public int TextSound
        {
            get { return textSound; }
            set { textSound = value; }
        }
        [D2OIgnore]
        public String TextRestriction
        {
            get { return textRestriction; }
            set { textRestriction = value; }
        }
    }
}