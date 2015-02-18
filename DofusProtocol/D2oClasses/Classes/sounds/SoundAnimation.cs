

// Generated on 02/18/2015 11:06:02
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
        public String MODULE = "SoundAnimations";
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
        public String Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        [D2OIgnore]
        public String Label
        {
            get { return this.label; }
            set { this.label = value; }
        }
        [D2OIgnore]
        public String Filename
        {
            get { return this.filename; }
            set { this.filename = value; }
        }
        [D2OIgnore]
        public int Volume
        {
            get { return this.volume; }
            set { this.volume = value; }
        }
        [D2OIgnore]
        public int Rolloff
        {
            get { return this.rolloff; }
            set { this.rolloff = value; }
        }
        [D2OIgnore]
        public int AutomationDuration
        {
            get { return this.automationDuration; }
            set { this.automationDuration = value; }
        }
        [D2OIgnore]
        public int AutomationVolume
        {
            get { return this.automationVolume; }
            set { this.automationVolume = value; }
        }
        [D2OIgnore]
        public int AutomationFadeIn
        {
            get { return this.automationFadeIn; }
            set { this.automationFadeIn = value; }
        }
        [D2OIgnore]
        public int AutomationFadeOut
        {
            get { return this.automationFadeOut; }
            set { this.automationFadeOut = value; }
        }
        [D2OIgnore]
        public Boolean NoCutSilence
        {
            get { return this.noCutSilence; }
            set { this.noCutSilence = value; }
        }
        [D2OIgnore]
        public uint StartFrame
        {
            get { return this.startFrame; }
            set { this.startFrame = value; }
        }
    }
}