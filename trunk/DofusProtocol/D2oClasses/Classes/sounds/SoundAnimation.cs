
// Generated on 03/02/2013 21:17:47
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SoundAnimations")]
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

        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String Label
        {
            get { return label; }
            set { label = value; }
        }

        public String Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        public int Volume
        {
            get { return volume; }
            set { volume = value; }
        }

        public int Rolloff
        {
            get { return rolloff; }
            set { rolloff = value; }
        }

        public int AutomationDuration
        {
            get { return automationDuration; }
            set { automationDuration = value; }
        }

        public int AutomationVolume
        {
            get { return automationVolume; }
            set { automationVolume = value; }
        }

        public int AutomationFadeIn
        {
            get { return automationFadeIn; }
            set { automationFadeIn = value; }
        }

        public int AutomationFadeOut
        {
            get { return automationFadeOut; }
            set { automationFadeOut = value; }
        }

        public Boolean NoCutSilence
        {
            get { return noCutSilence; }
            set { noCutSilence = value; }
        }

        public uint StartFrame
        {
            get { return startFrame; }
            set { startFrame = value; }
        }

    }
}