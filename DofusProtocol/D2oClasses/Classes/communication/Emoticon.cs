

// Generated on 12/29/2014 16:35:07
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Emoticon", "com.ankamagames.dofus.datacenter.communication")]
    [Serializable]
    public class Emoticon : IDataObject, IIndexedData
    {
        public const String MODULE = "Emoticons";
        public uint id;
        [I18NField]
        public uint nameId;
        [I18NField]
        public uint shortcutId;
        public uint order;
        public String defaultAnim;
        public Boolean persistancy;
        public Boolean eight_directions;
        public Boolean aura;
        public List<String> anims;
        public uint cooldown = 1000;
        public uint duration = 0;
        public uint weight;
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
        public uint NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public uint ShortcutId
        {
            get { return this.shortcutId; }
            set { this.shortcutId = value; }
        }
        [D2OIgnore]
        public uint Order
        {
            get { return this.order; }
            set { this.order = value; }
        }
        [D2OIgnore]
        public String DefaultAnim
        {
            get { return this.defaultAnim; }
            set { this.defaultAnim = value; }
        }
        [D2OIgnore]
        public Boolean Persistancy
        {
            get { return this.persistancy; }
            set { this.persistancy = value; }
        }
        [D2OIgnore]
        public Boolean Eight_directions
        {
            get { return this.eight_directions; }
            set { this.eight_directions = value; }
        }
        [D2OIgnore]
        public Boolean Aura
        {
            get { return this.aura; }
            set { this.aura = value; }
        }
        [D2OIgnore]
        public List<String> Anims
        {
            get { return this.anims; }
            set { this.anims = value; }
        }
        [D2OIgnore]
        public uint Cooldown
        {
            get { return this.cooldown; }
            set { this.cooldown = value; }
        }
        [D2OIgnore]
        public uint Duration
        {
            get { return this.duration; }
            set { this.duration = value; }
        }
        [D2OIgnore]
        public uint Weight
        {
            get { return this.weight; }
            set { this.weight = value; }
        }
    }
}