using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Communication
{
    [Serializable]
    [ActiveRecord("emoticon")]
    [AttributeAssociatedFile("Emoticons")]
    [D2OClass("Emoticon", "com.ankamagames.dofus.datacenter.communication")]
    public sealed class EmoticonRecord : DataBaseRecord<EmoticonRecord>
    {

        [D2OField("id")]
        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public uint Id
        {
            get;
            set;
        }

        [D2OField("nameId")]
        [Property("NameId")]
        public uint NameId
        {
            get;
            set;
        }

        [D2OField("shortcutId")]
        [Property("ShortcutId")]
        public uint ShortcutId
        {
            get;
            set;
        }

        [D2OField("defaultAnim")]
        [Property("DefaultAnim")]
        public String DefaultAnim
        {
            get;
            set;
        }

        [D2OField("instant")]
        [Property("Instant")]
        public Boolean Instant
        {
            get;
            set;
        }

        [D2OField("eight_directions")]
        [Property("Eightdirections")]
        public Boolean Eightdirections
        {
            get;
            set;
        }

        [D2OField("aura")]
        [Property("Aura")]
        public Boolean Aura
        {
            get;
            set;
        }

        [D2OField("anims")]
        [Property("Anims", ColumnType = "Serializable")]
        public List<String> Anims
        {
            get;
            set;
        }

        [D2OField("cooldown")]
        [Property("Cooldown")]
        public uint Cooldown
        {
            get;
            set;
        }

        [Property]
        public uint Duration
        {
            get;
            set;
        }
    }
}