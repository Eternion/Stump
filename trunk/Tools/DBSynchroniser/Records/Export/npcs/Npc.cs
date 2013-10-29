 


// Generated on 10/28/2013 14:03:25
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
    [TableName("Npcs")]
    [D2OClass("Npc", "com.ankamagames.dofus.datacenter.npcs")]
    public class NpcRecord : ID2ORecord
    {
        private const String MODULE = "Npcs";
        public int id;
        [I18NField]
        public uint nameId;
        public List<List<int>> dialogMessages;
        public List<List<int>> dialogReplies;
        public List<uint> actions;
        public uint gender;
        public String look;
        public int tokenShop;
        public List<AnimFunNpcData> animFunList;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
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
        [Ignore]
        public List<List<int>> DialogMessages
        {
            get { return dialogMessages; }
            set
            {
                dialogMessages = value;
                m_dialogMessagesBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_dialogMessagesBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] DialogMessagesBin
        {
            get { return m_dialogMessagesBin; }
            set
            {
                m_dialogMessagesBin = value;
                dialogMessages = value == null ? null : value.ToObject<List<List<int>>>();
            }
        }

        [D2OIgnore]
        [Ignore]
        public List<List<int>> DialogReplies
        {
            get { return dialogReplies; }
            set
            {
                dialogReplies = value;
                m_dialogRepliesBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_dialogRepliesBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] DialogRepliesBin
        {
            get { return m_dialogRepliesBin; }
            set
            {
                m_dialogRepliesBin = value;
                dialogReplies = value == null ? null : value.ToObject<List<List<int>>>();
            }
        }

        [D2OIgnore]
        [Ignore]
        public List<uint> Actions
        {
            get { return actions; }
            set
            {
                actions = value;
                m_actionsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_actionsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] ActionsBin
        {
            get { return m_actionsBin; }
            set
            {
                m_actionsBin = value;
                actions = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        [D2OIgnore]
        public uint Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        [D2OIgnore]
        [NullString]
        public String Look
        {
            get { return look; }
            set { look = value; }
        }

        [D2OIgnore]
        public int TokenShop
        {
            get { return tokenShop; }
            set { tokenShop = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<AnimFunNpcData> AnimFunList
        {
            get { return animFunList; }
            set
            {
                animFunList = value;
                m_animFunListBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_animFunListBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] AnimFunListBin
        {
            get { return m_animFunListBin; }
            set
            {
                m_animFunListBin = value;
                animFunList = value == null ? null : value.ToObject<List<AnimFunNpcData>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Npc)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            DialogMessages = castedObj.dialogMessages;
            DialogReplies = castedObj.dialogReplies;
            Actions = castedObj.actions;
            Gender = castedObj.gender;
            Look = castedObj.look;
            TokenShop = castedObj.tokenShop;
            AnimFunList = castedObj.animFunList;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (Npc)parent : new Npc();
            obj.id = Id;
            obj.nameId = NameId;
            obj.dialogMessages = DialogMessages;
            obj.dialogReplies = DialogReplies;
            obj.actions = Actions;
            obj.gender = Gender;
            obj.look = Look;
            obj.tokenShop = TokenShop;
            obj.animFunList = AnimFunList;
            return obj;
        
        }
    }
}