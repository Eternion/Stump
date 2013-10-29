 


// Generated on 10/28/2013 14:03:22
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("Emoticons")]
    [D2OClass("Emoticon", "com.ankamagames.dofus.datacenter.communication")]
    public class EmoticonRecord : ID2ORecord
    {
        private const String MODULE = "Emoticons";
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

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint ShortcutId
        {
            get { return shortcutId; }
            set { shortcutId = value; }
        }

        [D2OIgnore]
        public uint Order
        {
            get { return order; }
            set { order = value; }
        }

        [D2OIgnore]
        [NullString]
        public String DefaultAnim
        {
            get { return defaultAnim; }
            set { defaultAnim = value; }
        }

        [D2OIgnore]
        public Boolean Persistancy
        {
            get { return persistancy; }
            set { persistancy = value; }
        }

        [D2OIgnore]
        public Boolean Eight_directions
        {
            get { return eight_directions; }
            set { eight_directions = value; }
        }

        [D2OIgnore]
        public Boolean Aura
        {
            get { return aura; }
            set { aura = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<String> Anims
        {
            get { return anims; }
            set
            {
                anims = value;
                m_animsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_animsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] AnimsBin
        {
            get { return m_animsBin; }
            set
            {
                m_animsBin = value;
                anims = value == null ? null : value.ToObject<List<String>>();
            }
        }

        [D2OIgnore]
        public uint Cooldown
        {
            get { return cooldown; }
            set { cooldown = value; }
        }

        [D2OIgnore]
        public uint Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        [D2OIgnore]
        public uint Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Emoticon)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            ShortcutId = castedObj.shortcutId;
            Order = castedObj.order;
            DefaultAnim = castedObj.defaultAnim;
            Persistancy = castedObj.persistancy;
            Eight_directions = castedObj.eight_directions;
            Aura = castedObj.aura;
            Anims = castedObj.anims;
            Cooldown = castedObj.cooldown;
            Duration = castedObj.duration;
            Weight = castedObj.weight;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (Emoticon)parent : new Emoticon();
            obj.id = Id;
            obj.nameId = NameId;
            obj.shortcutId = ShortcutId;
            obj.order = Order;
            obj.defaultAnim = DefaultAnim;
            obj.persistancy = Persistancy;
            obj.eight_directions = Eight_directions;
            obj.aura = Aura;
            obj.anims = Anims;
            obj.cooldown = Cooldown;
            obj.duration = Duration;
            obj.weight = Weight;
            return obj;
        
        }
    }
}