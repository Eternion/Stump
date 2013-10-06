

// Generated on 10/06/2013 17:58:55
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Npc", "com.ankamagames.dofus.datacenter.npcs")]
    [Serializable]
    public class Npc : IDataObject, IIndexedData
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
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }
        [D2OIgnore]
        public List<List<int>> DialogMessages
        {
            get { return dialogMessages; }
            set { dialogMessages = value; }
        }
        [D2OIgnore]
        public List<List<int>> DialogReplies
        {
            get { return dialogReplies; }
            set { dialogReplies = value; }
        }
        [D2OIgnore]
        public List<uint> Actions
        {
            get { return actions; }
            set { actions = value; }
        }
        [D2OIgnore]
        public uint Gender
        {
            get { return gender; }
            set { gender = value; }
        }
        [D2OIgnore]
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
        public List<AnimFunNpcData> AnimFunList
        {
            get { return animFunList; }
            set { animFunList = value; }
        }
    }
}