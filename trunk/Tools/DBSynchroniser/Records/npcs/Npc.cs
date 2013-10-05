 


// Generated on 10/06/2013 01:10:59
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("Npcs")]
    public class NpcRecord : ID2ORecord
    {
        private const String MODULE = "Npcs";
        public int id;
        public uint nameId;
        public List<List<int>> dialogMessages;
        public List<List<int>> dialogReplies;
        public List<uint> actions;
        public uint gender;
        public String look;
        public int tokenShop;
        public List<AnimFunNpcData> animFunList;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

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
        public byte[] DialogMessagesBin
        {
            get { return m_dialogMessagesBin; }
            set
            {
                m_dialogMessagesBin = value;
                dialogMessages = value == null ? null : value.ToObject<List<List<int>>>();
            }
        }

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
        public byte[] DialogRepliesBin
        {
            get { return m_dialogRepliesBin; }
            set
            {
                m_dialogRepliesBin = value;
                dialogReplies = value == null ? null : value.ToObject<List<List<int>>>();
            }
        }

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
        public byte[] ActionsBin
        {
            get { return m_actionsBin; }
            set
            {
                m_actionsBin = value;
                actions = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        public uint Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public String Look
        {
            get { return look; }
            set { look = value; }
        }

        public int TokenShop
        {
            get { return tokenShop; }
            set { tokenShop = value; }
        }

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
        public byte[] AnimFunListBin
        {
            get { return m_animFunListBin; }
            set
            {
                m_animFunListBin = value;
                animFunList = value == null ? null : value.ToObject<List<AnimFunNpcData>>();
            }
        }

        public void AssignFields(object obj)
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
        
        public object CreateObject()
        {
            var obj = new Npc();
            
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