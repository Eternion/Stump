

// Generated on 10/28/2013 14:03:21
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SoundAnimation", "com.ankamagames.dofus.datacenter.sounds")]
    [Serializable]
    public class SoundAnimation : IDataObject, IIndexedData
    {
        public uint id;
        public String name;
        public String label;
        public String filename;
        public int volume;
        public int rolloff;
        public int automationDuration;
        public int automationVolume;
        public int automationFadeIn;
        public int automationFadeOut;
        public Boolean noCutSilence;
        public uint startFrame;
        public String MODULE = "SoundAnimations";
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
        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        [D2OIgnore]
        public String Label
        {
            get { return label; }
            set { label = value; }
        }
        [D2OIgnore]
        public String Filename
        {
            get { return filename; }
            set { filename = value; }
        }
        [D2OIgnore]
        public int Volume
        {
            get { return volume; }
            set { volume = value; }
        }
        [D2OIgnore]
        public int Rolloff
        {
            get { return rolloff; }
            set { rolloff = value; }
        }
        [D2OIgnore]
        public int AutomationDuration
        {
            get { return automationDuration; }
            set { automationDuration = value; }
        }
        [D2OIgnore]
        public int AutomationVolume
        {
            get { return automationVolume; }
            set { automationVolume = value; }
        }
        [D2OIgnore]
        public int AutomationFadeIn
        {
            get { return automationFadeIn; }
            set { automationFadeIn = value; }
        }
        [D2OIgnore]
        public int AutomationFadeOut
        {
            get { return automationFadeOut; }
            set { automationFadeOut = value; }
        }
        [D2OIgnore]
        public Boolean NoCutSilence
        {
            get { return noCutSilence; }
            set { noCutSilence = value; }
        }
        [D2OIgnore]
        public uint StartFrame
        {
            get { return startFrame; }
            set { startFrame = value; }
        }
    }
}