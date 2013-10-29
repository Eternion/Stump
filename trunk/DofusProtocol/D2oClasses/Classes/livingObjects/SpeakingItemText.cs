

// Generated on 10/28/2013 14:03:19
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SpeakingItemText", "com.ankamagames.dofus.datacenter.livingObjects")]
    [Serializable]
    public class SpeakingItemText : IDataObject, IIndexedData
    {
        private const String MODULE = "SpeakingItemsText";
        public int textId;
        public double textProba;
        [I18NField]
        public uint textStringId;
        public int textLevel;
        public int textSound;
        public String textRestriction;
        int IIndexedData.Id
        {
            get { return (int)textId; }
        }
        [D2OIgnore]
        public int TextId
        {
            get { return textId; }
            set { textId = value; }
        }
        [D2OIgnore]
        public double TextProba
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